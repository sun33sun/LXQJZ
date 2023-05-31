using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LXQJZ.Exam
{
	public class ExamManager : BaseManager<ExamManager>
	{
		List<ITitle> nowTitle = new List<ITitle>();
		Paper nowPaper = null;

		//private void InitPaper(string papaerName)
		//{
		//	if (paperDic == null)
		//	{
		//		string json = File.ReadAllText(papaerName);
		//		paperDic = JsonConvert.DeserializeObject<Dictionary<string, Paper>>(json);
		//	}
		//}

		public Paper GetPaper(string paperName)
		{
			string json = File.ReadAllText(paperName);
			nowPaper = JsonConvert.DeserializeObject<Paper>(json);
			return nowPaper;
		}

		public GameObject CreateTitle(TitleData source)
		{
			GameObject newObj = null;
			ITitle title = null;
			switch (source.titleType)
			{
				case TitleType.SingleChoice:
					newObj = ResMgr.GetInstance().Load<GameObject>(ProjectSettings.EXAM_SINGLECHOICETITLE);
					title = newObj.GetComponent<SingleChoiceTitle>();
					break;
				case TitleType.MultipleChoice:
					newObj = ResMgr.GetInstance().Load<GameObject>(ProjectSettings.EXAM_MULTIPLECHOICETITLE);
					title = newObj.GetComponent<MultipleChoiceTitle>();
					break;
			}
			title.InitTitleData(source);
			return newObj;
		}

		public void SaveTotalScore(List<TitleData> source)
		{

		}
	}
}
