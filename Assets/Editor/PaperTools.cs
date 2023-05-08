using LXQJZ.Exam;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LXQJZ.Editor
{
	public class PaperTools
	{
		//[MenuItem("Tools/Paper/TestPaperDataPresistence")]
		//public static void TestPaperDataPresistence()
		//{
		//	List<TitleData> dataList = new List<TitleData>();
		//	for (int i = 0; i < 9; i++)
		//	{
		//		if(i % 2 == 1)
		//		{
		//			int titleNumber = i;
		//			int score = i;
		//			List<string> optionDescriptionList = new List<string>();
		//			optionDescriptionList.Add("选项 A 的描述");
		//			optionDescriptionList.Add("选项 B 的描述");
		//			optionDescriptionList.Add("选项 C 的描述");
		//			optionDescriptionList.Add("选项 D 的描述");
		//			string titleDescription = "题目" + i + "描述";
		//			Option rightOption = Option.A;


		//			TitleData single = new TitleData(titleNumber, optionDescriptionList, score, titleDescription, rightOption);

		//			dataList.Add(single);
		//		}
		//		else
		//		{
		//			int titleNumber = i;
		//			int score = i;
		//			List<string> optionDescriptionList = new List<string>();
		//			optionDescriptionList.Add("选项 A 的描述");
		//			optionDescriptionList.Add("选项 B 的描述");
		//			optionDescriptionList.Add("选项 C 的描述");
		//			optionDescriptionList.Add("选项 D 的描述");
		//			string titleDescription = "题目" + i + "描述";
		//			List<Option> rightOptionList = new List<Option>();
		//			rightOptionList.Add(Option.A);
		//			rightOptionList.Add(Option.B);


		//			TitleData single = new TitleData(titleNumber, optionDescriptionList, score, titleDescription, rightOptionList);
		//			dataList.Add(single);
		//		}

		//	}

		//	Paper paper = new Paper(dataList, "测试");

		//	string json = JsonConvert.SerializeObject(paper);
		//	if (!File.Exists(ProjectSettings.EXAM_PAPER_JSON))
		//		File.Create(ProjectSettings.EXAM_PAPER_JSON);
		//	File.WriteAllText(ProjectSettings.EXAM_PAPER_JSON, json);
		//}

		//[MenuItem("Tools/String/StringPresistence")]
		//public static void StringPresistence()
		//{
		//	Dictionary<string, string> textDic = new Dictionary<string, string>();
		//	textDic.Add("RingWax", "\t雕蜡常用的材料分为硬蜡和软蜡，市面上可以买到不同形状的蜡材，可以根据需要进行购买。如果需要雕刻戒指，可以购买戒指蜡，戒指蜡的不同型号对应不同的形状，每次使用时可以将其据切成需要的厚度。");
		//	textDic.Add("MeasureTool", "\t戒指雕蜡使用的测量工具与金工的测量工具相同，常用于测量厚度的工具是游标卡尺；用于测量戒指型号的是戒指棒。圆规机剪，常用于圆形的绘制。");
		//	textDic.Add("CuttingTool", "在正式开始雕蜡之前，需要使用蜡锯将硬蜡蜡材（戒指蜡）切成所需的形状、尺寸。蜡锯由锯条和锯弓组成，锯条分为蜡锯条和金属锯条两种。蜡锯条呈螺旋状，有不同粗细型号，较粗的蜡锯条适合锯切较厚的蜡块。");
		//	textDic.Add("CarvingTool", "\t在雕蜡过程中，需要使用雕刻工具进行细节的塑造。想要雕刻精致的蜡件，要使用到不同的形状、不同尺寸的工具。\n\t雕蜡刀是雕刻细节时必不可少的工具，一套雕蜡刀有不同形状的刀头，对应处理不同的细节。（视频中没有用到雕蜡刀，可以在前文工具介绍中放图片简要说明下）\n\t雕蜡机也是雕蜡过程中不可缺少的工具，它可以更省时省力地处理一些造型、制作一些纹理。雕蜡机可以更换不同形状的金属钻头，旋转雕蜡机机身上的旋钮可以调剂转速。\n\t雕蜡钻头有不同的形状，球形雕刻针常被用来掏洞、雕刻形状、制作纹理等，其中不同大小的钻头分别应用于雕刻不同的形状和细节。");
		//	textDic.Add("RepairTool", "\t将两个蜡件融接一起或者堆蜡时需要用到焊蜡机，安全起见，焊蜡机手柄在不使用适合需要放置在手柄架子上。通过焊蜡机机身面板，可以调节焊蜡机的温度。");
		//	textDic.Add("GrindingTool", "\t打磨蜡材的常用工具为锉刀和砂纸。锉刀分为金属锉刀和蜡锉刀两种。金属锉刀纹理较细，既适合蜡材的锉修也适合金属材料的锉修。\n\t蜡什锦刀纹理较粗，适合快速塑形和纹理塑造。\n\t金属什锦锉刀相对小巧，形状较多，适合细节的锉修。\n\t砂纸棒用于银饰最后的表面抛光处理，10000目或更高目数为宜。");
		//	textDic.Add("CastingTool", "");
		//	textDic.Add("PolishingTool", "\t砂纸棒用于银饰最后的表面抛光处理，10000目或更高目数为宜。");
		//	string json = JsonConvert.SerializeObject(textDic);
		//	if (!File.Exists(ProjectSettings.DEVICES_DESCRIPTION_PAHT))
		//		File.Create(ProjectSettings.DEVICES_DESCRIPTION_PAHT);
		//	File.WriteAllText(ProjectSettings.DEVICES_DESCRIPTION_PAHT, json);
		//}
	}
}
