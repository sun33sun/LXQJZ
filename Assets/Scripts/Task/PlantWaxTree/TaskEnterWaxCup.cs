using LXQJZ.Exam;
using LXQJZ.UI;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskEnterWaxCup : TaskBase
	{
		bool isSuccess1 = false;

		protected override void OnDisable()
		{
			isSuccess1 = false;
			base.OnDisable();
		}

		public override void InitState()
		{
			base.InitState();
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.tips = "将蜡树缓缓放入缠绕着胶带的盅里";
			step1.objList.Add(GetObj("WaxCup1"));
			step1.objList.Add(GetObj("WaxCup2"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState2;
		}

		#region Step1
		private void ClickObj1()
		{
			GetObj("WaxCup2").GetComponent<ObjClickEvent>().SelfDestroy();
			StartCoroutine(working2());
		}

		IEnumerator working2()
		{
			WaitForSeconds wait1 = new WaitForSeconds(1);

			AnimStart("WaxCup", "WaxCup_Cover_WaxTree");
			yield return wait1;
			GetObj("WaxCup").transform.SetParent(GetObj("WaxTree").transform);
			AnimStart("WaxCup", "WaxCup_Forward_WaxTree");

			Paper newPaper = ExamManager.GetInstance().GetPaper(ProjectSettings.PAPER_EnterWax);
			KnowledgeAssessmentPanel.Instance.ShowOnlineLabExam(newPaper, OnConfrimExam);
		}
		void OnConfrimExam(int score)
		{
			isSuccess1 = true;
			TaskManager.Instance.totalScore += score;
		}

		StepState CheckState2()
		{
			return CheckState(isSuccess1);
		}
		#endregion
	}
}