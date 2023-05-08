using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ
{
	public class ModelManager : SingletonMono<ModelManager>
	{
		Dictionary<string,GameObject> objDic;
		protected override void Awake()
		{
			base.Awake();
			Init();
		}
		public void Init()
		{
			objDic = new Dictionary<string, GameObject>();
			foreach (var item in transform.GetComponentsInChildren<Transform>())
			{
				objDic.Add(item.name, item.gameObject);
			}
		}
		public GameObject Get(string objName)
		{
			GameObject needObj = null;
			objDic.TryGetValue(objName,out needObj);
			return needObj;
		}
	}
}
