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
		[Header("�����ⲿ��UI")]
		[SerializeField] GameObject titleFather;
		[SerializeField] List<GameObject> titleList = new List<GameObject>();
		[Header("�ύ��ť")]
		[SerializeField] Button btnSubmit;
		//����
		Paper nowPaper = null;
		DateTime startTime;
		DateTime endTime;

		UnityAction<int> OnOnlineLabSubmit;

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
			//UI����
			Show();
			GetComponent<Image>().enabled = false;
			LoadPaper(newPaper);
			//�ص�����
			OnOnlineLabSubmit += callBack;
		}

		void TestPaper()
		{
			string json = File.ReadAllText(ProjectSettings.PAPER_Knowledge);
			nowPaper = JsonConvert.DeserializeObject<Paper>(json);
			LoadPaperAsync(nowPaper);
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
			//����ʵ�鿼�˲���
			if (OnOnlineLabSubmit != null)
			{
				int totalScore = 0;
				for (int i = 0; i < titleList.Count; i++)
				{
					switch (nowPaper.dataList[i].titleType)
					{
						case TitleType.SingleChoice:
							totalScore += titleList[i].GetComponent<SingleChoiceTitle>().Score;
							break;
						case TitleType.MultipleChoice:
							totalScore += titleList[i].GetComponent<MultipleChoiceTitle>().Score;
							break;
					}
				}
				OnOnlineLabSubmit.Invoke(totalScore);
				OnOnlineLabSubmit = null;
				//UI����
				Hide();
				GetComponent<Image>().enabled = true;
				DestroyPaper();
				return;
			}

			//endTime = DateTime.Now;
			//int score = 0;
			//for (int i = 0; i < titleList.Count; i++)
			//{
			//	ITitle title = titleList[i].GetComponent<ITitle>();
			//	if (title.IsRight)
			//		score += title.Score;
			//}
			LabReportPanel.Instance.Show();
			//LabReportPanel.Instance.CreateModuleReport("֪ʶ����", startTime, endTime, score);
			Hide();


		}
	}
}

