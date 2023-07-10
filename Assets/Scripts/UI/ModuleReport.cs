using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ
{
	public class ModuleReportData
	{
		public int seq;
		public string title;
		public DateTime startTime = default(DateTime);
		public DateTime endTime = default(DateTime);
		public TimeSpan expectTime = new TimeSpan(0, 5, 0);
		public int maxScore = 0;
		public int score = 0;
		public int repeatCount = 1;
		public string evaluation = null;
		public string scoringModel = null;
		public string remarks = null;
		public string ext_data = null;
	}
	public class ModuleReport : MonoBehaviour
	{
		[SerializeField] Text[] txts;
		public ModuleReportData mData = new ModuleReportData();

		public void InitData(ModuleReportData newData)
		{
			mData = newData;
			txts[0].text = mData.title;
			txts[1].text = "开始：" + mData.startTime.ToString("M/d-H:m");
			txts[2].text = "结束：" + mData.endTime.ToString("M/d-H:m");
			txts[3].text = "用时：" + (mData.endTime - mData.startTime).ToString(@"mm\:ss");
			txts[4].text = "得分：" + mData.score.ToString();
		}
	}
}
