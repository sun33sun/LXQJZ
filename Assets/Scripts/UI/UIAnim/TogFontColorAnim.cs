using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ
{
	public class TogFontColorAnim : MonoBehaviour
	{
		List<Toggle> togList = new List<Toggle>();
		List<Text> txtList = new List<Text>();
		Toggle nowToggle;
		private void Awake()
		{
			togList = transform.GetComponentsInChildren<Toggle>().ToList();
			for (int i = 0; i < togList.Count; i++)
			{
				txtList.Add(togList[i].transform.GetChild(0).GetComponent<Text>());
				int index = i;
				txtList[i].color = Color.gray;
				togList[i].onValueChanged.AddListener((isOn)=>
				{
					if (isOn)
						txtList[index].color = Color.white;
					else
						txtList[index].color = Color.gray;
				});
			}
			txtList[0].color = Color.gray;
		}
	}
}
