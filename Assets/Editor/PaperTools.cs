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
		//			optionDescriptionList.Add("ѡ�� A ������");
		//			optionDescriptionList.Add("ѡ�� B ������");
		//			optionDescriptionList.Add("ѡ�� C ������");
		//			optionDescriptionList.Add("ѡ�� D ������");
		//			string titleDescription = "��Ŀ" + i + "����";
		//			Option rightOption = Option.A;


		//			TitleData single = new TitleData(titleNumber, optionDescriptionList, score, titleDescription, rightOption);

		//			dataList.Add(single);
		//		}
		//		else
		//		{
		//			int titleNumber = i;
		//			int score = i;
		//			List<string> optionDescriptionList = new List<string>();
		//			optionDescriptionList.Add("ѡ�� A ������");
		//			optionDescriptionList.Add("ѡ�� B ������");
		//			optionDescriptionList.Add("ѡ�� C ������");
		//			optionDescriptionList.Add("ѡ�� D ������");
		//			string titleDescription = "��Ŀ" + i + "����";
		//			List<Option> rightOptionList = new List<Option>();
		//			rightOptionList.Add(Option.A);
		//			rightOptionList.Add(Option.B);


		//			TitleData single = new TitleData(titleNumber, optionDescriptionList, score, titleDescription, rightOptionList);
		//			dataList.Add(single);
		//		}

		//	}

		//	Paper paper = new Paper(dataList, "����");

		//	string json = JsonConvert.SerializeObject(paper);
		//	if (!File.Exists(ProjectSettings.EXAM_PAPER_JSON))
		//		File.Create(ProjectSettings.EXAM_PAPER_JSON);
		//	File.WriteAllText(ProjectSettings.EXAM_PAPER_JSON, json);
		//}

		//[MenuItem("Tools/String/StringPresistence")]
		//public static void StringPresistence()
		//{
		//	Dictionary<string, string> textDic = new Dictionary<string, string>();
		//	textDic.Add("RingWax", "\t�������õĲ��Ϸ�ΪӲ���������������Ͽ����򵽲�ͬ��״�����ģ����Ը�����Ҫ���й��������Ҫ��̽�ָ�����Թ����ָ������ָ���Ĳ�ͬ�ͺŶ�Ӧ��ͬ����״��ÿ��ʹ��ʱ���Խ�����г���Ҫ�ĺ�ȡ�");
		//	textDic.Add("MeasureTool", "\t��ָ����ʹ�õĲ���������𹤵Ĳ���������ͬ�������ڲ�����ȵĹ������α꿨�ߣ����ڲ�����ָ�ͺŵ��ǽ�ָ����Բ�������������Բ�εĻ��ơ�");
		//	textDic.Add("CuttingTool", "����ʽ��ʼ����֮ǰ����Ҫʹ�����⽫Ӳ�����ģ���ָ�����г��������״���ߴ硣�����ɾ����;⹭��ɣ�������Ϊ�������ͽ����������֡�������������״���в�ͬ��ϸ�ͺţ��ϴֵ��������ʺϾ��нϺ�����顣");
		//	textDic.Add("CarvingTool", "\t�ڵ��������У���Ҫʹ�õ�̹��߽���ϸ�ڵ����졣��Ҫ��̾��µ�������Ҫʹ�õ���ͬ����״����ͬ�ߴ�Ĺ��ߡ�\n\t�������ǵ��ϸ��ʱ�ز����ٵĹ��ߣ�һ�׵������в�ͬ��״�ĵ�ͷ����Ӧ����ͬ��ϸ�ڡ�����Ƶ��û���õ���������������ǰ�Ĺ��߽����з�ͼƬ��Ҫ˵���£�\n\t������Ҳ�ǵ��������в���ȱ�ٵĹ��ߣ������Ը�ʡʱʡ���ش���һЩ���͡�����һЩ�������������Ը�����ͬ��״�Ľ�����ͷ����ת�����������ϵ���ť���Ե���ת�١�\n\t������ͷ�в�ͬ����״�����ε���볣�������Ͷ��������״����������ȣ����в�ͬ��С����ͷ�ֱ�Ӧ���ڵ�̲�ͬ����״��ϸ�ڡ�");
		//	textDic.Add("RepairTool", "\t�����������ڽ�һ����߶���ʱ��Ҫ�õ�����������ȫ������������ֱ��ڲ�ʹ���ʺ���Ҫ�������ֱ������ϡ�ͨ��������������壬���Ե��ں��������¶ȡ�");
		//	textDic.Add("GrindingTool", "\t��ĥ���ĵĳ��ù���Ϊﱵ���ɰֽ��ﱵ���Ϊ����ﱵ�����ﱵ����֡�����ﱵ������ϸ�����ʺ����ĵ����Ҳ�ʺϽ������ϵ���ޡ�\n\t��ʲ��������ϴ֣��ʺϿ������κ��������졣\n\t����ʲ��ﱵ����С�ɣ���״�϶࣬�ʺ�ϸ�ڵ���ޡ�\n\tɰֽ�������������ı����׹⴦��10000Ŀ�����Ŀ��Ϊ�ˡ�");
		//	textDic.Add("CastingTool", "");
		//	textDic.Add("PolishingTool", "\tɰֽ�������������ı����׹⴦��10000Ŀ�����Ŀ��Ϊ�ˡ�");
		//	string json = JsonConvert.SerializeObject(textDic);
		//	if (!File.Exists(ProjectSettings.DEVICES_DESCRIPTION_PAHT))
		//		File.Create(ProjectSettings.DEVICES_DESCRIPTION_PAHT);
		//	File.WriteAllText(ProjectSettings.DEVICES_DESCRIPTION_PAHT, json);
		//}
	}
}
