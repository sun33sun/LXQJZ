using LXQJZ.Exam;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace LXQJZ
{
	public static class ProjectSettings
	{
		public static string LAB_INTRODUCTION_JSON { get { return Application.streamingAssetsPath + "\\LabIntroduction\\description.json"; } }

		#region 试题
		public static string EXAM_MULTIPLECHOICETITLE { get { return "Prefabs\\Exam_Prefab\\MultipleChoiceTitle"; } }
		public static string EXAM_SINGLECHOICETITLE { get { return "Prefabs\\Exam_Prefab\\SingleChoiceTitle"; } }
		public static string PAPER_Knowledge { get { return Application.streamingAssetsPath + "\\PaperKnowledge.json"; } }
		public static string PAPER_EnterWax { get { return Application.streamingAssetsPath + "\\PaperEnterWax.json"; } }
		public static string PAPER_PouringGypsum { get { return Application.streamingAssetsPath + "\\PaperPouringGypsum.json"; } }
		public static string PAPER_RingMaking { get { return Application.streamingAssetsPath + "\\PaperRingMaking.json"; } }
		public static string PAPER_Dewaxing { get { return Application.streamingAssetsPath + "\\PaperDewaxing.json"; } }
		public static string PAPER_Pouring { get { return Application.streamingAssetsPath + "\\PaperPouring.json"; } }
		public static string PAPER_SoakingSO4 { get { return Application.streamingAssetsPath + "\\PaperSoakingSO4"; } }
		#endregion


		#region 保存字典
		public static void GENERATE_PNG_JSON(string rootPath, string jsonName)
		{
			Dictionary<string, List<string>> pngDic = new Dictionary<string, List<string>>();

			string rootName = System.IO.Path.GetDirectoryName(rootPath);

			string[] dirPathArray = Directory.GetDirectories(rootPath);

			for (int i = 0; i < dirPathArray.Length; i++)
			{
				DirectoryInfo folder = new DirectoryInfo(dirPathArray[i]);
				pngDic.Add(folder.Name, new List<string>());

				FileInfo[] fileInfos = folder.GetFiles("*.png");

				for (int j = 0; j < fileInfos.Length; j++)
				{
					string fileName = string.Copy(fileInfos[j].Name).Replace(".png", "");
					pngDic[folder.Name].Add($"{rootName}\\{folder.Name}\\{fileName}");
				}
			}
			string json = JsonConvert.SerializeObject(pngDic);
			File.WriteAllText($"{Application.streamingAssetsPath}\\{jsonName}.json", json);
		}

		#endregion

		#region Enum
		public static string ToChinese(this TitleType titleType)
		{
			switch (titleType)
			{
				case TitleType.SingleChoice:
					return "单选";
				case TitleType.MultipleChoice:
					return "多选";
			}
			return null;
		}
		#endregion
	}
}

