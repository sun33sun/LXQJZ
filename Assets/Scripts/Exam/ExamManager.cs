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
		Dictionary<string, Paper> paperDic = null;
		Paper nowPaper = null;

		private void InitPaperDic()
		{
			if (paperDic == null)
			{
				string json = File.ReadAllText(ProjectSettings.EXAM_PAPER_JSON);
				paperDic = JsonConvert.DeserializeObject<Dictionary<string, Paper>>(json);
			}
		}

		public Paper GetPaper(string paperName)
		{
			InitPaperDic();
			if (!paperDic.ContainsKey(paperName))
				return null;
			nowPaper = paperDic[paperName];
			return nowPaper;
		}

		public GameObject CreateTitle(TitleData source)
		{
			GameObject newObj = null;
			switch (source.titleType)
			{
				case TitleType.SingleChoice:
					newObj = ResMgr.GetInstance().Load<GameObject>(ProjectSettings.EXAM_SINGLECHOICETITLE);
					break;
				case TitleType.MultipleChoice:
					newObj = ResMgr.GetInstance().Load<GameObject>(ProjectSettings.EXAM_MULTIPLECHOICETITLE);
					break;
			}
			ITitle tile = newObj.GetComponent<ITitle>();
			tile.InitTitleData(source);
			return newObj;
		}

		public void SaveTotalScore(List<TitleData> source)
		{

		}
	}
}
