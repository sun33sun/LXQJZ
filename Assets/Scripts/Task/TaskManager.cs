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
		int startIndex = 12;

		//生成实验报告
		public int maxScore = 50;
		public int score = 0;
		public int repeatCount = 0;
		DateTime startTime;

		void OnNextTask()
		{
			//在线实验完成
			if (taskIndex >= taskList.Count - 1)
			{
				TaskEndFun();
				return;
			}

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
			repeatCount++;
			EventCenter.GetInstance().EventTrigger("在线实验结束");
			//创建实验报告
			string evaluation;
			float percentage = score / maxScore;
			if (percentage > 0.8)
				evaluation = "优";
			else if (percentage > 0.6)
				evaluation = "良";
			else
				evaluation = "差";
			ModuleReportData newData = new ModuleReportData()
			{
				seq = 0,
				title = "在线实验",
				startTime = this.startTime,
				endTime = DateTime.Now,
				expectTime = new TimeSpan(0, 10, 0),
				score = score,
				repeatCount = this.repeatCount,
				evaluation = evaluation,
				scoringModel = "赋分模型",
				remarks = "在线实验的备注",
				ext_data = "这个数据的意义是啥？"
			};
			score = 0;
			LabReportPanel.Instance.CreateModuleReport(newData);
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

		public void ShowExam(string paperName, UnityAction<int> callBack)
		{
			Paper newPaper = ExamManager.GetInstance().GetPaper(paperName);
			KnowledgeAssessmentPanel.Instance.ShowOnlineLabPaper(newPaper, callBack);
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
