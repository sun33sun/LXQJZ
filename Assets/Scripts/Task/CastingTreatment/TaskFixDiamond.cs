using LXQJZ.UI;
using QFramework;
using System;
using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskFixDiamond : TaskBase
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
			modelName = "FixDiamond";
		}

		public override void RegisterSteps()
		{
			startTime = DateTime.Now;
			Step step1 = new Step();
			step1.tips = "将钻石放在开好槽的爪镶底托上，准备开始镶嵌。";
			step1.objList.Add(GetObj("Diamond"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;

			Step step2 = new Step();
			step2.objList.Add(GetObj("NeedleNosePlier1"));
			step2.objList.Add(GetObj("NeedleNosePlier2"));
			step2.OnClickObj += ClickObj2;
			step2.CheckState += CheckState2;
		}

		#region Step1
		void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			ActionKit.Sequence()
				.Callback(() => { AnimStart("Diamond", "Diamond_FixTo_Ring_Polish"); })
				.Delay(2, () =>
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
		void ClickObj2()
		{
			RoamCamera.Instance.IsEnable = false;
			GetObj("NeedleNosePlier2").GetComponent<ObjClickEvent>().SelfDestroy(false);
			ActionKit.Sequence()
				.Callback(() => { AnimStart("NeedleNosePlier", "NeedleNosePlier_Fix_Diamond"); })
				.Delay(12, () =>
				{
					ModuleReportData report = new ModuleReportData()
					{
						title = "铸件处理_固定钻石",
						score = 2,
						startTime = this.startTime,
						endTime = DateTime.Now
					};
					LabReportPanel.Instance.CreateModuleReport(report);
					isSuccess2 = true;
				}).Start(this);
		}

		StepState CheckState2()
		{
			return CheckState(isSuccess2);
		}
		#endregion
	}
}