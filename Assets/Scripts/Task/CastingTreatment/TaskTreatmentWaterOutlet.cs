using LXQJZ.UI;
using System;
using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskTreatmentWaterOutlet : TaskBase
	{
		bool isSuccess1 = false;
		bool isSuccess2 = false;
		DateTime startTime;

		protected override void OnDisable()
		{
			isSuccess1 = false;
			isSuccess2 = false;
			base.OnDisable();
		}

		public override void BeforeInitState()
		{
			startTime = DateTime.Now;
			modelName = "TreatmentWaterOutlet";
		}
		
		public override void AfterInitState()
		{
			GetObj("Ring_Pre_Detail").SetActive(false);
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.tips = "用金工锯将银戒指胚上多余的部分锯除，并将戒指的粗糙面修锉平整。";
			step1.objList.Add(GetObj("WoodenHandleSaw"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;

			Step step2 = new Step();
			step2.objList.Add(GetObj("HalfMoonFile"));
			step2.OnClickObj += ClickObj2;
			step2.CheckState += CheckState2;
		}

		#region Step1
		void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working1());
		}

		IEnumerator working1()
		{
			AnimStart("Ring_Rough_Silver", "Ring_Rough_Silver_SO4Cup_Up");
			yield return new WaitForSeconds(2);
			GetObj("DeaerationMixer1").gameObject.SetActive(true);
			GetObj("PumpTube").gameObject.SetActive(true);
			AnimStart("WoodenHandleSaw", "WoodenHandleSaw_Cut_Ring_Rough_Silver");
			yield return new WaitForSeconds(6);
			GetObj("Ring_Rough_Silver2").SetActive(false);
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
			AnimStart("HalfMoonFile", "HalfMoonFile_Grind_Ring_Rough_Silver1");
			yield return new WaitForSeconds(14);
			GetObj("Ring_Rough_Silver1").SetActive(false);
			GetObj("Ring_Pre_Detail").SetActive(true);
			ModuleReportData report = new ModuleReportData()
			{
				title = "铸件处理_处理水口",
				score = 2,
				startTime = this.startTime,
				endTime = DateTime.Now
			};
			LabReportPanel.Instance.CreateModuleReport(report);
			isSuccess2 = true;
		}

		StepState CheckState2()
		{
			return CheckState(isSuccess2);
		}
		#endregion
	}
}