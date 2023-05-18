using QFramework;
using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskPolishRing : TaskBase
	{
		bool isSuccess1 = false;
		bool isSuccess2 = false;
		bool isSuccess3 = false;
		bool isSuccess4 = false;
		bool isSuccess5 = false;


		protected override void OnDisable()
		{
			isSuccess1 = false;
			isSuccess2 = false;
			isSuccess3 = false;
			isSuccess4 = false;
			isSuccess5 = false;
			base.OnDisable();
		}
		public override void InitState()
		{
			modelName = "PolishRing";
			base.InitState();
			GetObj("Ring_Polish").SetActive(false);
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.tips = "将戒指逐渐打磨光滑。";
			step1.objList.Add(GetObj("Sander"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;

			Step step2 = new Step();
			step2.objList.Add(GetObj("Ring_Detail"));
			step2.OnClickObj += ClickObj2;
			step2.CheckState += CheckState2;

			Step step3 = new Step();
			step3.objList.Add(GetObj("MagnetNeedle1"));
			step3.objList.Add(GetObj("MagnetNeedle2"));
			step3.objList.Add(GetObj("MagnetNeedle3"));
			step3.objList.Add(GetObj("MagnetNeedle4"));
			step3.objList.Add(GetObj("MagnetNeedle5"));
			step3.OnClickObj += ClickObj3;
			step3.CheckState += CheckState3;

			Step step4 = new Step();
			step4.objList.Add(GetObj("WashingPowderCup"));
			step4.OnClickObj += ClickObj4;
			step4.CheckState += CheckState4;


			Step step5 = new Step();
			step5.objList.Add(GetObj("Ring_Detail"));
			step5.OnClickObj += ClickObj5;
			step5.CheckState += CheckState5;
		}

		#region Step1
		void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			ActionKit.Sequence()
				//240
				.Callback(() => { AnimStart("SanderStick240", "SanderStick240_FixedTo_Sander"); })
				.Delay(1, () => {
					GetObj("SanderStick240").transform.SetParent(GetObj("Sander").transform);
					AnimStart("SanderStick240", "SanderStick240_Forward_Sander");
					AnimStart("Sander", "Sander_Polish_Ring_Detail");
				})
				.Delay(6, () => 
				{
					GetObj("SanderStick240").transform.SetParent(GetObj("Tool").transform);
					AnimStart("SanderStick240", "SanderStick240_From_Sander_To_Origin");
				})
				//800
				.Delay(1, () => { AnimStart("SanderStick800", "SanderStick800_FixedTo_Sander"); })
				.Delay(1, () => {
					GetObj("SanderStick800").transform.SetParent(GetObj("Sander").transform);
					AnimStart("SanderStick800", "SanderStick800_Forward_Sander");
					AnimStart("Sander", "Sander_Polish_Ring_Detail");
				})
				.Delay(6, () => 
				{
					GetObj("SanderStick800").transform.SetParent(GetObj("Tool").transform);
					AnimStart("SanderStick800", "SanderStick800_From_Sander_To_Origin");
				})
				//2000
				.Delay(1, () => { AnimStart("SanderStick2000", "SanderStick2000_FixedTo_Sander"); })
				.Delay(1, () => 
				{
					GetObj("SanderStick2000").transform.SetParent(GetObj("Sander").transform);
					AnimStart("SanderStick2000", "SanderStick2000_Forward_Sander");
					AnimStart("Sander", "Sander_Polish_Ring_Detail");
				})
				.Delay(6, () => 
				{
					GetObj("SanderStick2000").transform.SetParent(GetObj("Tool").transform);
					AnimStart("SanderStick2000", "SanderStick2000_From_Sander_To_Origin");
				})
				//7000
				.Delay(1, () => { AnimStart("SanderStick7000", "SanderStick7000_FixedTo_Sander"); })
				.Delay(1, () => 
				{
					GetObj("SanderStick7000").transform.SetParent(GetObj("Sander").transform);
					AnimStart("SanderStick7000", "SanderStick7000_Forward_Sander");
					AnimStart("Sander", "Sander_Polish_Ring_Detail");
				})
				.Delay(6, () => 
				{
					GetObj("SanderStick7000").transform.SetParent(GetObj("Tool").transform);
					AnimStart("SanderStick7000", "SanderStick7000_From_Sander_To_Origin"); 
				})
				//10000
				.Delay(1, () => { AnimStart("SanderStick10000", "SanderStick10000_FixedTo_Sander"); })
				.Delay(1, () => 
				{
					GetObj("SanderStick10000").transform.SetParent(GetObj("Sander").transform);
					AnimStart("SanderStick10000", "SanderStick10000_Forward_Sander");
					AnimStart("Sander", "Sander_Polish_Ring_Detail"); 
				})
				.Delay(6, () => 
				{
					GetObj("SanderStick10000").transform.SetParent(GetObj("Tool").transform);
					AnimStart("SanderStick10000", "SanderStick10000_From_Sander_To_Origin");
					isSuccess1 = true;
				})
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
			ActionKit.Sequence().Callback(()=> { AnimStart("Ring_Detail", "Ring_Detail_Enter_WashCup"); })
			.Delay(1, () =>
			{
				isSuccess2 = true;
			}).Start(this);
		}

		StepState CheckState2()
		{
			return CheckState(isSuccess2);
		}
		#endregion

		#region Step3
		void ClickObj3()
		{
			RoamCamera.Instance.IsEnable = false;
			GetObj("MagnetNeedle2").GetComponent<ObjClickEvent>().SelfDestroy();
			GetObj("MagnetNeedle3").GetComponent<ObjClickEvent>().SelfDestroy();
			GetObj("MagnetNeedle4").GetComponent<ObjClickEvent>().SelfDestroy();
			GetObj("MagnetNeedle5").GetComponent<ObjClickEvent>().SelfDestroy();
			ActionKit.Sequence().Callback(() => { AnimStart("MagnetNeedle", "MagnetNeedle_Enter_WashCup"); })
			.Delay(1, () =>
			{
				isSuccess3 = true;
			}).Start(this);
		}

		StepState CheckState3()
		{
			return CheckState(isSuccess3);
		}
		#endregion

		#region Step4
		void ClickObj4()
		{
			RoamCamera.Instance.IsEnable = false;
			ActionKit.Sequence().Callback(() => { AnimStart("WashingPowderCup", "WashingPowderCup_Pour_WashCup"); })
			.Delay(3, () =>
			{
				AnimStart("Ring_Detail", "Ring_Detail_From_WashCup_To_Up");
			}).Delay(1, () => { isSuccess4 = true; })
			.Start(this);
		}

		StepState CheckState4()
		{
			return CheckState(isSuccess4);
		}
		#endregion

		#region Step5
		void ClickObj5()
		{
			RoamCamera.Instance.IsEnable = false;
			ActionKit.Sequence()
			.Callback(() => { AnimStart("Drill1", "Drill1_FixedTo_Sander"); })
			.Delay(1,() => 
			{
				GetObj("Drill1").transform.SetParent(GetObj("Sander").transform);
				AnimStart("Drill1", "Drill1_Forward_Sander");
				AnimStart("Sander", "Sander_Polish_Ring_Detail");
			})
			.Delay(6, () =>
			{
				GetObj("Drill1").transform.SetParent(GetObj("Tool").transform);
				AnimStart("Drill1", "Drill1_From_OriginSander_To_Origin");
			}).Delay(1,()=> { isSuccess5 = true; })
			.Start(this);
		}

		StepState CheckState5()
		{
			return CheckState(isSuccess5);
		}
		#endregion
	}
}