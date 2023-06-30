using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace LXQJZ
{
	public class ScalePathAnim : MonoBehaviour
	{
		Vector3 showPos;
		Vector3 hidePos;
		public bool isShow = true;

		private void Awake()
		{
			showPos = transform.localPosition;
		}
		public void DoHideAnim(Vector3 pos)
		{
			transform.DOScale(new Vector3(0, 0, 1), 0.5f);
			transform.DOLocalMove(pos, 0.5f);
			isShow = false; 
		}

		public void DoHideAnimImmediately(Vector3 pos)
		{
			transform.localScale = new Vector3(0, 0, 1);
			transform.position = pos;
			isShow = false;
		}

		public void DoShowAnim()
		{
			transform.DOScale(new Vector3(1, 1, 1), 0.5f);
			transform.DOLocalMove(showPos, 0.5f);
			isShow = true;
		}
	}
}
