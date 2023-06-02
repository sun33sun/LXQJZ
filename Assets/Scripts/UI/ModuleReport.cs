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
		public DateTime startTime;
		public DateTime endTime;
		public TimeSpan expectTime;
		public int maxScore;
		public int score;
		public int repeatCount;
		public string evaluation;
		public string scoringModel;
		public string remarks;
		public string ext_data;
	}
	public class ModuleReport : MonoBehaviour
	{
		[SerializeField] Text[] txts;
		public ModuleReportData mData;

		public void InitData(ModuleReportData newData)
		{
			mData = newData;
			txts[0].text = mData.title;
			txts[1].text = mData.startTime.ToString("u");
			txts[2].text = mData.endTime.ToString("u");
			txts[3].text = "实验用时：" + (mData.endTime - mData.startTime).ToString("mm") + "min";
			txts[4].text = "实验得分：" + mData.score.ToString();
		}

		private void OnDisable()
		{
			Debug.Log(name + "被Disable");
		}

		private void OnDestroy()
		{
			Debug.Log(name + " 被销毁");
			Debug.Log("请查看");
		}
	}
}
