using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.Exam
{
	public class SingleChoiceTitle : MonoBehaviour, ITitle
	{
		TitleData data;
		public int Score => data.score;
		public bool IsRight => isRight;

		[SerializeField] List<Toggle> togList;
		Toggle togSelected = null;
		[SerializeField] List<Text> optionDescriptionList;
		[SerializeField] Text titleDescription;
		[Header("选择后提示信息")]
		[SerializeField] Text selectedTip;
		[Header("数据")]
		[SerializeField] Option rightOption = Option.None;
		[SerializeField] Option selectedOption = Option.None;
		[SerializeField] bool isRight = false;

		private void Start()
		{
			InitListener();
		}
		void InitListener()
		{
			for (int i = 0; i < togList.Count; i++)
			{
				Option nowOption = (Option)i;
				int index = i;
				togList[i].onValueChanged.AddListener((isOn) =>
				{
					if (!isOn)
					{
						if (togSelected == togList[index])
						{
							selectedTip.text = "";
							selectedTip.color = Color.white;
							togSelected = null;
							selectedOption = Option.None;
							isRight = false;
						}
						return;
					}
					else
					{
						if (togSelected != null)
							togSelected.isOn = false;
						selectedOption = nowOption;
						selectedTip.gameObject.SetActive(true);
						if (selectedOption == rightOption)
						{
							selectedTip.text = "回答正确！";
							selectedTip.color = Color.green;
							isRight = true;
						}
						else
						{
							selectedTip.text = "回答错误！正确答案为 " + rightOption;
							selectedTip.color = Color.red;

							isRight = false;
						}
						togSelected = togList[index];
					}

				});
			}
		}

		public void InitTitleData(TitleData source)
		{
			data = source;
			rightOption = data.rightOption;
			titleDescription.text = $"{data.titleNumber + 1}.{data.titleType.ToChinese()}\u00A0\u00A0{data.titleDescription}";
			for (int i = 0; i < optionDescriptionList.Count; i++)
			{
				optionDescriptionList[i].text = data.optionDescriptionList[i];
			}
		}
	}
}
