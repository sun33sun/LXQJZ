using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ
{
	public class ModuleReportData
	{
		public string moduleName;
		public DateTime startTime;
		public DateTime endTime;
		public int moduleScore;
	}
	public class ModuleReport : MonoBehaviour
	{
		[SerializeField] Text[] txts;
		public ModuleReportData mData;

		public void InitData(ModuleReportData newData)
		{
			mData = newData;
			txts[0].text = mData.moduleName;
			txts[1].text = mData.startTime.ToString("u");
			txts[2].text = mData.endTime.ToString("u");
			txts[3].text = "ʵ����ʱ��" + (mData.endTime - mData.startTime).ToString("mm") + "min";
			txts[4].text = "ʵ��÷֣�" + mData.moduleScore.ToString();
		}

		private void OnDisable()
		{
			Debug.Log(name + "��Disable");
		}

		private void OnDestroy()
		{
			Debug.Log(name + " ������");
			Debug.Log("��鿴");
		}
	}
}
