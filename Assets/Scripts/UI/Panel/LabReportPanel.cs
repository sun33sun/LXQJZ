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
		[SerializeField] Transform contentTrans;
		[Header("当前日期")]
		[SerializeField] Text txtNowDate;
		[Header("总成绩")]
		[SerializeField] Text txtTotalScore;
		int totalScore = 0;

		protected override void Start()
		{
			InitListener();
			base.Start();

			StartCoroutine(HideAsync(0.05f));
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

		public void CreateModuleReport(string moduleName, DateTime moduleStartTime, DateTime moduleEndTime, int moduleScore)
		{
			GameObject newReport = ResMgr.GetInstance().Load<GameObject>("ModuleReport");
			newReport.GetComponent<ModuleReport>().InitData(moduleName, moduleStartTime, moduleEndTime, moduleScore);
			newReport.transform.SetParent(contentTrans);
			totalScore += moduleScore;
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
