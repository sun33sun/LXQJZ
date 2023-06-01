using Newtonsoft.Json;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.UI
{
	public class Device
	{
		public string nameCN = "";
		public string description = "";
		public List<string> spriteList = new List<string>();
	}
	public class DeviceAwarenessPanel : BasePanel<DeviceAwarenessPanel>
	{
		[SerializeField] RectTransform leftTogRect;
		[SerializeField] Button btnExit;
		[SerializeField] List<Toggle> togList;
		[SerializeField] Transform transDevice;
		[SerializeField] Text txtHeader;
		[SerializeField] Text txtDescription;

		Dictionary<string, Device> deviceDic = new Dictionary<string, Device>();
		[SerializeField] Dictionary<string, List<Sprite>> spriteDic = new Dictionary<string, List<Sprite>>();
		ResLoader resLoader = ResLoader.Allocate();
		List<GameObject> childs = new List<GameObject>();

		protected override void Start()
		{
			InitData();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				foreach (var item in spriteDic.Values)
				{
					foreach (var sprite in item)
					{
						Debug.Log(sprite.name);
					}
				}
			}
		}

		private void InitData()
		{
			ResKit.InitAsync().ToAction().Start(this, () =>
			 {
				 resLoader.Add2Load<TextAsset>("Devices", "deviceDic");
				 resLoader.LoadAsync(() =>
				 {
					 TextAsset jsonAsset = resLoader.LoadSync<TextAsset>("deviceDic");
					 deviceDic = JsonConvert.DeserializeObject<Dictionary<string, Device>>(jsonAsset.text);
					 foreach (var item in deviceDic)
					 {
						 List<string> spriteList = item.Value.spriteList;
						 string newKey = item.Key;
						 spriteDic.Add(newKey, new List<Sprite>());
						 for (int j = 0; j < spriteList.Count; j++)
						 {
							 resLoader.Add2Load<Sprite>("Devices", spriteList[j], (isSuccess, newAsset) =>
							 {
								 if (isSuccess)
									 spriteDic[newKey].Add(newAsset.Asset.As<Sprite>());
							 });
						 }
					 }
					 resLoader.LoadAsync(()=> { InitListener(); });
				 });
			 });
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
				RectTransform rect = togList[i].transform as RectTransform;
				GameObject objBk = togList[i].transform.GetChild(0).gameObject;
				togList[i].onValueChanged.AddListener((isOn) =>
				{
					objBk.SetActive(!isOn);
					if (isOn)
					{
						rect.sizeDelta = new Vector2(264, 104);
						ChangeTextAndImage(key);
					}
					else
					{
						rect.sizeDelta = new Vector2(184, 75);
					}
					LayoutRebuilder.MarkLayoutForRebuild(leftTogRect);
				});
			}
			InitState();
			Hide();
		}

		private void ChangeTextAndImage(string key)
		{
			//É¾³ýÔàÍ¼Æ¬

			if (childs != null && childs.Count > 0)
			{
				for (int i = childs.Count - 1; i > -1; i--)
					Destroy(childs[i].gameObject);
			}

			for (int i = 0; i < deviceDic[key].spriteList.Count; i++)
			{
				GameObject newObj = new GameObject();
				newObj.name = deviceDic[key].spriteList[i];
				childs.Add(newObj);
				
				Image newImg = newObj.AddComponent<Image>();
				newImg.sprite = spriteDic[key][i];

				newObj.transform.SetParent(transDevice);
				(newObj.transform as RectTransform).sizeDelta = new Vector2(400, 400);
			}
			txtHeader.text = deviceDic[key].nameCN;
			txtDescription.text = deviceDic[key].description;
		}

		protected override void InitState()
		{
			togList[0].isOn = true;
			ChangeTextAndImage(togList[0].name);
			togList[0].transform.GetChild(0).gameObject.SetActive(false);
		}
	}
}

