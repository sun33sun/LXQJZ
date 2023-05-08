using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LXQJZ
{
	public class TogScaleAnim : MonoBehaviour
	{
		Toggle tog;
		RectTransform togRect;
		//选中前
		Vector2 originSize;
		//选中后
		Vector2 selectedSize = new Vector2(240,80);
		[Header("文字")]
		[SerializeField] Text txtLabel;
		RectTransform txtRect;
		//选中前
		int originFrontSize;
		Vector2 originTxtPos;
		Vector2 originTxtSize;
		//选中后
		int selectedFrontSize = 30;
		Vector2 selectedTxtPos = new Vector2(135, -37);
		Vector2 selectedTxtSize = new Vector2(90, 40);

		private void Awake()
		{
			tog = GetComponent<Toggle>();
			tog.onValueChanged.AddListener(OnIsOn);
			togRect = (transform as RectTransform);
			originSize = togRect.sizeDelta;

			txtRect = txtLabel.transform as RectTransform;
			originTxtPos = txtRect.localPosition;
			originTxtSize = txtRect.sizeDelta;
			originFrontSize = txtLabel.fontSize;
		}

		void OnIsOn(bool isOn)
		{
			if (isOn)
			{
				togRect.sizeDelta = selectedSize;
				txtLabel.fontSize = selectedFrontSize;
				txtRect.sizeDelta = selectedTxtSize;
				txtRect.localPosition = selectedTxtPos;
			}
			else
			{
				togRect.sizeDelta = originSize;
				txtLabel.fontSize = originFrontSize;
				txtRect.sizeDelta = originTxtSize;
				txtRect.localPosition = originTxtPos;
			}
		}
	}
}
