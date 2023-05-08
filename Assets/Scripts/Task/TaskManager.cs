using LXQJZ.UI;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ.Task
{
	public class TaskManager : SingletonMono<TaskManager>
	{
		[SerializeField] List<TaskBase> taskList = new List<TaskBase>();
		int taskIndex = 0;
		int startIndex = 0;

		void OnNextTask()
		{
			if (taskIndex > taskList.Count - 2)
				return;

			taskList[taskIndex].enabled = false;
			if (taskList[taskIndex].modelName != null)
				Destroy(taskList[taskIndex].modelInstance);

			OnlineLabPanel.Instance.NextTask();
			taskIndex++;
			Debug.LogError("��ǰ�������Ϊ��" + taskIndex);
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
			//��Ȧ����
			taskList.Add(GetComponent<TaskRingMaking>());
			//צ������
			taskList.Add(GetComponent<TaskClawInlayProduction>());
			//צ���������
			taskList.Add(GetComponent<TaskClawInlaySurfaceRroduction>());

			//ʧ������
			//������
			taskList.Add(GetComponent<TaskPlantWaxTree>());
			//����
			taskList.Add(GetComponent<TaskEnterWaxCup>());
			//����ʯ��
			taskList.Add(GetComponent<TaskPouringGypsum>());
			//����
			taskList.Add(GetComponent<TaskDewaxing>());
			//����
			taskList.Add(GetComponent<TaskPouring>());
			//������ģ
			taskList.Add(GetComponent<TaskMetalDemolding>());
			//����
			taskList.Add(GetComponent<TaskSoakingSO4>());

			//��������
			//����ˮ��
			taskList.Add(GetComponent<TaskTreatmentWaterOutlet>());
			//������ָϸ��
			taskList.Add(GetComponent<TaskAdjustDetail>());
			//�׹��ָ
			taskList.Add(GetComponent<TaskPolishRing>());
			//׼����Ƕ��ʯ
			taskList.Add(GetComponent<TaskPrepareSetDiamond>());
			//�̶���ʯ
			taskList.Add(GetComponent<TaskFixDiamond>());
			//����צ��
			taskList.Add(GetComponent<TaskTrimClawInlay>());
			//��ָ���׹�����
			taskList.Add(GetComponent<TaskPolishAndCleanRing>());
			//��ɽ�ָ
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
