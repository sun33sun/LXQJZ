﻿using QFramework;
using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskDryRing : TaskBase
	{
		bool isSuccess1 = false;

		public override void InitState()
		{
			modelName = "DryRing";
			base.InitState();
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.tips = "用清水冲洗戒指，将戒指擦干。";
			step1.objList.Add(GetObj("Ring_Diamond"));
			step1.Prepare += Prepare1;
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;
		}

		#region Step1

		void Prepare1()
		{
			RoamCamera.Instance.IsEnable = false;
			RoamCamera.Instance.MoveTo(new Vector3(1, 0.8f, -1.3f));
		}

		void ClickObj1()
		{
			ActionKit.Sequence()
				.Callback(() => { AnimStart("Ring_Diamond", "Ring_Diamond_Enter_WaterCup"); })
				.Delay(2, () => { AnimStart("Towel", "Towel_Clean_Ring_Diamond"); })
				.Delay(3, () => { isSuccess1 = true; })
				.Start(this);
		}

		StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}
		#endregion
	}
}