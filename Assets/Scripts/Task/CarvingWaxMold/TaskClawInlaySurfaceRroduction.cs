using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace LXQJZ.Task
{
	public class TaskClawInlaySurfaceRroduction : TaskBase
	{
		bool isSuccess1 = false;
		bool isSuccess2 = false;
		bool isSuccess3 = false;
		bool isSuccess4 = false;
		bool isSuccess5 = false;
		bool isSuccess6 = false;
		bool isSuccess7 = false;

		protected override void OnDisable()
		{
			isSuccess1 = false;
			isSuccess2 = false;
			isSuccess3 = false;
			isSuccess4 = false;
			isSuccess5 = false;
			isSuccess6 = false;
			isSuccess7 = false;
			base.OnDisable();
		}

		public override void InitState()
		{
			modelName = "ClawInlaySurfaceRroduction";
			base.InitState();
		}

		public override void RegisterSteps()
		{
			GetObj("ClawInlay_Melt").SetActive(false);
			GetObj("RingWax_Gap_Melt").SetActive(false);
			GetObj("RingWax_Gap").SetActive(false);
			GetObj("ClawInlaySurface_Grind").SetActive(false);
			GetObj("ClawInlaySurface_Wireframe").SetActive(false);
			GetObj("ClawInlaySurface_Groove1").SetActive(false);
			GetObj("ClawInlaySurface_Groove2").SetActive(false);
			GetObj("ClawInlayRing1").SetActive(false);
			GetObj("ClawInlayRing2").SetActive(false);
			GetObj("TreatedRing").SetActive(false);
			GetObj("TreatedRing1").SetActive(false);
			GetObj("TreatedRing2").SetActive(false);

			Step step1 = new Step();
			step1.tips = "�ڽ�Ȧ������ĥ��һ������צ�������ȱ�ڣ�����Ȧ��צ�������۽���һ�𣬲����ӿڶ��������ĥƽ��";
			step1.objList.Add(GetObj("Sander"));
			step1.Prepare += Prepare1;
			step1.OnClickObj += ClickObj;
			step1.CheckState += CheckState1;

			Step step2 = new Step();
			step2.objList.Add(GetObj("WeldingWaxMachine_Pen"));
			step2.OnClickObj += ClickObj2;
			step2.CheckState += CheckState2;

			Step step3 = new Step();
			step3.objList.Add(GetObj("Drill2"));
			step3.OnClickObj += ClickObj3;
			step3.CheckState += CheckState3;

			Step step4 = new Step();
			step4.objList.Add(GetObj("RingStick"));
			step4.OnClickObj += ClickObj4;
			step4.CheckState += CheckState4;

			Step step5 = new Step();
			step5.objList.Add(GetObj("Sander"));
			step5.OnClickObj += ClickObj5;
			step5.CheckState += CheckState5;
			step5.tips = "�ڽ�����צ��צ�ӵ�λ�ã������δ�ĥ�뽫צ�������м���ಿ�ִ�ĥ��������ĥ��צ��";

			Step step6 = new Step();
			step6.objList.Add(GetObj("Compass1"));
			step6.objList.Add(GetObj("Compass2"));
			step6.objList.Add(GetObj("Compass3"));
			step6.objList.Add(GetObj("Compass4"));
			step6.OnClickObj += ClickObj6;
			step6.CheckState += CheckState6;

			Step step7 = new Step();
			step7.objList.Add(GetObj("Sander"));
			step7.OnClickObj += ClickObj7;
			step7.CheckState += CheckState7;
		}

		#region Step1
		void Prepare1()
		{
			StartCoroutine(workingPrepare1());
		}

		IEnumerator workingPrepare1()
		{
			AnimStart("Drill1", "Drill1_FixedTo_Sander");
			yield return new WaitForSeconds(1);
			GetObj("Drill1").transform.SetParent(GetObj("Sander").transform);
			AnimManager.Play(GetObj("Drill1"), "Drill1_Forward_Sander");

		}

		void ClickObj()
		{
			RoamCamera.Instance.IsEnable = false;
			AnimCallBack("Sander", AnimEnd1, "Sander_GrindGap_RingWaxBlock_Smooth");
		}

		void AnimEnd1()
		{
			AnimCallBack("Drill1", AnimEnd1_1, "Drill1_Rotate_Loop", 5);
		}
		void AnimEnd1_1()
		{
			GetObj("RingWaxBlock_MiddleLine").SetActive(false);
			GetObj("RingWax_Gap").SetActive(true);
			AnimCallBack("Sander", AnimEnd1_2, "Sander_From_RingWaxBlock_Smooth_To_Origin", 2);
		}
		void AnimEnd1_2()
		{
			GetObj("Drill1").transform.SetParent(GetObj("Tool").transform);
			AnimCallBack("Drill1", AnimEnd1_3, "Drill1_From_OriginSander_To_Origin", 1);
		}

		void AnimEnd1_3()
		{
			isSuccess1 = true;
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
			StartCoroutine(working2());
		}

		IEnumerator working2()
		{
			AnimStart("ClawInlayWaxBlock", "ClawInlayWaxBlock_Approach_RingWax_Gap");
			yield return new WaitForSeconds(3);
			GetObj("ClawInlayWaxBlock").SetActive(false);
			GetObj("ClawInlay_Melt").SetActive(true);

			AnimStart("WeldingWaxMachine", "WeldingWaxMachine_Approach_RingWax_Gap");
			yield return new WaitForSeconds(5);
			AnimStart("WeldingWaxMachine", "WeldingWaxMachine_From_RingWax_Gap_To_Origin");
			yield return new WaitForSeconds(1);
			isSuccess2 = true;
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
			ActionKit.Sequence().Callback(
				() =>
				{
					AnimStart("Drill2", "Drill2_FixedTo_Sander");
				})
				.Delay(1,
				() =>
				{
					GetObj("Drill2").transform.SetParent(GetObj("Sander").transform);
					AnimStart("Drill2", "Drill2_Forward_Sander");
					AnimStart("Sander", "Sander_Smooth_RingWaxBlock_Smooth");
				}).Delay(3,
				() =>
				{
					GetObj("Drill2").transform.SetParent(GetObj("Tool").transform);
					AnimStart("Drill2", "Drill2_From_OriginSander_To_Origin");
				}).Delay(1,
				() =>
				{
					isSuccess3 = true;
				})
				.Start(this);
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
			AnimCallBack("RingStick", AnimEnd4, "RingStick_Check_ClawInlaySurface", 4);
		}

		void AnimEnd4()
		{

			isSuccess4 = true;
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
			StartCoroutine(working5());
		}

		IEnumerator working5()
		{
			WaitForSeconds wait1 = new WaitForSeconds(1.1f);
			WaitForSeconds wait3 = new WaitForSeconds(3.1f);

			AnimStart("Drill1", "Drill1_FixedTo_Sander");
			yield return wait1;
			GetObj("Drill1").transform.SetParent(GetObj("Sander").transform);
			AnimStart("Drill1", "Drill1_Forward_Sander");
			yield return wait1;
			AnimStart("Sander", "Sander_GrindEdge_ClawInlay");
			yield return wait3;
			GetObj("RingWax_Gap").SetActive(false);
			GetObj("ClawInlay_Melt").SetActive(false);
			GetObj("ClawInlaySurface_Grind").SetActive(true);
			GetObj("Drill1").transform.SetParent(GetObj("Tool").transform);
			AnimStart("Drill1", "Drill1_From_OriginSander_To_Origin");
			yield return wait1;
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
			GetObj("Compass2").GetComponent<ObjClickEvent>().SelfDestroy(false);
			GetObj("Compass3").GetComponent<ObjClickEvent>().SelfDestroy(false);
			GetObj("Compass4").GetComponent<ObjClickEvent>().SelfDestroy(false);
			AnimCallBack("Compass", AnimEnd6, "Compass_Wireframe_ClawInlaySurface_Grind", 4);
		}
		void AnimEnd6()
		{
			GetObj("ClawInlaySurface_Grind").SetActive(false);
			GetObj("ClawInlaySurface_Wireframe").SetActive(true);
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
			StartCoroutine(working7());
		}

		IEnumerator working7()
		{
			WaitForSeconds wait1 = new WaitForSeconds(1.1f);
			WaitForSeconds wait6 = new WaitForSeconds(6.1f);
			WaitForSeconds wait3 = new WaitForSeconds(3);

			//һ�ΰ�װ��ͷ��
			AnimStart("Drill_Big", "Drill_Big_FixedTo_Sander");
			yield return wait1;
			GetObj("Drill_Big").transform.SetParent(GetObj("Sander").transform);
			AnimStart("Drill_Big", "Drill_Big_Forward_Sander");
			yield return new WaitForEndOfFrame();
			//һ�δ�ĥ
			AnimStart("Sander", "Sander_Grind_ClawInlaySurface_Wireframe");
			yield return wait6;
			//��ʾ��ĥ��ģ��
			GetObj("ClawInlaySurface_Wireframe").SetActive(false);
			GetObj("ClawInlaySurface_Groove1").SetActive(true);
			//ж�ش�ͷ��
			GetObj("Drill_Big").transform.SetParent(GetObj("Tool").transform);
			AnimStart("Drill_Big", "Drill_Big_From_OriginSander_To_Origin");
			yield return wait1;
			//��װСͷ��
			AnimStart("Drill_Small", "Drill_Small_FixedTo_Sander");
			yield return wait1;
			GetObj("Drill_Small").transform.SetParent(GetObj("Sander").transform);
			AnimStart("Drill_Small", "Drill_Small_Forward_Sander");
			yield return new WaitForEndOfFrame();
			//���δ�ĥ
			AnimStart("Sander", "Sander_Grind_ClawInlaySurface_Wireframe");
			yield return wait6;
			//��ʾ���δ�ĥ��ģ��
			GetObj("ClawInlaySurface_Groove1").SetActive(false);
			GetObj("ClawInlaySurface_Groove2").SetActive(true);
			//ж��Сͷ��
			GetObj("Drill_Small").transform.SetParent(GetObj("Tool").transform);
			AnimStart("Drill_Small", "Drill_Small_From_OriginSander_To_Origin");
			yield return wait1;
			//�ٴΰ�װ��ͷ��
			AnimStart("Drill_Big", "Drill_Big_FixedTo_Sander");
			yield return wait1;
			GetObj("Drill_Big").transform.SetParent(GetObj("Sander").transform);
			AnimStart("Drill_Big", "Drill_Big_Forward_Sander");
			yield return new WaitForEndOfFrame();
			//���δ�ĥ
			AnimStart("Sander", "Sander_Grind_ClawInlaySurface_Wireframe");
			yield return wait6;
			//����ж�ش�ͷ��
			GetObj("Drill_Big").transform.SetParent(GetObj("Tool").transform);
			AnimStart("Drill_Big", "Drill_Big_From_OriginSander_To_Origin");
			yield return wait1;
			//���ΰ�װСͷ��
			AnimStart("Drill_Small", "Drill_Small_FixedTo_Sander");
			yield return wait1;
			GetObj("Drill_Small").transform.SetParent(GetObj("Sander").transform);
			AnimStart("Drill_Small", "Drill_Small_Forward_Sander");
			yield return new WaitForEndOfFrame();
			//��ĥ��צ����״
			AnimStart("Sander", "Sander_Generate_ClawInlayRing1");
			//��ʾģ��
			yield return wait3;
			GetObj("ClawInlaySurface_Groove1").SetActive(false);
			GetObj("ClawInlayRing1").SetActive(true);
			yield return wait3;
			GetObj("ClawInlayRing1").SetActive(false);
			GetObj("ClawInlayRing2").SetActive(true);
			yield return wait3;
			GetObj("ClawInlayRing2").SetActive(false);
			GetObj("TreatedRing").SetActive(true);
			yield return wait3;
			GetObj("TreatedRing").SetActive(false);
			GetObj("TreatedRing1").SetActive(true);
			yield return wait3;
			GetObj("TreatedRing1").SetActive(false);
			GetObj("TreatedRing2").SetActive(true);
			//����ж��Сͷ��
			GetObj("Drill_Small").transform.SetParent(GetObj("Tool").transform);
			AnimStart("Drill_Small", "Drill_Small_From_OriginSander_To_Origin");
			yield return wait1;
			isSuccess7 = true;
		}

		StepState CheckState7()
		{
			return CheckState(isSuccess7);
		}
		#endregion
	}
}
