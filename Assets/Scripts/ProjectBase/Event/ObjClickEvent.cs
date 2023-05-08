using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LXQJZ
{
	public class ObjClickEvent : BaseEvent
	{
		private void OnMouseUpAsButton()
		{
			SelfDestroy();
		}
		
		public override void SelfDestroy(bool isInvoke = true)
		{
			if (isDestroy)
				return;
			isDestroy = true;
			if(isInvoke)
				OnClick?.Invoke();
			OnClick = null;
			cakeslice.Outline outline = GetComponent<cakeslice.Outline>();
			if (outline != null)
				Destroy(outline);
			Destroy(this);
		}
	}
}
