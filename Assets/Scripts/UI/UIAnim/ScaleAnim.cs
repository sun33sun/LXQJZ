using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace LXQJZ.UI
{
	public class ScaleAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] Vector2 maxScale = new Vector2(1.1f, 1.1f);
		Vector2 normalScale = new Vector2(1, 1);
		public void OnPointerEnter(PointerEventData eventData)
		{
			transform.DOKill();
			(transform as RectTransform).DOKill();
			transform.DOScale(maxScale, 0.5f);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			transform.DOKill();
			(transform as RectTransform).DOKill();
			transform.DOScale(normalScale, 0.5f);
		}

		private void OnDisable()
		{
			transform.DOKill();
			(transform as RectTransform).DOKill();
			transform.localScale = normalScale;
		}
	}
}
