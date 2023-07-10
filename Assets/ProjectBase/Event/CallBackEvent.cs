using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LXQJZ
{
	public class CallBackEvent : BaseEvent
	{
		UnityAction action;
		public string actionName;

		public void AddEndEvent(UnityAction callBack, string eventName)
		{
			if (isDestroy)
				return;
			actionName = eventName;
			if (callBack != null)
				action += callBack;
			action += RemoveCallBack;
			EventCenter.GetInstance().AddEventListener(actionName, action);
		}
		private void RemoveCallBack()
		{
			if (isDestroy)
				return;
			isDestroy = true;
			EventCenter.GetInstance().RemoveEventListener(actionName, action);
			Destroy(this);
		}
	}
}
