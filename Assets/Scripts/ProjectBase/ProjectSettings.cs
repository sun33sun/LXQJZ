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

		#region 设备认知
		public static List<string> DEVICES_IMAGE
		{
			get
			{
				return new List<string>() { "DEVICE\\RingWax", "DEVICE\\MeasureTool", "DEVICE\\CuttingTool", "DEVICE\\CarvingTool", "DEVICE\\RepairTool", "DEVICE\\GrindingTool", "DEVICE\\CastingTool", "DEVICE\\PolishingTool" };
			}
		}
		public static string DEVICES_DESCRIPTION_PAHT { get { return Application.streamingAssetsPath + "\\deviceDescripton.json"; } }
		#endregion


		#region 试题
		public static string EXAM_PAPER_JSON { get { return Application.streamingAssetsPath + "\\ExamPaper.json"; } }
		public static string EXAM_MULTIPLECHOICETITLE { get { return "Prefabs\\MultipleChoiceTitle"; } }
		public static string EXAM_SINGLECHOICETITLE { get { return "Prefabs\\SingleChoiceTitle"; } }
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

