using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ
{
	public class DontDestroyOnLoadManager : MonoBehaviour
	{
		#region µ¥Àý
		static DontDestroyOnLoadManager instance = null;
		public static DontDestroyOnLoadManager Instance { get { return instance; } }
		#endregion

		public static List<GameObject> dontDestroyList = new List<GameObject>();
		
		void Awake()
		{
			DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(this);
			if (instance == null)
				instance = this;
			else
				Destroy(this);
			for (int i = 0; i < dontDestroyList.Count; i++)
				DontDestroyOnLoad(dontDestroyList[i]);
		}

		private void OnDestroy()
		{
			if (instance == this)
				instance = null;
		}

		public static void Add(GameObject newObj)
		{
			dontDestroyList.Add(newObj);
			DontDestroyOnLoad(newObj);
		}
	}
}
