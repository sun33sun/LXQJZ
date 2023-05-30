using QFramework;
using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskPolishAndCleanRing : TaskBase
	{
		bool isSuccess1 = false;
		bool isSuccess2 = false;

		protected override void OnDisable()
		{
			isSuccess1 = false;
			isSuccess2 = false;
			base.OnDisable();
		}

		public override void InitState()
		{
			modelName = "PolishAndCleanRing";
			base.InitState();
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.tips = "在打磨轮盘上涂抛光蜡，将戒指进行整体抛光。";
			step1.objList.Add(GetObj("Turntable2"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;

			Step step2 = new Step();
			step2.objList.Add(GetObj("hengwenshuiyuguo17"));
			step2.OnClickObj += ClickObj2;
			step2.CheckState += CheckState2;
		}

		#region Step1

		void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			ActionKit.Sequence()
				.Callback(() => { AnimStart("BoilCup", "BoilCup_Pour_Turntable2"); })
				.Delay(2, () =>{ AnimStart("Ring_Diamond", "Ring_Diamond_Approach_Turntable2"); })
				.Delay(3.1f, () => { isSuccess1 = true; })
				.Start(this);
		}

		StepState CheckState1()
		{
			if (isSuccess1)
				return StepState.Success;
			else
				return StepState.Running;
		}
		#endregion

		#region Step2
		void ClickObj2()
		{
			ActionKit.Sequence()
				.Callback(() => {
					GetObj("hengwenshuiyuguo14").SetActive(false);
					AnimStart("Ring_Diamond", "Ring_Diamond_Enter_UltrasonicCup");
				})
				.Delay(1, () =>{
					GetObj("hengwenshuiyuguo14").SetActive(true);
					AnimStart("UltrasonicCup", "UltrasonicCup_Clean_Loop");})
				.Delay(3, () => 
				{
					GetObj("hengwenshuiyuguo14").SetActive(false);
					AnimStop("UltrasonicCup");
					AnimStart("Ring_Diamond", "Ring_Diamond_From_UltrasonicCup_To_Up");
				})
				.Delay(2,() => 
				{
					GetObj("hengwenshuiyuguo14").SetActive(true);
					isSuccess2 = true;
					RoamCamera.Instance.MoveToOrigin();
				})
				.Start(this);
		}

		StepState CheckState2()
		{
			return CheckState(isSuccess2);
		}
		#endregion
	}
}