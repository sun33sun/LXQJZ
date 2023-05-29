using LXQJZ.UI;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskManager : SingletonMono<TaskManager>
	{
		[SerializeField] List<TaskBase> taskList = new List<TaskBase>();
		int taskIndex = 0;
		int startIndex = 4;
		public Action OnLabCompleted;

		void OnNextTask()
		{
			if (taskIndex >= taskList.Count - 1)
			{
				OnLabCompleted?.Invoke();
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

		public void StartTask()
		{
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
