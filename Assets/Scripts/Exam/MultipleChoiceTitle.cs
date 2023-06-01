using LXQJZ.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.Exam
{
	public class MultipleChoiceTitle : MonoBehaviour, ITitle
	{
		TitleData data;
		public int Score => data.score;
		public bool IsRight => isRight;

		[SerializeField] MultipleToggle mt;
		[SerializeField] List<Text> optionDescriptionList;
		[SerializeField] Text titleType;
		[SerializeField] Text titleDescription;
		[Header("选择后提示信息")]
		[SerializeField] Text selectedTip;
		[Header("数据")]
		[SerializeField] List<Option> rightOptionList = new List<Option>();
		[SerializeField] List<Option> selectedOptionList = new List<Option>();
		[SerializeField] bool isRight = false;

		void OnGroupValueChanged(List<int> valueGroup)
		{
			selectedOptionList = IntListToOptionList(valueGroup);

			if (selectedOptionList.Count == rightOptionList.Count)
			{
				bool isEquals = true;
				for (int i = 0; i < selectedOptionList.Count; i++)
				{
					if (!rightOptionList.Contains(selectedOptionList[i]))
					{
						isEquals = false;
						break;
					}
				}
				if (isEquals)
				{
					selectedTip.text = "回答正确！";
					selectedTip.color = Color.green;
					isRight = true;
				}
				else
				{
					StringBuilder sb = new StringBuilder();
					for (int i = 0; i < rightOptionList.Count; i++)
						sb.Append(Enum.GetName(typeof(Option), rightOptionList[i]) + " ");

					selectedTip.text = "回答错误！正确答案为 " + sb;
					selectedTip.color = Color.red;
					isRight = false;
				}
			}
			else
			{
				selectedTip.text = "";
				selectedTip.color = Color.black;
			}
		}

		public List<Option> IntListToOptionList(List<int> intList)
		{
			List<Option> nowOptions = new List<Option>();
			foreach (var item in intList)
				nowOptions.Add((Option)item);
			return nowOptions;
		}

		public void InitTitleData(TitleData source)
		{
			//1、初始化UI状态
			selectedTip.text = "";
			selectedTip.color = Color.black;
			titleType.text = data.titleNumber + "." + data.titleType.ToChinese();
			titleDescription.text = $"{data.titleDescription}";
			for (int i = 0; i < optionDescriptionList.Count; i++)
				optionDescriptionList[i].text = data.optionDescriptionList[i];
			//2、订阅事件
			mt.onValueGroupChanged -= OnGroupValueChanged;
			mt.onValueGroupChanged += OnGroupValueChanged;
			//3、设置数据
			if (data != null && data != source)//想要重置题目状态时会将自己持有的数据传入
				Destroy(data);
			data = source;
			rightOptionList = data.rightOptionList;
		}

		public void ShowTip()
		{
			throw new NotImplementedException();
		}

		public void SetInteractive(bool isInteractive)
		{
			throw new NotImplementedException();
		}
	}
}
