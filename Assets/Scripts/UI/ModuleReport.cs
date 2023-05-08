using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ
{
	public class ModuleReport : SingletonMono<ModuleReport>
	{
		[SerializeField] Text[] txts;

		public void InitData(string moduleName,DateTime moduleStartTime, DateTime moduleEndTime, int moduleScore)
		{
			txts[0].text = moduleName;
			txts[1].text = moduleStartTime.ToString("u");
			txts[2].text = moduleEndTime.ToString("u");
			txts[3].text = "ʵ����ʱ��" + (moduleEndTime - moduleStartTime).ToString("mm") + "min";
			txts[4].text = "ʵ��÷֣�" + moduleScore.ToString();
		}
	}
}
