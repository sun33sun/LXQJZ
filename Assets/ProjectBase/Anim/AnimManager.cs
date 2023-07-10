using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LXQJZ
{
	public class AnimManager : BaseManager<AnimManager>
	{
		Dictionary<Animator, IEnumerator> ieDic = new Dictionary<Animator, IEnumerator>();

		public static bool IsPlaying
		{
			get
			{
				foreach (var item in GetInstance().ieDic)
				{
					if (item.Value == null)
					{
						MonoMgr.GetInstance().StopCoroutine(item.Value);
						GetInstance().ieDic.Remove(item.Key);
					}
				}
				return GetInstance().ieDic.Count > 0;
			}
		}

		#region Animator
		public static void Play(GameObject obj, string clipName, float waitTime = 3)
		{
			Animator animator = obj.GetComponent<Animator>();
			if (GetInstance().ieDic.ContainsKey(animator))
				Stop(obj);
			IEnumerator newIe = GetInstance().PlayAsync(animator, clipName, waitTime);
			GetInstance().ieDic.Add(animator, newIe);
			MonoMgr.GetInstance().StartCoroutine(newIe);
		}

		public static void Stop(GameObject obj)
		{
			Animator animator = obj.GetComponent<Animator>();
			MonoMgr.GetInstance().StopCoroutine(GetInstance().ieDic[animator]);
			animator.speed = 0;
			if (GetInstance().ieDic.ContainsKey(animator))
				GetInstance().ieDic.Remove(animator);
		}
		
		public void StopAll()
		{
			ieDic.Clear();
			MonoMgr.GetInstance().StopAllCoroutine();
		}
		IEnumerator PlayAsync(Animator animator, string clipName, float waitTime = 3)
		{
			animator.speed = 1;
			animator.Play(clipName);
			yield return new WaitForSeconds(waitTime);
			//AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			//yield return new WaitUntil(() => {

			//	Debug.Log(stateInfo.normalizedTime);
			//	return stateInfo.normalizedTime >= 1 || stateInfo.normalizedTime <= 0;
			//});
			EventCenter.GetInstance().EventTrigger(clipName + "AnimEnd");
			Debug.Log(clipName + " End " + animator.name);
			Stop(animator.gameObject);
		}
		#endregion
	}
}
