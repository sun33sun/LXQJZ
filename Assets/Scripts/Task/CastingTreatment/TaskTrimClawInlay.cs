using QFramework;
using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskTrimClawInlay : TaskBase
	{
		bool isSuccess1 = false;
		bool isSuccess2 = false;

		public override void InitState()
		{
			modelName = "TrimClawInlay";
			base.InitState();
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.tips = "再用钳子将爪镶爪子长于钻石平面的部分剪去，并打磨光亮。";
			step1.objList.Add(GetObj("NeedleNosePlier1"));
			step1.objList.Add(GetObj("NeedleNosePlier2"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;

			Step step2 = new Step();
			step2.objList.Add(GetObj("Turntable2"));
			step2.Prepare += Prepare2;
			step2.OnClickObj += ClickObj2;
			step2.CheckState += CheckState2;
		}

		#region Step1
		void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			GetObj("NeedleNosePlier2").GetComponent<ObjClickEvent>().SelfDestroy();
			ActionKit.Sequence()
				.Callback(() => { AnimStart("NeedleNosePlier", "NeedleNosePlier_Cut_Ring_Polish"); })
				.Delay(6, () =>
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

		void Prepare2()
		{
			RoamCamera.Instance.IsEnable = false;
			RoamCamera.Instance.MoveTo(new Vector3(1, 0.8f, -1.3f));
		}

		void ClickObj2()
		{
			ActionKit.Sequence()
				.Callback(() => { AnimStart("Ring_Polish", "Ring_Polish_Approach_Turntable2"); })
				.Delay(3.1f, () =>{RoamCamera.Instance.MoveToOrigin();})
				.Delay(2, () => { isSuccess2 = true; })
				.Start(this);
		}

		StepState CheckState2()
		{
			return CheckState(isSuccess2);
		}
		#endregion
	}
}