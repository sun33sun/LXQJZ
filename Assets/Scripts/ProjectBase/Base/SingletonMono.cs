using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LXQJZ
{
    //C#中 泛型知识点
    //设计模式 单例模式的知识点
    //继承了 MonoBehaviour 的 单例模式对象 需要我们自己保证它的位移性
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance;

        public static T Instance { get { return instance; } }

        public bool needDestroy = false;

        protected virtual void Awake()
        {
            if (instance != null)
			{
                Destroy(gameObject);
			}
			else
			{
                instance = this as T;
                if(!needDestroy)
                    DontDestroyOnLoadManager.Add(gameObject);
            }
        }
        protected virtual void OnDestory()
        {
            if (instance == this)
                instance = null;
        }
    }
}

