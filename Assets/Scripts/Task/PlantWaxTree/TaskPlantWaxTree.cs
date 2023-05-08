using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskPlantWaxTree : TaskBase
	{
		bool isSuccess1 = false;

		public override void InitState()
		{
			modelName = "PlantWaxTree";
			base.InitState();
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.objList.Add(GetObj("TreatedRing2"));
			step1.tips = "将雕刻好的戒指蜡模，固定在短蜡条上";
			step1.OnClickObj += CLickObj1;
			step1.CheckState += CheckState1;
		}

		#region Step1
		void CLickObj1()
		{
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working1());
		}

		IEnumerator working1()
		{
			WaitForSeconds wait2 = new WaitForSeconds(2);
			WaitForSeconds wait1 = new WaitForSeconds(1.1f);
			WaitForSeconds wait05 = new WaitForSeconds(0.5f);

			//固定蜡戒
			AnimStart("TreatedRing2", "TreatedRing2_Approach_WaxTree");
			yield return wait1;
			AnimStart("WeldingWaxMachine", "WeldingWaxMachine_Weld_WaxTree");
			yield return wait05;
			ParticleStart("WeldingWaxMachine_Pen_Fire", 2);
			yield return wait2;
			yield return wait05;
			GetObj("TreatedRing2").transform.SetParent(GetObj("GypsumEffect").transform);
			AnimStart("TreatedRing2", "TreatedRing2_Forward_GypsumEffect");
			isSuccess1 = true;
		}

		StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}

		#endregion

	}
}