using LXQJZ.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskClawInlayProduction : TaskBase
	{
		[SerializeField] List<Sprite> sketchs;

		bool isSuccess1 = false;
		bool isSuccess2 = false;
		bool isSuccess3 = false;
		bool isSuccess4 = false;
		bool isSuccess5 = false;
		bool isSuccess6 = false;
		bool isSuccess7 = false;

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.Prepare += Prepare1;
			step1.objList.Add(GetObj("WoodenHandleSaw"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;
			step1.tips = "请在蜡片上用圆规机剪，根据钻石尺寸标出所需切割的圆形，并打磨爪镶部分蜡块";

			Step step2 = new Step();
			step2.objList.Add(GetObj("Drill1"));
			step2.OnClickObj += ClickObj2;
			step2.CheckState += CheckState2;

			Step step3 = new Step();
			step3.objList.Add(GetObj("Sander"));
			step3.OnClickObj += ClickObj3;
			step3.CheckState += CheckState3; ;

			Step step4 = new Step();
			step4.objList.Add(GetObj("BenchVice1"));
			step4.objList.Add(GetObj("BenchVice2"));
			step4.OnClickObj += ClickObj4;
			step4.CheckState += CheckState4;

			Step step5 = new Step();
			step5.objList.Add(GetObj("Scalpel"));
			step5.OnClickObj += ClickObj5;
			step5.CheckState += CheckState5; ;

			Step step6 = new Step();
			step6.objList.Add(GetObj("Caliper_1"));
			step6.objList.Add(GetObj("Caliper_2"));
			step6.OnClickObj += ClickObj6;
			step6.CheckState += CheckState6;
			step6.tips = "请检查爪镶蜡块的尺寸，并取出钻针";

			Step step7 = new Step();
			step7.objList.Add(GetObj("Drill1"));
			step7.OnClickObj = ClickObj7;
			step7.CheckState += CheckState7;
		}

		public override void InitState()
		{
			modelName = "ClawInlayProduction";
			base.InitState();
		}

		#region Step1
		private void Prepare1()
		{
			OnlineLabPanel.Instance.ShowSketch(sketchs);
		}
		private void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			AnimCallBack("ClawInlayWaxBlock", AnimEnd1, "ClawInlayWaxBlock_Up");
		}
		private void AnimEnd1()
		{
			AnimCallBack("WoodenHandleSaw", AnimEnd1_1, "WoodenHandleSaw_Cut_ClawInlayWaxBlock", 6);
		}

		void AnimEnd1_1()
		{
			//显示被切割后的模型
			GetObj("ClawInlayWaxBlock").GetComponent<MeshRenderer>().enabled = false;
			GetObj("ClawInlayWaxBlock").GetComponent<BoxCollider>().enabled = false;
			GetObj("ClawInlayWaxBlock_Cut").GetComponent<BoxCollider>().enabled = true;
			isSuccess1 = true;
		}

		private StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}
		#endregion

		#region Step2
		private void ClickObj2()
		{
			RoamCamera.Instance.IsEnable = false;
			AnimCallBack("ClawInlayWaxBlock", null, "ClawInlayWaxBlock_Fixed_Drill");
			AnimCallBack("Drill1", AnimEnd2, "Drill1_Fixed_ClawInlayBlock");
		}

		private void AnimEnd2()
		{
			ParticleCallBack("FireGun_Fire", ParticleEnd2_1, 5);
			GetObj("ClawInlayWaxBlock").transform.SetParent(GetObj("Drill1").transform);
			AnimStart("ClawInlayWaxBlock", "ClawInlayWaxBlock_Forward_Drill1");
		}
		private void ParticleEnd2_1()
		{
			isSuccess2 = true;
		}
		private StepState CheckState2()
		{
			return CheckState(isSuccess2);
		}
		#endregion

		#region Step3
		private void ClickObj3()
		{
			RoamCamera.Instance.IsEnable = false;
			AnimCallBack("Drill1", AnimEnd3, "Drill1_Approach_Sander");
		}
		private void AnimEnd3()
		{
			GetObj("Drill1").transform.SetParent(GetObj("Sander").transform);
			AnimManager.Play(GetObj("Drill1"), "Drill1_Forward_Sander", 0);
			isSuccess3 = true;
		}
		private StepState CheckState3()
		{
			return CheckState(isSuccess3);
		}
		#endregion

		#region Step4
		private void ClickObj4()
		{
			RoamCamera.Instance.IsEnable = false;

			GetObj("BenchVice2").GetComponent<ObjClickEvent>().SelfDestroy(false);
			AnimManager.Play(GetObj("BenchVice"), "BenchVice_Open", 1);
			AnimCallBack("Sander", AnimEnd4, "Sander_Fixed_BenchVice");
		}
		private void AnimEnd4()
		{
			GetObj("Sander").transform.SetParent(GetObj("BenchVice").transform);
			AnimManager.Play(GetObj("Sander"), "Sander_Forward_BenchVice", 0);
			isSuccess4 = true;
		}
		private StepState CheckState4()
		{
			return CheckState(isSuccess4);
		}
		#endregion

		#region Step5
		void ClickObj5()
		{
			RoamCamera.Instance.IsEnable = false;

			AnimCallBack("Drill1", AnimEnd5, "Drill1_Rotate_Loop", 9);
			AnimCallBack("Scalpel", null, "Scalpel_Approach_ClawInlayWaxBlock_Cut");
		}
		void AnimEnd5()
		{
			AnimCallBack("Scalpel", AnimEnd5_1, "Scalpel_From_ClawInlayWaxBlock_Cut_To_Origin");
		}

		void AnimEnd5_1()
		{
			GameObject ClawInlayWaxBlock_Cut = GetObj("ClawInlayWaxBlock_Cut");
			ClawInlayWaxBlock_Cut.GetComponent<BoxCollider>().enabled = false;
			ClawInlayWaxBlock_Cut.GetComponent<MeshRenderer>().enabled = false;

			GameObject ClawInlayWaxBlock_Sander = GetObj("ClawInlayWaxBlock_Sander");
			ClawInlayWaxBlock_Sander.GetComponent<MeshRenderer>().enabled = true;
			ClawInlayWaxBlock_Sander.GetComponent<BoxCollider>().enabled = true;
			isSuccess5 = true;
		}

		StepState CheckState5()
		{
			return CheckState(isSuccess5);
		}
		#endregion

		#region Step6
		void ClickObj6()
		{
			RoamCamera.Instance.IsEnable = false;

			GetObj("Caliper_2").GetComponent<ObjClickEvent>().SelfDestroy();
			AnimCallBack("DigitalCaliper", AnimEnd6, "DigitalCaliper_Check_ClawInlayWaxBlock_Sander", 5);
		}
		void AnimEnd6()
		{
			isSuccess6 = true;
		}
		StepState CheckState6()
		{
			return CheckState(isSuccess6);
		}
		#endregion

		#region Step7
		void ClickObj7()
		{
			RoamCamera.Instance.IsEnable = false;

			GetObj("Drill1").transform.SetParent(GetObj("Tool").transform);
			AnimCallBack("Drill1", AnimEnd7, "Drill1_Detach_ClawInlayWaxBlock_Sander");

			GetObj("Sander").transform.SetParent(GetObj("Tool").transform);
			AnimCallBack("Sander", null, "Sander_From_BenchVice_To_Origin");
		}

		void AnimEnd7()
		{
			ParticleCallBack("FireGun_Fire", ParticleEnd7, 3);
		}
		void ParticleEnd7()
		{
			GetObj("ClawInlayWaxBlock").transform.SetParent(GetObj(modelInstance.name).transform);
			AnimCallBack("ClawInlayWaxBlock", AnimEnd7_1, "ClawInlayWaxBlock_From_Drill1_To_Origin");

			AnimCallBack("Drill1", null, "Drill1_From_ClawInlayWaxBlock_Sander_To_Origin");
		}
		void AnimEnd7_1()
		{
			isSuccess7 = true;
		}
		StepState CheckState7()
		{
			return CheckState(isSuccess7);
		}
		#endregion
	}
}
