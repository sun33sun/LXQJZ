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
		[SerializeField] Button btnExit;
		[SerializeField] private List<Toggle> togList;
		[SerializeField] private Image imgDevice;
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
				togList[i].onValueChanged.AddListener((isOn) =>
				{
					if (isOn)
						ChangeDeviceImageAndText(key);
				});
			}
		}

		private void ChangeDeviceImageAndText(string key)
		{
			if (!spriteDic.ContainsKey(key))
			{
				Debug.Log($"ͼƬ{key}������");
				return;
			}
			imgDevice.sprite = spriteDic[key];
		}

		protected override void InitState()
		{
			togList[0].isOn = true;
			string key = string.Copy(togList[0].name);
			key = key.Remove(0, 3);
			ChangeDeviceImageAndText(key);
		}
	}
}

