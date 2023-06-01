using System.Collections;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskMetalDemolding:TaskBase
	{
		bool isSuccess1 = false;


		protected override void OnDisable()
		{
			isSuccess1 = false;
			base.OnDisable();
			GameObject GypsumEffect = GetObj("GypsumEffect");
			if(GypsumEffect != null)
				GypsumEffect.GetComponent<MeshRenderer>().enabled = true;
		}

		public override void InitState()
		{
			modelName = "MetalDemolding";
			base.InitState();
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

			AnimStart("WaxTree", "WaxTree_Enter_Bucket",ViewType.Follow);
			yield return new WaitForSeconds(4);
			GetObj("GypsumEffect").GetComponent<MeshRenderer>().enabled = false;
			GetObj("Ring_Rough_Silver").SetActive(true);
			yield return new WaitForSeconds(1);
			AnimStart("Ring_Rough_Silver", "Ring_Rough_Silver_Up");
			yield return new WaitForSeconds(1);
			isSuccess1 = true;
		}

		StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}
		#endregion
	}
}