using LXQJZ.UI;
using LXQJZ.UI.Effect;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.Task
{
	public class StepManager : BaseManager<StepManager>
	{
		public StepManager()
		{
			MonoMgr.GetInstance().AddUpdateListener(Update);
		}

		List<QuickOutline.Outline> outlines = new List<QuickOutline.Outline>();
		List<UIOutline> uIOutlines = new List<UIOutline>();

		private bool IsEnable = false;
		private static int _stepIndex = -1;
		/// <summary>
		/// 步骤索引
		/// </summary>
		public static int StepIndex
		{
			get
			{
				return _stepIndex;
			}

			set
			{
				_stepIndex = value;
			}
		}

		public List<Step> StepList = new List<Step>();
		public Step nowStep { get { return StepList[_stepIndex]; } }
		public List<GameObject> nowObjList { get { return StepList[_stepIndex].objList; } }
		public List<Button> nowBtnList { get { return StepList[_stepIndex].btnList; } }
		public List<Toggle> nowTogList { get { return StepList[_stepIndex].togList; } }

		private List<BaseEvent> eventList = null;

		private void Update()
		{
			if (!IsEnable)
				return;
			switch (nowStep.State)
			{
				case StepState.Running:
					nowStep.State = nowStep.CheckState();
					break;
				case StepState.Success:
					IsEnable = false;
					NextStep();
					break;
			}
		}

		public void ClearStep()
		{
			IsEnable = false;
			ClearEvent();
			for (int i = StepList.Count - 1; i > -1; i--)
			{
				StepList[i].Clear();
				StepList.RemoveAt(i);
			}
			ClearOutline();
		}

		private void ClearOutline()
		{
			for (int i = outlines.Count - 1; i > -1; i--)
			{
				MonoMgr.GetInstance().DoDestroy(outlines[i]);
				outlines.RemoveAt(i);
			}
			for (int i = uIOutlines.Count - 1; i > -1; i--)
			{
				MonoMgr.GetInstance().DoDestroy(uIOutlines[i]);
				uIOutlines.RemoveAt(i);
			}
		}

		/// <summary>
		/// 下一步
		/// </summary>
		private void NextStep()
		{
			if (_stepIndex < StepList.Count - 1)
			{
				_stepIndex++;
				ClearEvent();
				Prepare();
			}
			else
			{
				ClearStep();
				EventCenter.GetInstance().EventTrigger("下一个任务");
			}
		}

		public static void Register(BaseEvent baseEvent)
		{
			if (GetInstance().eventList == null)
				GetInstance().eventList = new List<BaseEvent>();
			if (!GetInstance().eventList.Contains(baseEvent))
				GetInstance().eventList.Add(baseEvent);
		}

		public static void Deregister(BaseEvent baseEvent)
		{
			if (GetInstance().eventList == null || !GetInstance().eventList.Contains(baseEvent))
				return;
			GetInstance().eventList.Remove(baseEvent);
		}

		public static void ClearEvent()
		{
			if (GetInstance().eventList == null)
				return;
			for (int i = GetInstance().eventList.Count - 1; i > -1; i--)
			{
				MonoMgr.GetInstance().DoDestroy(GetInstance().eventList[i]);
				GetInstance().eventList.RemoveAt(i);
			}
			GetInstance().eventList = null;
		}

		public void Start()
		{
			if (StepList == null || StepList.Count < 1)
				return;
			IsEnable = true;
			_stepIndex = 0;
			ClearEvent();
			Prepare();
		}

		public void Prepare()
		{
			for (int i = 0; i < nowObjList.Count; i++)
			{
				QuickOutline.Outline outline = nowObjList[i].AddComponent<QuickOutline.Outline>();
				outlines.Add(outline);
				if (nowStep.OnClickObj != null)
					nowObjList[i].AddComponent<ObjClickEvent>().OnClick += nowStep.OnClickObj;
			}
			for (int i = 0; i < nowBtnList.Count; i++)
			{
				Button nowBtn = nowBtnList[i];
				UIOutline uIOutline = nowBtn.gameObject.AddComponent<UIOutline>();
				uIOutlines.Add(uIOutline);
				if (nowStep.OnClickBtn != null)
					nowBtn.gameObject.AddComponent<UIClickEvent>().OnClick += nowStep.OnClickBtn;
			}
			for (int i = 0; i < nowTogList.Count; i++)
			{
				Toggle nowTog = nowTogList[i];
				UIOutline uIOutline = nowTogList[i].gameObject.AddComponent<UIOutline>();
				uIOutlines.Add(uIOutline);
				if (nowStep.OnClickBtn != null)
					nowTog.gameObject.AddComponent<UIClickEvent>().OnClick += nowStep.OnClickTog;
			}
			if (nowStep.tips != null)
				OnlineLabPanel.Instance.ShowOnlineTip(nowStep.tips);
			nowStep.Prepare?.Invoke();
			IsEnable = true;

			//if(nowObjList != null && nowObjList.Count > 0)
			//	RoamCamera.Instance.LookAt(nowObjList[0].transform, 1);
		}
	}
}

