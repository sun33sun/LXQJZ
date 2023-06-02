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

		//����ʵ�鱨��
		public int maxScore = 50;
		public int score = 0;
		public int repeatCount = 0;
		DateTime startTime;

		void OnNextTask()
		{
			//����ʵ�����
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
			Debug.LogWarning("��ǰ�������Ϊ��" + taskIndex);
			taskList[taskIndex].enabled = true;
			if (taskList[taskIndex].modelName != null)
				taskList[taskIndex].InitState();
			taskList[taskIndex].RegisterSteps();
			StepManager.GetInstance().Start();
		}

		void TaskEndFun()
		{
			repeatCount++;
			EventCenter.GetInstance().EventTrigger("����ʵ�����");
			//����ʵ�鱨��
			string evaluation;
			float percentage = score / maxScore;
			if (percentage > 0.8)
				evaluation = "��";
			else if (percentage > 0.6)
				evaluation = "��";
			else
				evaluation = "��";
			ModuleReportData newData = new ModuleReportData()
			{
				seq = 0,
				title = "����ʵ��",
				startTime = this.startTime,
				endTime = DateTime.Now,
				expectTime = new TimeSpan(0, 10, 0),
				score = score,
				repeatCount = this.repeatCount,
				evaluation = evaluation,
				scoringModel = "����ģ��",
				remarks = "����ʵ��ı�ע",
				ext_data = "������ݵ�������ɶ��"
			};
			score = 0;
			LabReportPanel.Instance.CreateModuleReport(newData);
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
