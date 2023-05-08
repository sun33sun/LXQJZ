using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.UI
{
	public class MultipleToggle : MonoBehaviour
	{
		List<Toggle> togList;
		[SerializeField] int selectCount = 1;
		List<int> selectedList = new List<int>();

		public Action<List<int>> onValueGroupChanged;
		
		private void Start()
		{
			InitListener();
		}
		void InitListener()
		{
			togList = transform.GetComponentsInChildren<Toggle>().ToList();
			if (togList == null)
				return;
			for (int i = 0; i < togList.Count; i++)
			{
				int index = i;
				togList[i].onValueChanged.AddListener((isOn) => { OnValueChanged(isOn, index); });
			}
		}
		void OnValueChanged(bool isOn, int index)
		{
			if (isOn)
			{
				selectedList.Add(index);
				if (selectedList.Count > selectCount)
				{
					int j = selectedList[0];
					selectedList.RemoveAt(0);
					togList[j].isOn = false;
				}
				else
				{
					onValueGroupChanged?.Invoke(selectedList);
				}
			}
			else
			{
				if (selectedList.Contains(index))
					selectedList.Remove(index);
				onValueGroupChanged?.Invoke(selectedList);
			}
		}
	}
}
