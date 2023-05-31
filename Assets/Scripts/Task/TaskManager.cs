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

		//����ʵ�鱨��
		public int totalScore = 0;
		DateTime startTime;


		void OnNextTask()
		{
			//����ʵ�����
			if (taskIndex >= taskList.Count - 1)
				TaskEndFun();

			taskList[taskIndex].enabled = false;
			if (taskList[taskIndex].modelName != null)
				Destroy(taskList[taskIndex].modelInstance);

			OnlineLabPanel.Instance.NextTask();
			taskIndex++;
			Debug.LogWarning("��ǰ�������Ϊ��" + taskIndex);
			taskList[taskIndex].enabled = true;
			if (taskList[taskIndex].modelName != null)
				taskList[taskIndex].InitState();
			taskList[taskIndex].RegisterSteps();
			StepManager.GetInstance().Start();
		}

		void TaskEndFun()
		{
			OnLabCompleted?.Invoke();
			//����ʵ�鱨��
			ModuleReportData newData = new ModuleReportData()
			{
				moduleName = "����ʵ��",
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

			EventCenter.GetInstance().AddEventListener("��һ������", OnNextTask);

			//�����ģ
			//��Ȧ����0
			taskList.Add(GetComponent<TaskRingMaking>());
			//צ������1
			taskList.Add(GetComponent<TaskClawInlayProduction>());
			//צ���������2
			taskList.Add(GetComponent<TaskClawInlaySurfaceRroduction>());

			//ʧ������
			//������3
			taskList.Add(GetComponent<TaskPlantWaxTree>());
			//����4
			taskList.Add(GetComponent<TaskEnterWaxCup>());
			//����ʯ��5
			taskList.Add(GetComponent<TaskPouringGypsum>());
			//����6
			taskList.Add(GetComponent<TaskDewaxing>());
			//����7
			taskList.Add(GetComponent<TaskPouring>());
			//������ģ8
			taskList.Add(GetComponent<TaskMetalDemolding>());
			//����9
			taskList.Add(GetComponent<TaskSoakingSO4>());

			//��������
			//����ˮ��10
			taskList.Add(GetComponent<TaskTreatmentWaterOutlet>());
			//������ָϸ��11
			taskList.Add(GetComponent<TaskAdjustDetail>());
			//�׹��ָ12
			taskList.Add(GetComponent<TaskPolishRing>());
			//׼����Ƕ��ʯ13
			taskList.Add(GetComponent<TaskPrepareSetDiamond>());
			//�̶���ʯ14
			taskList.Add(GetComponent<TaskFixDiamond>());
			//����צ��15
			taskList.Add(GetComponent<TaskTrimClawInlay>());
			//��ָ���׹�����16
			taskList.Add(GetComponent<TaskPolishAndCleanRing>());
			//��ɽ�ָ17
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
