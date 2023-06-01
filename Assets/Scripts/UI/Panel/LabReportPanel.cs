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
		[Header("面板下需要隐藏的子物体")]
		[SerializeField] GameObject[] childObjs;
		[Header("当前日期")]
		[SerializeField] Text txtNowDate;
		[Header("总成绩")]
		[SerializeField] Text txtTotalScore;
		int totalScore = 0;
		[SerializeField] Dictionary<string, ModuleReport> reportDic = new Dictionary<string, ModuleReport>();
		[SerializeField] Button btnSubmit;
		[SerializeField] InputField inputEvaluate;
		[SerializeField] RectTransform Grid;
		[SerializeField] RectTransform Content;

		protected override void Start()
		{
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
			if (!reportDic.ContainsKey(newData.moduleName))
			{
				//设置Obj
				GameObject newObj = ResMgr.GetInstance().Load<GameObject>("Prefabs\\Exam_Prefab\\ModuleReport");
				newObj.name = newData.moduleName;
				newObj.transform.SetParent(Grid);
				LayoutRebuilder.ForceRebuildLayoutImmediate(Grid);
				LayoutRebuilder.ForceRebuildLayoutImmediate(Content);
				//Report
				newReoprt = newObj.GetComponent<ModuleReport>();
				reportDic.Add(newData.moduleName, newReoprt);
			}
			else
			{
				newReoprt = reportDic[newData.moduleName];
				totalScore -= newReoprt.mData.moduleScore;
			}
			newReoprt.InitData(newData);

			totalScore += newReoprt.mData.moduleScore;
			txtTotalScore.text = "总成绩：" + totalScore.ToString();
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
	}
}
