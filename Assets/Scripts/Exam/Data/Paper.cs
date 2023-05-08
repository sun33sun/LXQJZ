using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ.Exam
{
	public class Paper
	{
		public string paperName;
		public List<TitleData> dataList;
		public Paper(List<TitleData> dataList, string paperName)
		{
			this.dataList = dataList;
			this.paperName = paperName;
		}
	}
}