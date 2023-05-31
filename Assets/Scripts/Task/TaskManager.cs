using LXQJZ.Exam;
using LXQJZ.UI;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LXQJZ.Task
{
	public class TaskManager : SingletonMono<TaskManager>
	{
		[SerializeField] List<TaskBase> taskList = new List<TaskBase>();
		int taskIndex = 0;
		int startIndex = 4;
		public Action OnLabCompleted;

		//生成实验报告
		public int totalScore = 0;
		DateTime startTime;


		void OnNextTask()
		{
			//在线实验完成
			if (taskIndex >= taskList.Count - 1)
				TaskEndFun();

			taskList[taskIndex].enabled = false;
			if (taskList[taskIndex].modelName != null)
				Destroy(taskList[taskIndex].modelInstance);

			OnlineLabPanel.Instance.NextTask();
			taskIndex++;
			Debug.LogWarning("当前任务序号为：" + taskIndex);
			taskList[taskIndex].enabled = true;
			if (taskList[taskIndex].modelName != null)
				taskList[taskIndex].InitState();
			taskList[taskIndex].RegisterSteps();
			StepManager.GetInstance().Start();
		}

		void TaskEndFun()
		{
			OnLabCompleted?.Invoke();
			//创建实验报告
			ModuleReportData newData = new ModuleReportData()
			{
				moduleName = "在线实验",
				startTime = this.startTime,
				endTime = DateTime.Now,
				moduleScore = totalScore
			};
			totalScore = 0;
			LabReportPanel.Instance.CreateModuleReport(newData);
			return;
		}

		protected override void Awake()
		{
			base.Awake();

			EventCenter.GetInstance().AddEventListener("下一个任务", OnNextTask);

			//雕刻蜡模
			//戒圈制作0
			taskList.Add(GetComponent<TaskRingMaking>());
			//爪镶制作1
			taskList.Add(GetComponent<TaskClawInlayProduction>());
			//爪镶戒面制作2
			taskList.Add(GetComponent<TaskClawInlaySurfaceRroduction>());

			//失蜡铸造
			//种蜡树3
			taskList.Add(GetComponent<TaskPlantWaxTree>());
			//进盅4
			taskList.Add(GetComponent<TaskEnterWaxCup>());
			//浇筑石膏5
			taskList.Add(GetComponent<TaskPouringGypsum>());
			//脱蜡6
			taskList.Add(GetComponent<TaskDewaxing>());
			//浇铸7
			taskList.Add(GetComponent<TaskPouring>());
			//金属脱模8
			taskList.Add(GetComponent<TaskMetalDemolding>());
			//泡酸9
			taskList.Add(GetComponent<TaskSoakingSO4>());

			//铸件处理
			//处理水口10
			taskList.Add(GetComponent<TaskTreatmentWaterOutlet>());
			//调整戒指细节11
			taskList.Add(GetComponent<TaskAdjustDetail>());
			//抛光戒指12
			taskList.Add(GetComponent<TaskPolishRing>());
			//准备镶嵌钻石13
			taskList.Add(GetComponent<TaskPrepareSetDiamond>());
			//固定钻石14
			taskList.Add(GetComponent<TaskFixDiamond>());
			//修整爪镶15
			taskList.Add(GetComponent<TaskTrimClawInlay>());
			//戒指的抛光和清洁16
			taskList.Add(GetComponent<TaskPolishAndCleanRing>());
			//烘干戒指17
			taskList.Add(GetComponent<TaskDryRing>());
		}

		public void ShowExam(string paperName,UnityAction<int> callBack)
		{
			Paper newPaper = ExamManager.GetInstance().GetPaper(paperName);
			KnowledgeAssessmentPanel.Instance.ShowOnlineLabExam(newPaper,callBack);
		}
		public bool CheckExam()
		{
			return KnowledgeAssessmentPanel.Instance.CheckOnlineLabExam();
		}

		public void StartTask()
		{
			startTime = DateTime.Now;
			taskIndex = startIndex;

			taskList[taskIndex].enabled = true;
			if (taskList[taskIndex].modelName != null)
				taskList[taskIndex].InitState();
			taskList[taskIndex].RegisterSteps();
			StepManager.GetInstance().Start();
		}

		public void Exit()
		{
			Destroy(taskList[taskIndex].modelInstance);
			taskList[taskIndex].enabled = false;
			taskIndex = startIndex;
		}
	}
}
