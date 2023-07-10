using LXQJZ.Exam;
using LXQJZ.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.Task
{
	public class TaskPouringGypsum : TaskBase
	{
		bool isSuccess1 = false;
		bool isSuccess2 = false;
		bool isSuccess3 = false;
		DateTime startTime;

		protected override void OnDisable()
		{
			isSuccess1 = false;
			isSuccess2 = false;
			isSuccess3 = false;
			base.OnDisable();
		}

		public override void RegisterSteps()
		{
			startTime = DateTime.Now;
			Step step1 = new Step();
			step1.tips = "将石膏，倒入刚刚的铁皮圆桶中";
			step1.objList.Add(GetObj("WaterCup"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;

			Step step2 = new Step();
			step2.objList.Add(GetObj("WaterCup"));
			step2.OnClickObj += ClickObj2;
			step2.CheckState += CheckState2;

			Step step3 = new Step();
			step3.tips = "必须要把石膏浆中多余的气泡震出才可以进行铸造，否则会导致蜡模铸造出来有气泡凹陷";
			step3.objList.Add(GetObj("DeaerationMixer2"));
			step3.OnClickObj += ClickObj3;
			step3.CheckState += CheckState3;
		}

		#region Step1
		void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working1());
		}

		IEnumerator working1()
		{
			WaitForSeconds wait6 = new WaitForSeconds(6);
			WaitForSeconds wait2 = new WaitForSeconds(2);

			AnimStart("Gypsum", "Gypsum_Enter_WaterCup");
			yield return wait2;
			//搅拌水杯
			AnimStart("Stirrer", "Stirrer_Stir_WaterCup");
			yield return wait6;
			//水杯变色
			MeshRenderer renderer = GetObj("WaterCupEffect").GetComponent<MeshRenderer>();
			renderer.material.color = new Color(1,1,1,0.7f);

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
			WaitForSeconds wait4 = new WaitForSeconds(4);
			WaitForSeconds wait3 = new WaitForSeconds(3);

			TaskManager.Instance.ShowExam(ProjectSettings.PAPER_PouringGypsum, OnConfirmExam2);
			yield return new WaitUntil(TaskManager.Instance.CheckExam);
			//蜡树进入吸泡机
			GameObject DeaerationMixer1 = GetObj("DeaerationMixer1");
			GameObject PumpTube = GetObj("PumpTube");
			DeaerationMixer1.SetActive(false);
			PumpTube.SetActive(false);
			AnimStart("WaxTree", "WaxTree_Enter_DeaerationMixer");
			yield return wait4;
			//倒进蜡树里
			AnimStart("WaterCup", "WaterCup_Dump_WaxTree");
			yield return wait4;
			DeaerationMixer1.SetActive(true);
			PumpTube.SetActive(true);
			//水杯变色
			MeshRenderer renderer = GetObj("WaterCupEffect").GetComponent<MeshRenderer>();
			renderer.enabled = false;
			GetObj("GypsumEffect").GetComponent<MeshRenderer>().enabled = true;
			GetObj("GypsumEffect").GetComponent<CapsuleCollider>().enabled = true;
			isSuccess2 = true;
		}

		void OnConfirmExam2(int addScore)
		{
			ModuleReportData report = new ModuleReportData()
			{
				title = "失蜡铸造_浇筑石膏",
				score = 3 + addScore,
				startTime = this.startTime,
				endTime = DateTime.Now
			};
			LabReportPanel.Instance.CreateModuleReport(report);
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
			StartCoroutine(working3());
		}

		IEnumerator working3()
		{
			WaitForSeconds wait4 = new WaitForSeconds(4);
			WaitForSeconds wait01 = new WaitForSeconds(0.1f);

			//加载进度条
			GetUI<Image>("DeaerationMixer_Slider").gameObject.SetActive(true);
			Image imgFill = GetUI<Image>("DeaerationMixer_Fill");
			imgFill.gameObject.SetActive(true);
			float process = 0;
			while (process < 1)
			{
				process += 0.05f;
				imgFill.fillAmount = process;
				yield return wait01;
			}
			GetUI<Image>("DeaerationMixer_Slider").gameObject.SetActive(false);
			//蜡树出吸泡机
			GameObject DeaerationMixer1 = GetObj("DeaerationMixer1");
			GameObject PumpTube = GetObj("PumpTube");
			DeaerationMixer1.SetActive(false);
			PumpTube.SetActive(false);
			AnimStart("WaxTree", "WaxTree_From_DeaerationMixer_To_Origin");
			yield return wait4;
			DeaerationMixer1.SetActive(true);
			PumpTube.SetActive(true);

			isSuccess3 = true;
		}

		StepState CheckState3()
		{
			return CheckState(isSuccess3);
		}
		#endregion
	}
}


