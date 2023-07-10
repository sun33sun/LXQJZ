using LXQJZ.Task;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LXQJZ
{
	public class BaseEvent : MonoBehaviour
	{
		public bool isDestroy = false;
		public UnityAction OnClick;

		protected virtual void Awake()
		{
			StepManager.Register(this);
		}

		protected virtual void OnDestroy()
		{
			StepManager.Deregister(this);
		}

		public virtual void SelfDestroy(bool isInvoke = true)
		{
		}
	}
}
