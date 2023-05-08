using LXQJZ.UI.Effect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LXQJZ.Task
{
	public enum StepState
	{
		Running, Success
	}
	public class Step
	{
		public UnityAction Prepare;
		public UnityAction OnClickObj;
		public UnityAction OnClickBtn;
		public UnityAction OnClickTog;
		public Func<StepState> CheckState;
		public StepState State = StepState.Running;

		//高亮物体
		public List<GameObject> objList = new List<GameObject>();
		//点击按钮
		public List<Button> btnList = new List<Button>();
		//点击开关
		public List<Toggle> togList = new List<Toggle>();
		//文字提示
		public string tips = null;

		public Step()
		{
			StepManager.GetInstance().StepList.Add(this);
		}

		public void Clear()
		{
			OnClickBtn = null;
			OnClickObj = null;
			OnClickTog = null;
			objList.Clear();
			objList = null;
			btnList.Clear();
			btnList = null;
			togList.Clear();
			togList = null;
		}
	}
}

