using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ.Exam
{
	public enum TitleType
	{
		SingleChoice,
		MultipleChoice,
	}

	public class TitleData : Object
	{
		public int titleNumber = 0;
		public List<string> optionDescriptionList;
		public int score;


		public TitleType titleType;
		public string titleDescription = "";

		public List<Option> rightOptionList;
		public Option rightOption;

		public TitleData() { }

		public TitleData(int titleNumber, List<string> optionDescriptionList, int score, string titleDescription, Option rightOption)
		{
			this.titleType = TitleType.SingleChoice;
			this.titleNumber = titleNumber;
			this.optionDescriptionList = optionDescriptionList;
			this.score = score;
			this.rightOption = rightOption;
			this.titleDescription = titleDescription;
		}

		public TitleData(int titleNumber, List<string> optionDescriptionList, int score, string titleDescription, List<Option> rightOptionList)
		{
			this.titleType = TitleType.MultipleChoice;
			this.titleNumber = titleNumber;
			this.optionDescriptionList = optionDescriptionList;
			this.score = score;
			this.rightOptionList = rightOptionList;
			this.titleDescription = titleDescription;
		}
	}

}
