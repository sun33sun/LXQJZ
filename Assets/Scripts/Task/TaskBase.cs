using LXQJZ.Task;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LXQJZ
{
	public enum ViewType
	{
		None, LookAt, Follow
	}

	public abstract class TaskBase : MonoBehaviour
	{
		ResLoader mResLoader = ResLoader.Allocate();
		public string modelName = null;
		public GameObject modelInstance = null;
		public GameObject modelPrefab = null;
		protected Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();

		[SerializeField] protected List<UIBehaviour> uiList;
		Dictionary<string, UIBehaviour> uiDic = new Dictionary<string, UIBehaviour>();

		protected virtual void Awake()
		{
			for (int i = 0; i < uiList.Count; i++)
			{
				UIBehaviour newUI = uiList[i];
				if (newUI == null)
				{
					Debug.Log(name + "存在空 UI对象");
					continue;
				}
				else if (uiDic.ContainsKey(newUI.name))
				{
					Debug.Log(name + "存在重复 UI对象：" + newUI.name);
					continue;
				}
				uiDic.Add(newUI.name, newUI);
			}
		}

		protected virtual void OnDisable()
		{
			StopAllCoroutines();
		}

		protected virtual GameObject GetObj(string objName)
		{
			GameObject needObj = null;
			objDic.TryGetValue(objName, out needObj);
			if (needObj == null)
				needObj = TaskToolManager.Instance.GetObj(objName);
			return needObj;
		}

		protected virtual T GetUI<T>(string uiName) where T : UIBehaviour
		{
			UIBehaviour ui = null;
			uiDic.TryGetValue(uiName, out ui);
			T t = ui as T;
			if (t != null)
			{
				return t;
			}
			else
			{
				if (ui == null)
					Debug.LogError(uiName);
				t = ui.gameObject.GetComponent<T>();
				return t;
			}
		}

		protected virtual void AnimCallBack(string objName, UnityAction callBack, string clipName, float waitTime = 3)
		{
			GameObject obj = GetObj(objName);
			if (callBack != null)
				obj.AddComponent<CallBackEvent>().AddEndEvent(callBack, clipName + "AnimEnd");
			AnimManager.Play(obj, clipName, waitTime);
			RoamCamera.Instance.LookAt(obj.transform, waitTime);
		}


		protected void AnimStart(string objName, string clipName, ViewType viewType = ViewType.LookAt)
		{
			GameObject animObj = GetObj(objName);
			switch (viewType)
			{
				case ViewType.None:
					break;
				case ViewType.LookAt:
					RoamCamera.Instance.LookAt(animObj.transform, 1);
					break;
				case ViewType.Follow:
					RoamCamera.Instance.Follow(animObj.transform);
					break;
			}

			Animator animator = animObj.GetComponent<Animator>();
			animator.speed = 1;
			animator.Play(clipName, 0, 0);


		}

		protected void AnimStop(string objName)
		{
			Animator animator = GetObj(objName).GetComponent<Animator>();
			animator.speed = 0;
		}

		protected virtual void ParticleStart(string objName, float waitTime)
		{
			ParticleSystem particle = GetObj(objName).GetComponent<ParticleSystem>();
			if (ParticleManager.particleList.Contains(particle))
				ParticleManager.particleList.Remove(particle);
			ParticleManager.particleList.Add(particle);
			particle.Play();
			ActionKit.Sequence()
				.Delay(waitTime,
				() =>
				{
					ParticleManager.particleList.Remove(particle);
					particle.Stop();
				}).Start(this);
		}

		protected virtual void ParticleCallBack(string objName, UnityAction callBack, int time)
		{
			GameObject obj = GetObj(objName);
			if (callBack != null)
				obj.AddComponent<CallBackEvent>().AddEndEvent(callBack, objName + "ParticleEnd" + time);
			ParticleManager.Play(obj, time);
		}

		protected virtual StepState CheckState(bool isSuccess)
		{
			if (isSuccess)
			{
				RoamCamera.Instance.IsEnable = true;
				return StepState.Success;
			}
			else
				return StepState.Running;
		}

		public virtual void InitState()
		{
			if (modelName == null || modelName.Equals(""))
				return;
			if (modelPrefab == null)
			{
				ResKit.Init();
				modelPrefab = mResLoader.LoadSync<GameObject>(modelName);
			}
			modelInstance = Instantiate(modelPrefab);
			modelInstance.name = modelName;
			modelInstance.SetActive(true);

			if (objDic != null)
			{
				if (objDic.ContainsKey(modelInstance.name))
					Destroy(objDic[modelInstance.name]);
				objDic.Clear();
			}

			Transform[] transArray = modelInstance.GetComponentsInChildren<Transform>();
			for (int i = 0; i < transArray.Length; i++)
			{
				objDic.Add(transArray[i].name, transArray[i].gameObject);
			}
		}

		public virtual void RegisterSteps()
		{
		}
	}
}

