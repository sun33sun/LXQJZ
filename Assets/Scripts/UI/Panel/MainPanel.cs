using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.UI
{
	public class MainPanel : BasePanel<MainPanel>
	{
		[SerializeField] List<Button> btnList;

		protected override void Start()
		{
			InitListener();
		}

		void InitListener()
		{
			btnList[0].onClick.AddListener(()=> {
				gameObject.SetActive(false);
				LabIntroductionPanel.Instance.Show();
				});
			btnList[1].onClick.AddListener(()=> 
			{
				gameObject.SetActive(false);
				DeviceAwarenessPanel.Instance.Show();
			});
			btnList[2].onClick.AddListener(()=> 
			{
				gameObject.SetActive(false);
				OnlineLabPanel.Instance.Show();
			});
			btnList[3].onClick.AddListener(()=> 
			{
				gameObject.SetActive(false);
				KnowledgeAssessmentPanel.Instance.Show();
				KnowledgeAssessmentPanel.Instance.ShowKnowledgePaper();
			});
			btnList[4].onClick.AddListener(()=>
			{
				gameObject.SetActive(false);
				LabReportPanel.Instance.Show();
			});
		}
	}

}
