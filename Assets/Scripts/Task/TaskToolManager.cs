using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ
{
	public class TaskToolManager : SingletonMono<TaskToolManager>
	{
		Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();

		protected override void Awake()
		{
			base.Awake();
			Transform[] transArray = transform.GetComponentsInChildren<Transform>();
			for (int i = 0; i < transArray.Length; i++)
			{
				objDic.Add(transArray[i].name, transArray[i].gameObject);
			}
		}

		public virtual GameObject GetObj(string objName)
		{
			GameObject needObj = null;
			objDic.TryGetValue(objName, out needObj);
			return needObj;
		}
	}
}
