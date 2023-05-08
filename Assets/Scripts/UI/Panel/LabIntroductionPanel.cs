using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.UI
{
	public class LabIntroductionPanel : BasePanel<LabIntroductionPanel>
	{
		[SerializeField] Button btnExit;

		protected override void Start()
		{
			StartCoroutine(HideAsync(0.05f));

			btnExit.onClick.AddListener(() =>
			{
				MainPanel.Instance.Show();
				gameObject.SetActive(false);
			});
		}
	}
}