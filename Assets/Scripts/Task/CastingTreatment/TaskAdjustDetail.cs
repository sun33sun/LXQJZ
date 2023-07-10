using LXQJZ.UI;
using System;
using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskAdjustDetail : TaskBase
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
			modelName = "AdjustDetail";
		}

		public override void RegisterSteps()
		{
			startTime = DateTime.Now;
			Step step1 = new Step();
			step1.tips = "用三角细锉调整细节，用圆形锉刀修整戒指爪镶凹槽的部分。";
			step1.objList.Add(GetObj("TriangularFile"));
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
			AnimStart("TriangularFile", "TriangularFile_Grind_Ring_Pre_Detail");
			yield return new WaitForSeconds(8);
			AnimStart("RoundFile", "RoundFile_Grind_Ring_Pre_Detail");
			yield return new WaitForSeconds(6);
			ModuleReportData report = new ModuleReportData()
			{
				title = "铸件处理_调整戒指细节",
				score = 2,
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