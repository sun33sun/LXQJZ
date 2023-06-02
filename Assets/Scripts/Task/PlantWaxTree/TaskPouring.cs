using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskPouring : TaskBase
	{
		bool isSuccess1 = false;

		protected override void OnDisable()
		{
			isSuccess1 = false;
			base.OnDisable();
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.objList.Add(GetObj("Ladle_Metal"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;
			step1.tips = "取适量银块，倒入石膏桶中";
		}

		#region Step1
		void ClickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working1());
		}

		IEnumerator working1()
		{
			WaitForSeconds wait1 = new WaitForSeconds(1);
			WaitForSeconds wait2 = new WaitForSeconds(2);

			AnimStart("FireGun", "FireGun_Melt_Silver");
			yield return wait1;
			ParticleStart("FireGun_Fire", 2);
			yield return wait2;
			GetObj("SilverEffect").SetActive(true);
			GetObj("SilverCube").SetActive(false);
			AnimStart("Ladle_Metal", "Ladle_Metal_Pour_WaxTree");
			yield return wait1;
			GetObj("SilverEffect").SetActive(false);
			yield return wait1;
			TaskManager.Instance.ShowExam(ProjectSettings.PAPER_Pouring, OnConfirmExam1);
		}

		void OnConfirmExam1(int addScore)
		{
			TaskManager.Instance.score += addScore;
			isSuccess1 = true;
		}


		StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}
		#endregion

	}
}

