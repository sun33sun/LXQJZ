using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LXQJZ.Exam;
using System.IO;
using Newtonsoft.Json;
using System;

namespace LXQJZ.UI
{
	public class KnowledgeAssessmentPanel : BasePanel<KnowledgeAssessmentPanel>
	{
		[SerializeField] Button btnExit;
		[Header("考试题部分UI")]
		[SerializeField] GameObject titleFather;
		[SerializeField] List<GameObject> titleList = new List<GameObject>();
		[Header("提交按钮")]
		[SerializeField] Button btnSubmit;
		//数据
		Paper nowPaper = null;
		DateTime startTime;
		DateTime endTime;

		protected override void Start()
		{
			StartCoroutine(HideAsync(0.05f));

			InitListener();
			//StartCoroutine(LoadPaper());
			base.Start();
		}
		private void OnEnable()
		{
			startTime = DateTime.Now;
		}

		void InitListener()
		{
			btnSubmit.onClick.AddListener(SubmitPaper);
			btnExit.onClick.AddListener(() =>
			{
				MainPanel.Instance.Show();
				Hide();
			});
		}

		IEnumerator LoadPaper()
		{
			//nowPaper = ExamManager.GetInstance().GetPaper("测试");
			string json = File.ReadAllText(ProjectSettings.EXAM_PAPER_JSON);
			nowPaper = JsonConvert.DeserializeObject<Paper>(json);

			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			for (int i = 0; i < nowPaper.dataList.Count; i++)
			{
				GameObject newObj = ExamManager.GetInstance().CreateTitle(nowPaper.dataList[i]);
				titleList.Add(newObj);
				newObj.transform.SetParent(titleFather.transform);
				yield return wait;
			}
		}

		void DestoryPaper()
		{
			for (int i = titleList.Count; i > -1; i--)
			{
				Destroy(titleList[i]);
				titleList.RemoveAt(i);
			}
			titleList.Clear();
		}

		void SubmitPaper()
		{
			//endTime = DateTime.Now;
			//int score = 0;
			//for (int i = 0; i < titleList.Count; i++)
			//{
			//	ITitle title = titleList[i].GetComponent<ITitle>();
			//	if (title.IsRight)
			//		score += title.Score;
			//}
			LabReportPanel.Instance.Show();
			//LabReportPanel.Instance.CreateModuleReport("知识考核", startTime, endTime, score);
			Hide();
		}
	}
}

