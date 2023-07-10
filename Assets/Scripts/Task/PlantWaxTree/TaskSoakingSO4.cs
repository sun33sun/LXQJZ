using LXQJZ.UI;
using System;
using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskSoakingSO4 : TaskBase
	{
		bool isSuccess1 = false;
		DateTime startTime;

		protected override void OnDisable()
		{
			isSuccess1 = false;
			base.OnDisable();
		}

		public override void BeforeInitState()
		{
			modelName = "SoakingSO4";
		}

		public override void RegisterSteps()
		{
			startTime = DateTime.Now;
			Step step1 = new Step();
			step1.tips = "请清除铸造过程中产生的杂质";
			step1.objList.Add(GetObj("Ring_Rough_Silver1"));
			step1.objList.Add(GetObj("Ring_Rough_Silver2"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;
		}

		#region Step1
		void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working1());
		}

		IEnumerator working1()
		{
			GetObj("Ring_Rough_Silver2").GetComponent<ObjClickEvent>().SelfDestroy(false);
			AnimStart("Ring_Rough_Silver", "Ring_Rough_Silver_Enter_SO4Cup");
			GetObj("DeaerationMixer1").gameObject.SetActive(false);
			GetObj("PumpTube").gameObject.SetActive(false);
			yield return new WaitForSeconds(6);
			TaskManager.Instance.ShowExam(ProjectSettings.PAPER_SoakingSO4, OnConfirmExam1);
		}

		void OnConfirmExam1(int addScore)
		{
			ModuleReportData report = new ModuleReportData()
			{
				title = "失蜡铸造_泡酸",
				score = 3 + addScore,
				startTime = this.startTime,
				endTime = DateTime.Now
			};
			LabReportPanel.Instance.CreateModuleReport(report);
			isSuccess1 = true;
		}

		StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}
		#endregion
	}
}