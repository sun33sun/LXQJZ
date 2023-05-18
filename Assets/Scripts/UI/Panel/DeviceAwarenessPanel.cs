using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.UI
{
	public class DeviceAwarenessPanel : BasePanel<DeviceAwarenessPanel>
	{
		[SerializeField] RectTransform leftTogRect;
		[SerializeField] Button btnExit;
		[SerializeField] List<Toggle> togList;
		[SerializeField] List<string> strHeaderList;
		[SerializeField] List<string> strList;
		[SerializeField] Image imgDevice;
		[SerializeField] Text txtHeader;
		[SerializeField] Text txtDevice;

		Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
		Dictionary<string, string> textDic = new Dictionary<string, string>();


		protected override void Start()
		{
			InitDic();
			InitListener();
			base.Start();
			StartCoroutine(HideAsync(0.05f));
		}

		private void InitDic()
		{
			for (int i = 0; i < ProjectSettings.DEVICES_IMAGE.Count; i++)
			{
				Sprite sprite = ResMgr.GetInstance().Load<Sprite>(ProjectSettings.DEVICES_IMAGE[i]);
				spriteDic.Add(ProjectSettings.DEVICES_IMAGE[i].Split('\\')[1], sprite);
			}
		}
		private void InitListener()
		{
			btnExit.onClick.AddListener(() =>
			{
				MainPanel.Instance.Show();
				Hide();
			});
			for (int i = 0; i < togList.Count; i++)
			{
				string key = string.Copy(togList[i].name);
				key = key.Remove(0, 3);
				RectTransform rect = togList[i].transform as RectTransform;
				int index = i;
				togList[i].onValueChanged.AddListener((isOn) =>
				{
					if (isOn)
					{
						rect.sizeDelta = new Vector2(264, 104);
						ChangeDeviceImage(key);
						txtHeader.text = strHeaderList[index];
						txtDevice.text = strList[index];
					}
					else
					{
						rect.sizeDelta = new Vector2(184, 75);
					}
					LayoutRebuilder.MarkLayoutForRebuild(leftTogRect);
				});
			}
		}

		private void ChangeDeviceImage(string key)
		{
			if (!spriteDic.ContainsKey(key))
			{
				Debug.Log($"Í¼Æ¬{key}²»´æÔÚ");
				return;
			}
			imgDevice.sprite = spriteDic[key];
		}

		protected override void InitState()
		{
			togList[0].isOn = true;
			string key = string.Copy(togList[0].name);
			key = key.Remove(0, 3);
			ChangeDeviceImage(key);
		}
	}
}

