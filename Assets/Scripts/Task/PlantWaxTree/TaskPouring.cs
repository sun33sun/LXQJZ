using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskPouring : TaskBase
	{
		bool isSuccess1 = false;

		void OnEnable()
		{
			ParticleManager.Stop(GetObj("FireGun_Fire"));
			ParticleManager.Stop(GetObj("WeldingWaxMachine_Pen_Fire"));
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
			AnimStart("Ladle_Metal", "Ladle_Metal_Pour_WaxTree");
			yield return wait1;
			GetObj("SilverEffect").SetActive(false);
			yield return wait1;
			isSuccess1 = true;
		}


		StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}
		#endregion

	}
}

