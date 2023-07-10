using LXQJZ.UI;
using System;
using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskMetalDemolding:TaskBase
	{
		bool isSuccess1 = false;
		DateTime startTime;

		protected override void OnDisable()
		{
			isSuccess1 = false;
			base.OnDisable();
			GameObject GypsumEffect = GetObj("GypsumEffect");
			if(GypsumEffect != null)
				GypsumEffect.GetComponent<MeshRenderer>().enabled = true;
		}

		public override void BeforeInitState()
		{
			modelName = "MetalDemolding";
		}

		public override void AfterInitState()
		{
			GetObj("Ring_Rough_Silver").SetActive(false);
		}

		public override void RegisterSteps()
		{
			startTime = DateTime.Now;
			Step step1 = new Step();
			step1.objList.Add(GetObj("GypsumEffect"));
			step1.OnClickObj += ClickObj;
			step1.CheckState += CheckState1;
			step1.tips = "取出铸造好的银戒指胚";
		}

		#region Step1
		void ClickObj()
		{
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working1());
		}

		IEnumerator working1()
		{
			WaitForSeconds wait2 = new WaitForSeconds(2);
			WaitForSeconds wait8 = new WaitForSeconds(8);
			WaitForSeconds wait3 = new WaitForSeconds(3);

			AnimStart("WaxTree", "WaxTree_Enter_Bucket",ViewType.Follow);
			yield return wait8;
			GetObj("GypsumEffect").GetComponent<MeshRenderer>().enabled = false;
			GetObj("Ring_Rough_Silver").SetActive(true);
			yield return new WaitForSeconds(1);
			AnimStart("Ring_Rough_Silver", "Ring_Rough_Silver_Up");
			yield return wait3;
			ModuleReportData report = new ModuleReportData()
			{
				title = "失蜡铸造_金属脱模",
				score = 3,
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