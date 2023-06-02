using LXQJZ.Exam;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.UI
{
	public class LabReportPanel : BasePanel<LabReportPanel>
	{
		[SerializeField] Button btnExit;
		[Header("�������Ҫ���ص�������")]
		[SerializeField] GameObject[] childObjs;
		[Header("��ǰ����")]
		[SerializeField] Text txtNowDate;
		[Header("�ܳɼ�")]
		[SerializeField] Text txtTotalScore;
		public int totalScore = 0;
		[SerializeField] Dictionary<string, ModuleReport> reportDic = new Dictionary<string, ModuleReport>();
		[SerializeField] Button btnSubmit;
		[SerializeField] InputField inputEvaluate;
		[SerializeField] RectTransform Grid;
		[SerializeField] RectTransform Content;

		public DateTime startTime;

		protected override void Start()
		{
			startTime = DateTime.Now;
			InitListener();
			base.Start();

			StartCoroutine(HideAsync(0.1f));
		}

		private void FixedUpdate()
		{
			txtNowDate.text = DateTime.Now.ToString("g");
		}

		void InitListener()
		{
			btnExit.onClick.AddListener(() =>
			{
				MainPanel.Instance.Show();
				Hide();
			});
		}

		public void CreateModuleReport(ModuleReportData newData)
		{
			ModuleReport newReoprt = null;
			if (!reportDic.ContainsKey(newData.title))
			{
				//����Obj
				GameObject newObj = ResMgr.GetInstance().Load<GameObject>("Prefabs\\Exam_Prefab\\ModuleReport");
				newObj.name = newData.title;
				newObj.transform.SetParent(Grid);
				LayoutRebuilder.ForceRebuildLayoutImmediate(Grid);
				LayoutRebuilder.ForceRebuildLayoutImmediate(Content);
				//Report
				newReoprt = newObj.GetComponent<ModuleReport>();
				reportDic.Add(newData.title, newReoprt);
			}
			else
			{
				newReoprt = reportDic[newData.title];
				totalScore -= newReoprt.mData.score;
			}
			newReoprt.InitData(newData);

			totalScore += newReoprt.mData.score;
			txtTotalScore.text = "�ܳɼ���" + totalScore.ToString();
		}

		public override void Hide()
		{
			for (int i = 0; i < childObjs.Length; i++)
			{
				childObjs[i].SetActive(false);
			}
		}

		public override void Show()
		{
			for (int i = 0; i < childObjs.Length; i++)
			{
				childObjs[i].SetActive(true);
			}
		}


		public SubmitData CreateSubmitData()
		{
			List<Step> steps = new List<Step>();
			foreach (var item in reportDic)
			{
				Step newStep = new Step()
				{
					seq = item.Value.mData.seq,
					title = item.Value.mData.title,
					startTime = item.Value.mData.startTime,
					endTime = item.Value.mData.endTime,
					timeUsed = item.Value.mData.startTime - item.Value.mData.endTime,
					expectTime = item.Value.mData.expectTime,
					maxScore = item.Value.mData.maxScore,
					score = item.Value.mData.score,
					repeatCout = item.Value.mData.repeatCount,
					evaluation = item.Value.mData.evaluation,
					scoringModel = item.Value.mData.scoringModel,
					remarks = item.Value.mData.remarks,
					ext_data = item.Value.mData.ext_data
				};
				steps.Add(newStep);
			}
			Context newContext = new Context()
			{
				username = "username",
				title = "������Ƕ��ָ",
				status = 1,
				score = totalScore,
				startTime = startTime,
				endTIme = DateTime.Now,
				timeUsed = startTime - DateTime.Now,
				appid = 100001,
				originId = 1,
				group_id = 1,
				group_name = "��������",
				role_in_group ="ѧ������ʦ",
				steps = steps
			};

			SubmitData submitData = new SubmitData()
			{
				customName = "������ѧ",
				accountNumber = "test",
				contextJson = JsonConvert.SerializeObject(newContext)
			};
			return submitData;
		}
	}
}
