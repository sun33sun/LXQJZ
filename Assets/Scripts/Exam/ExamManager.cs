using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LXQJZ.Exam
{
	public class ExamManager : SingletonMono<ExamManager>
	{
		[SerializeField] GameObject singleTitle;
		[SerializeField] List<TextAsset> paperList;
		Dictionary<string, Paper> paperDic = null;

		List<ITitle> nowTitle = new List<ITitle>();
		Paper nowPaper = null;

		private void Start()
		{
			InitDic();
		}

		private void InitDic()
		{
			if (paperDic == null)
			{
				paperDic = new Dictionary<string, Paper>();
				for (int i = 0; i < paperList.Count; i++)
				{
					Paper newPaper = JsonConvert.DeserializeObject<Paper>(paperList[i].text);
					paperDic.Add(paperList[i].name, newPaper);
				}
			}

		}

		public Paper GetPaper(string paperName)
		{
			if (paperDic == null)
				InitDic();
			if (paperDic.ContainsKey(paperName))
			{
				nowPaper = paperDic[paperName];
				return nowPaper;
			}
			else
				return null;
		}

		public GameObject CreateTitle(TitleData source)
		{
			GameObject newObj = null;
			ITitle title = null;
			switch (source.titleType)
			{
				case TitleType.SingleChoice:
					newObj = Instantiate(singleTitle);
					title = newObj.GetComponent<SingleChoiceTitle>();
					break;
			}
			title.InitTitleData(source);
			return newObj;
		}
	}
}
