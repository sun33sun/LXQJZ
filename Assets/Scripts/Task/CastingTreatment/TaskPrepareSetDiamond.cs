using QFramework;
using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskPrepareSetDiamond : TaskBase
	{
		bool isSuccess1 = false;
		bool isSuccess2 = false;

		public override void InitState()
		{
			modelName = "PrepareSetDiamond";
			base.InitState();
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.tips = "将选好的钻石放在爪镶上，确认一下钻石的腰部大概在爪子的什么位置。";
			step1.objList.Add(GetObj("Diamond"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;

			Step step2 = new Step();
			step2.tips = "将选好的钻石放在爪镶上，确认一下钻石的腰部大概在爪子的什么位置。";
			step2.objList.Add(GetObj("FlySaucerDril"));
			step2.OnClickObj += ClickObj2;
			step2.CheckState += CheckState2;
		}

		#region Step1
		void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			ActionKit.Sequence()
				.Callback(() => { AnimStart("Diamond", "Diamond_Approach_Ring_Polish"); })
				.Delay(2, () => { isSuccess1 = true; })
				.Start(this);
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
			ActionKit.Sequence()
				.Callback(() => { AnimStart("FlySaucerDril", "FlySaucerDril_Grave_Ring_Polish"); })
				.Delay(3, () => { isSuccess2 = true; })
				.Start(this);
		}

		StepState CheckState2()
		{
			return CheckState(isSuccess2);
		}
		#endregion
	}
}