using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LXQJZ.Exam;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEngine.Events;

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
		[SerializeField] Image imgLogo;
		//数据
		Paper nowPaper = null;
		DateTime startTime;

		UnityAction<int> OnOnlineLabSubmit;

		protected override void Start()
		{
			StartCoroutine(HideAsync(0.05f));

			InitListener();
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

		public void LoadPaper(Paper newPaper)
		{
			StartCoroutine(LoadPaperAsync(newPaper));
		}

		IEnumerator LoadPaperAsync(Paper newPaper)
		{
			nowPaper = newPaper;
			for (int i = 0; i < nowPaper.dataList.Count; i++)
			{
				GameObject newObj = ExamManager.GetInstance().CreateTitle(nowPaper.dataList[i]);
				titleList.Add(newObj);
				newObj.transform.SetParent(titleFather.transform);
			}
			yield return null;
		}

		public void ShowOnlineLabExam(Paper newPaper, UnityAction<int> callBack)
		{
			//持续检查
			onlineLabExamCompleted = false;
			//UI显隐
			Show();
			GetComponent<Image>().enabled = false;
			LoadPaper(newPaper);
			btnExit.gameObject.SetActive(false);
			imgLogo.gameObject.SetActive(false);
			//回调订阅
			OnOnlineLabSubmit += callBack;
		}

		bool onlineLabExamCompleted = false;
		public bool CheckOnlineLabExam()
		{
			return onlineLabExamCompleted;
		}

		void DestroyPaper()
		{
			StartCoroutine(DestroyPaperAsync());
		}

		IEnumerator DestroyPaperAsync()
		{

			for (int i = 0; i < titleList.Count; i++)
			{
				Destroy(titleList[i]);
				titleList.RemoveAt(i);
			}
			titleList.Clear();
			yield return null;
		}

		void SubmitPaper()
		{
			//在线实验考核部分
			if (OnOnlineLabSubmit != null)
			{
				int totalScore = 0;
				for (int i = 0; i < titleList.Count; i++)
				{
					ITitle title = titleList[i].GetComponent<ITitle>();
					totalScore += title.Score;
				}
				OnOnlineLabSubmit.Invoke(totalScore);
				OnOnlineLabSubmit = null;
				//UI显隐
				Hide();
				GetComponent<Image>().enabled = true;
				for (int i = titleList.Count - 1; i >= 0; i--)
				{
					Destroy(titleList[i]);
					titleList.RemoveAt(i);
				}
				titleList.Clear();
				btnExit.gameObject.SetActive(true);
				imgLogo.gameObject.SetActive(true);
				//状态调整
				onlineLabExamCompleted = true;
				return;
			}
			else
			{
				int totalScore = 0;
				for (int i = 0; i < titleList.Count; i++)
				{
					ITitle title = titleList[i].GetComponent<ITitle>();
					if (title.IsRight)
						totalScore += title.Score;
				}
				ModuleReportData newData = new ModuleReportData()
				{
					moduleName = "知识考核",
					startTime = this.startTime,
					endTime = DateTime.Now,
					moduleScore = totalScore
				};
				LabReportPanel.Instance.Show();
				LabReportPanel.Instance.CreateModuleReport(newData);
				Hide();
			}
		}

		public override IEnumerator HideAsync(float waitTime)
		{
			string json = File.ReadAllText(ProjectSettings.PAPER_Knowledge);
			nowPaper = JsonConvert.DeserializeObject<Paper>(json);
			yield return LoadPaperAsync(nowPaper);
			Hide();
		}
	}
}

