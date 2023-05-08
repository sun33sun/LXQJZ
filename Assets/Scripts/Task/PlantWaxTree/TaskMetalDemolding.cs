using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskMetalDemolding:TaskBase
	{
		bool isSuccess1 = false;

		protected override void OnDisable()
		{
			base.OnDisable();
			GameObject GypsumEffect = GetObj("GypsumEffect");
			if(GypsumEffect != null)
				GypsumEffect.GetComponent<MeshRenderer>().enabled = true;
		}

		public override void InitState()
		{
			modelName = "MetalDemolding";
			base.InitState();
			ParticleManager.Stop(GetObj("FireGun_Fire"));
			ParticleManager.Stop(GetObj("WeldingWaxMachine_Pen_Fire"));
			GetObj("Ring_Rough_Silver").SetActive(false);
		}

		public override void RegisterSteps()
		{
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

			AnimStart("GypsumEffect", "GypsumEffect_Enter_Bucket");
			yield return new WaitForSeconds(1.5f);
			GetObj("GypsumEffect").GetComponent<MeshRenderer>().enabled = false;
			GetObj("Ring_Rough_Silver").SetActive(true);
			yield return new WaitForSeconds(1);
			AnimStart("Ring_Rough_Silver", "Ring_Rough_Silver_From_Bucket_To_Table1");
			yield return wait2;
			isSuccess1 = true;
		}

		StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}
		#endregion
	}
}