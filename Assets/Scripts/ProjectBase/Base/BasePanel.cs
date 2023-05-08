using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ
{
	public class BasePanel<T> : MonoBehaviour where T : BasePanel<T>
	{
		private static T instance = null;
		public static T Instance { get { return instance; } }

		protected virtual void Awake()
		{
			if (instance != null)
				Destroy(gameObject);
			else
				instance = (T)this;
		}
		protected virtual void Start()
		{
			InitState();
		}
		protected virtual void OnDisable()
		{
			InitState();
		}

		protected virtual void InitState()
		{

		}

		public virtual void Show()
		{
			gameObject.SetActive(true);
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
		}

		public IEnumerator HideAsync(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			Hide();
		}

	}
}

