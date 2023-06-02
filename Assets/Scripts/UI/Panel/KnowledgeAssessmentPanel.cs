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
		[SerializeField] RectTransform titleFather;
		[SerializeField] List<ITitle> titleList = new List<ITitle>();
		[Header("�ύ��ť")]
		[SerializeField] Button btnSubmit;
		[SerializeField] Image imgLogo;
		//����
		Paper nowPaper = null;
		DateTime startTime;
		public int repeatCount = 0;
		public int maxScore = 50;

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
			clickSubmitCount = 0;
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

		IEnumerator LoadPaperAsync(Paper newPaper)
		{
			for (int i = titleList.Count - 1; i > -1; i--)
			{
				Destroy(titleList[i].gameObject);
				titleList.RemoveAt(i);
			}
			nowPaper = newPaper;
			for (int i = 0; i < nowPaper.dataList.Count; i++)
			{
				GameObject newObj = ExamManager.GetInstance().CreateTitle(nowPaper.dataList[i]);
				titleList.Add(newObj.GetComponent<ITitle>());
				newObj.transform.SetParent(titleFather);
			}
			btnSubmit.transform.SetAsLastSibling();
			LayoutRebuilder.ForceRebuildLayoutImmediate(titleFather);
			yield return null;
		}

		#region ������Ŀ
		public void ShowOnlineLabPaper(Paper newPaper, UnityAction<int> callBack)
		{
			//�������
			onlineLabExamCompleted = false;
			//UI����
			Show();
			GetComponent<Image>().enabled = false;
			StartCoroutine(LoadPaperAsync(newPaper));
			btnExit.gameObject.SetActive(false);
			imgLogo.gameObject.SetActive(false);
			//�ص�����
			OnOnlineLabSubmit += callBack;
		}

		public void ShowKnowledgePaper()
		{
			StartCoroutine(ShowKnowledgePaperAsync());
		}
		IEnumerator ShowKnowledgePaperAsync()
		{
			string json = File.ReadAllText(ProjectSettings.PAPER_Knowledge);
			nowPaper = JsonConvert.DeserializeObject<Paper>(json);
			yield return LoadPaperAsync(nowPaper);
		}
		#endregion


		bool onlineLabExamCompleted = false;
		public bool CheckOnlineLabExam()
		{
			return onlineLabExamCompleted;
		}

		IEnumerator DestroyPaperAsync()
		{

			for (int i = 0; i < titleList.Count; i++)
			{
				Destroy(titleList[i].gameObject);
				titleList.RemoveAt(i);
			}
			titleList.Clear();
			yield return null;
		}

		int clickSubmitCount = 0;
		void SubmitPaper()
		{
			clickSubmitCount++;
			if (clickSubmitCount < 2)
			{
				for (int i = 0; i < titleList.Count; i++)
				{
					titleList[i].ShowTip();
					titleList[i].SetInteractive(false);
				}
				return;
			}

			clickSubmitCount = 0;
			//����ʵ�鿼�˲���
			if (OnOnlineLabSubmit != null)
			{
				int totalScore = 0;
				for (int i = 0; i < titleList.Count; i++)
				{
					ITitle title = titleList[i];
					totalScore += title.Score;
				}
				OnOnlineLabSubmit.Invoke(totalScore);
				OnOnlineLabSubmit = null;
				//UI����
				Hide();
				GetComponent<Image>().enabled = true;
				for (int i = titleList.Count - 1; i >= 0; i--)
				{
					Destroy(titleList[i].gameObject);
					titleList.RemoveAt(i);
				}
				titleList.Clear();
				btnExit.gameObject.SetActive(true);
				imgLogo.gameObject.SetActive(true);
				//״̬����
				onlineLabExamCompleted = true;
				return;
			}
			else
			{
				int score = 0;
				for (int i = 0; i < titleList.Count; i++)
				{
					ITitle title = titleList[i];
					if (title.IsRight)
						score += title.Score;
				}
				string evaluation;
				float percentage = score / maxScore;
				if (percentage > 0.8)
					evaluation = "��";
				else if (percentage > 0.6)
					evaluation = "��";
				else
					evaluation = "��";
				repeatCount++;
				ModuleReportData newData = new ModuleReportData()
				{
					seq = 1,
					title = "֪ʶ����",
					startTime = this.startTime,
					endTime = DateTime.Now,
					expectTime = new TimeSpan(0, 10, 0),
					score = score,
					maxScore = titleList.Count * 5,
					repeatCount = repeatCount,
					evaluation = evaluation,
					scoringModel = "����ģ��",
					remarks = "֪ʶ���˵ı�ע",
					ext_data = "����ɶ����"
				};
				LabReportPanel.Instance.Show();
				LabReportPanel.Instance.CreateModuleReport(newData);
				Hide();
			}
		}

		public override IEnumerator HideAsync(float waitTime)
		{
			yield return ShowKnowledgePaperAsync();
			Hide();
		}
	}
}

