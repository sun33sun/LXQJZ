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
			Debug.LogWarning("当前任务序号为：" + taskIndex);
			taskList[taskIndex].enabled = true;
			if (taskList[taskIndex].modelName != null)
				taskList[taskIndex].InitState();
			taskList[taskIndex].RegisterSteps();
			StepManager.GetInstance().Start();
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
