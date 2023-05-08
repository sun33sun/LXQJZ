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
		[SerializeField] List<Text> optionDescriptionList;
		[SerializeField] Text titleType;
		[SerializeField] Text titleDescription;
		[Header("选择后提示信息")]
		[SerializeField] Text selectedTip;
		[Header("数据")]
		[SerializeField] Option rightOption;
		[SerializeField] Option selectedOption;
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
				togList[i].onValueChanged.AddListener((isOn) =>
				{
					if (!isOn)
						return;
					selectedOption = nowOption;
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
				});
			}
		}

		public void InitTitleData(TitleData source)
		{
			data = source;
			rightOption = data.rightOption;
			titleDescription.text = data.titleDescription;
			titleType.text = data.titleNumber + " . " + data.titleType.ToChinese();
			for (int i = 0; i < optionDescriptionList.Count; i++)
			{
				optionDescriptionList[i].text = data.optionDescriptionList[i];
			}
		}
	}
}
