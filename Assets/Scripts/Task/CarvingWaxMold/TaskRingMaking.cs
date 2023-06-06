using LXQJZ.Exam;
using LXQJZ.UI;
using LXQJZ.UI.Effect;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.Task
{
	public class TaskRingMaking : TaskBase
	{
		[SerializeField] List<GameObject> objHide;
		[SerializeField] List<Sprite> sketchs;

		bool isSuccess1 = false;
		bool isSuccess2 = false;
		bool isSuccess3 = false;
		bool isSuccess4 = false;
		bool isSuccess5 = false;

		public override void BeforeInitState()
		{
			modelName = "RingMaking";
		}

		protected override void OnDisable()
		{
			isSuccess1 = false;
			isSuccess2 = false;
			isSuccess3 = false;
			isSuccess4 = false;
			isSuccess5 = false;
			base.OnDisable();
		}

		void HideSomething()
		{
			ParticleManager.Stop(GetObj("FireGun_Fire"));
			GetObj("RingWaxTubeMarkLine").SetActive(false);
			GetObj("RingWaxBlock_Smooth").SetActive(false);
			GetUI<Image>("DeaerationMixer_Slider").gameObject.SetActive(false);

			GetObj("SilverEffect").SetActive(false);
		}

		public override void AfterInitState()
		{
			HideSomething();
		}

		bool isFirst = true;
		public override void RegisterSteps()
		{
			if (!isFirst)
			{
				HideSomething();
			}

			Step step1 = new Step();
			step1.objList.Add(GetObj("Caliper_1"));
			step1.objList.Add(GetObj("Caliper_2"));
			step1.Prepare += Prepare1;
			step1.CheckState += CheckState1;
			step1.OnClickObj += ClickObj1;
			step1.tips = "请标示戒圈蜡片，并锯下蜡块，切割戒圈戒圈蜡片";

			Step step2 = new Step();
			step2.Prepare += Prepare2;
			step2.CheckState += CheckState2;

			Step step3 = new Step();
			step3.objList.Add(GetObj("WoodenHandleSaw"));
			step3.OnClickObj += ClickObj3;
			step3.CheckState += CheckState3;

			Step step4 = new Step();
			step4.objList.Add(GetObj("LargeFile"));
			step4.OnClickObj += ClickObj4;
			step4.CheckState += CheckState4;
			step4.tips = "请用锉刀将戒指的外轮廓修整成需要的形状，并打磨平整，检查蜡块戒圈厚度";

			Step step5 = new Step();
			step5.objList.Add(GetObj("Caliper_1"));
			step5.objList.Add(GetObj("Caliper_2"));
			step5.OnClickObj += ClickObj5;
			step5.CheckState += CheckState5;
		}

		#region Step1
		private void Prepare1()
		{
			ActionKit.Delay(0.5f, () => { OnlineLabPanel.Instance.ShowSketch(sketchs); }).Start(this);
		}

		private void ClickObj1()
		{
			GetObj("Caliper_2").GetComponent<ObjClickEvent>().SelfDestroy(false);
			TaskManager.Instance.ShowExam(ProjectSettings.PAPER_RingMaking, OnConfirmExam1);
		}

		void OnConfirmExam1(int addScore)
		{
			TaskManager.Instance.score += addScore;
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working1());
		}

		IEnumerator working1()
		{
			AnimStart("RingWaxTube", "RingWaxTube_Up");
			yield return new WaitForSeconds(3);
			AnimStart("DigitalCaliper", "DigitalCaliper_Check_RingWaxBlock");
			yield return new WaitForSeconds(1);
			isSuccess1 = true;
		}

		private StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}
		#endregion

		#region Step2
		private void Prepare2()
		{
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working2());
		}

		IEnumerator working2()
		{
			AnimStart("RingWaxTube", "RingWaxTube_Rotate");
			yield return new WaitForSeconds(3);
			AnimStart("RingWaxTube", "RingWaxTube_Forward");
			GetObj("RingWaxTubeMarkLine").SetActive(true);
			AnimStart("DigitalCaliper", "DigitalCaliper_Back_Origin");
			yield return new WaitForSeconds(2);
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
			AnimCallBack("WoodenHandleSaw", AnimEnd3, "WoodenHandleSaw_Approach_RingWaxTube");
		}

		private void AnimEnd3()
		{
			AnimCallBack("RingWaxTube", AnimEnd3_1, "RingWaxTube_Rotate");
		}

		private void AnimEnd3_1()
		{
			GetObj("RingWaxBlock_MiddleLine").transform.SetParent(GetObj(modelInstance.name).transform);
			GetObj("RingWaxTube").SetActive(false);
			AnimManager.Play(GetObj("RingWaxBlock_MiddleLine"), "RingWaxBlockMiddleLine_Forward_TaskModel");
			AnimCallBack("WoodenHandleSaw", AnimEnd3_2, "WoodenHandleSaw_Cut_RingWaxBlock");
		}

		void AnimEnd3_2()
		{
			GetObj("RingWaxBlock").SetActive(false);
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
			AnimCallBack("LargeFile", AnimEnd4, "LargeFile_Cut_RingWaxBlock");
		}
		void AnimEnd4()
		{
			AnimCallBack("RingWaxBlock_MiddleLine", AnimEnd4_1, "RingWaxBlockMiddleLine_Rotate_TaskModel");
		}
		private void AnimEnd4_1()
		{
			GetObj("RingWaxBlock_Smooth").SetActive(true);
			GameObject RingWaxBlock_MiddleLine = GetObj("RingWaxBlock_MiddleLine");
			RingWaxBlock_MiddleLine.GetComponent<BoxCollider>().enabled = false;
			RingWaxBlock_MiddleLine.GetComponent<MeshRenderer>().enabled = false;
			AnimCallBack("LargeFile", AnimEnd4_2, "LargeFile_Back_Origin");
		}
		void AnimEnd4_2()
		{
			isSuccess4 = true;
		}
		private StepState CheckState4()
		{
			return CheckState(isSuccess4);
		}
		#endregion

		#region Step5
		private void ClickObj5()
		{
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working5());
		}

		IEnumerator working5()
		{
			GetObj("Caliper_2").GetComponent<ObjClickEvent>().SelfDestroy();
			AnimStart("DigitalCaliper", "DigitalCaliper_Check_RingWaxBlock");
			yield return new WaitForSeconds(2);
			AnimStart("DigitalCaliper", "DigitalCaliper_Back_Origin");
			isSuccess5 = true;
		}
		private StepState CheckState5()
		{
			return CheckState(isSuccess5);
		}
		#endregion
	}
}
