using QFramework;
using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskFixDiamond : TaskBase
	{
		bool isSuccess1 = false;
		bool isSuccess2 = false;

		public override void InitState()
		{
			modelName = "FixDiamond";
			base.InitState();
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.tips = "将钻石放在开好槽的爪镶底托上，准备开始镶嵌。";
			step1.objList.Add(GetObj("Diamond"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;

			Step step2 = new Step();
			step2.objList.Add(GetObj("NeedleNosePlier1"));
			step2.objList.Add(GetObj("NeedleNosePlier2"));
			step2.OnClickObj += ClickObj2;
			step2.CheckState += CheckState2;
		}

		#region Step1
		void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			ActionKit.Sequence()
				.Callback(() => { AnimStart("Diamond", "Diamond_FixTo_Ring_Polish"); })
				.Delay(2, () =>
				{
					isSuccess1 = true;
				}).Start(this);
		}

		StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}
		#endregion

		#region Step2
		void ClickObj2()
		{
			RoamCamera.Instance.IsEnable = false;
			GetObj("NeedleNosePlier2").GetComponent<ObjClickEvent>().SelfDestroy();
			ActionKit.Sequence()
				.Callback(() => { AnimStart("NeedleNosePlier", "NeedleNosePlier_Fix_Diamond"); })
				.Delay(6, () =>
				{
					isSuccess2 = true;
				}).Start(this);
		}

		StepState CheckState2()
		{
			return CheckState(isSuccess2);
		}
		#endregion
	}
}