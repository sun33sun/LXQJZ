using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LXQJZ.Exam;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEngine.Events;
using QFramework;

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
				GameObject newObj = ExamManager.Instance.CreateTitle(nowPaper.dataList[i]);
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
			OnOnlineLabSubmit = null;
			OnOnlineLabSubmit += callBack;
		}

		public void ShowKnowledgePaper()
		{
			StartCoroutine(ShowKnowledgePaperAsync());
		}
		IEnumerator ShowKnowledgePaperAsync()
		{
			nowPaper = ExamManager.Instance.GetPaper(ProjectSettings.PAPER_Knowledge);
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
				FirstSubmit();
				return;
			}
			clickSubmitCount = 0;
			SecondSubmit();
		}

		void FirstSubmit()
		{
			//�����Ŀ
			for (int i = 0; i < titleList.Count; i++)
			{
				titleList[i].ShowTip();
				titleList[i].SetInteractive(false);
			}
			//�������
			int totalScore = 0;
			for (int i = 0; i < titleList.Count; i++)
			{
				ITitle title = titleList[i];
				if (title.IsRight)
					totalScore += title.Score;
			}
			//�ύ�Ծ�
			if (OnOnlineLabSubmit != null)
			{
				OnOnlineLabSubmit?.Invoke(totalScore);
				return;
			}
			else
			{
				ModuleReportData newData = new ModuleReportData()
				{
					title = "֪ʶ����",
					startTime = this.startTime,
					endTime = DateTime.Now,
					score = totalScore,
				};
				LabReportPanel.Instance.CreateModuleReport(newData);
			}
		}

		void SecondSubmit()
		{
			Hide();
			if (OnOnlineLabSubmit != null)
			{
				OnOnlineLabSubmit = null;
				GetComponent<Image>().enabled = true;
				btnExit.gameObject.SetActive(true);
				imgLogo.gameObject.SetActive(true);
				//״̬����
				onlineLabExamCompleted = true;
			}
			else
			{
				LabReportPanel.Instance.Show();
			}
		}

		public override IEnumerator HideAsync(float waitTime)
		{
			yield return ShowKnowledgePaperAsync();
			Hide();
		}
	}
}

