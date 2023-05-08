﻿using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskSoakingSO4 : TaskBase
	{
		bool isSuccess1 = false;

		public override void InitState()
		{
			modelName = "SoakingSO4";
			base.InitState();
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.tips = "请清除铸造过程中产生的杂质";
			step1.objList.Add(GetObj("Ring_Rough_Silver1"));
			step1.objList.Add(GetObj("Ring_Rough_Silver2"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;

		}

		#region Step1
		void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working1());
		}

		IEnumerator working1()
		{
			GetObj("Ring_Rough_Silver2").GetComponent<ObjClickEvent>().SelfDestroy();
			AnimStart("Ring_Rough_Silver", "Ring_Rough_Silver_Enter_SO4Cup");
			yield return new WaitForSeconds(3);
			isSuccess1 = true;
		}

		StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}
		#endregion
	}
}