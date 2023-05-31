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
		[SerializeField] Transform contentTrans;
		[Header("��ǰ����")]
		[SerializeField] Text txtNowDate;
		[Header("�ܳɼ�")]
		[SerializeField] Text txtTotalScore;
		int totalScore = 0;

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
			GameObject newReport = ResMgr.GetInstance().Load<GameObject>("Prefabs\\Exam_Prefab\\ModuleReport");
			newReport.name = "ModuleReport";
			newReport.GetComponent<ModuleReport>().InitData(newData);
			newReport.transform.SetParent(contentTrans);
			totalScore += newData.moduleScore;
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
	}
}
