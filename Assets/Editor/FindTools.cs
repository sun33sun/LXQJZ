using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;

namespace LXQJZ.Editor
{
    public class FindTools
    {
        private static GameObject curSelectedGo = null;
        private const string CANVAS_ENVIROMENT = "Canvas (Environment)"; //双击UGUI的预制体，在Hierarchy窗口,此预制体的根节点会变为这个名字

        [MenuItem("GameObject/FindReference", false, 30)]
        public static void FindReferenced()
        {
            //Resources.FindObjectsOfTypeAll()方法找到的物体不仅包括 Hierarchy窗口，也包括Project窗口里的预制体。接下来会过滤掉预制体
            var objs = Resources.FindObjectsOfTypeAll(typeof(Transform));

            GameObject selectedGo = Selection.activeGameObject;
            if (selectedGo == null)
            {
                Debug.Log("请先选中要查找的物体");
                return;
            }

            curSelectedGo = selectedGo;

            var parentTranList = GetRootTranList(objs);
            bool isFind = false;
            for (int i = 0; i < parentTranList.Count; i++)
            {
                isFind = FindOneReferenced(parentTranList[i], curSelectedGo) || isFind;
            }

            if (!isFind)
            {
                Debug.LogWarning("没能找到引用");
            }
        }

        private static List<Transform> GetRootTranList(Object[] objs)
        {
            List<Transform> parentTranList = new List<Transform>();

            foreach (var o in objs)
            {
                var tran = o as Transform;
                if (tran.parent == null || tran.parent.name.Equals(CANVAS_ENVIROMENT))
                {
                    //过滤掉预制体
                    if (IsInHierarchy(tran.gameObject))
                    {
                        parentTranList.Add(tran);
                    }
                }
            }

            return parentTranList;
        }

        /// <summary>
        /// 判断一个GameObject是否在场景中。 用于过滤掉预制体
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        private static bool IsInHierarchy(GameObject go)
        {
            if (!string.IsNullOrEmpty(go.scene.path))
            {
                return true;
            }
            else if (go.name.Equals(CANVAS_ENVIROMENT))
            {
                return true;
            }

            return false;
        }

        private static bool FindOneReferenced(Transform rootTran, GameObject targetGo)
        {
            //获取这个节点和其子节点的所有组件
            Component[] coms = rootTran.GetComponentsInChildren<Component>();

            bool isFind = false;
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] == null)
                {
                    continue;
                }

                //遍历一个组件的所有字段
                var fileList = coms[i].GetType().GetFields().ToList<FieldInfo>();
                for (int j = 0; j < fileList.Count; j++)
                {
                    var fileInfo = fileList[j];
                    var fileValue = fileInfo.GetValue(coms[i]); //关键代码，获得一个字段的引用对象
                    GameObject fileGo = null;

                    if (fileValue == null)
                    {
                        continue;
                    }

                    //是否是GameObject
                    if (typeof(GameObject) == fileValue.GetType())
                    {
                        fileGo = (fileValue as GameObject)?.gameObject;
                    }
                    //或者是否继承Component组件
                    else if (typeof(Component).IsAssignableFrom(fileValue.GetType()))
                    {
                        var tempComp = (fileValue as Component);
                        if (tempComp != null)
                            fileGo = tempComp.gameObject;
                        // fileGo = (fileValue as Component)?.gameObject; 如果改用此简写，可能会报 UnassignedReferenceException 异常...
                    }

                    if (fileGo != null && fileGo == curSelectedGo)
                    {
                        isFind = true;
                        //在Hierarchy窗口显示查到的物体
                        EditorGUIUtility.PingObject(coms[i]);
                        Debug.Log($"找到引用,引用物体名：{coms[i]} 字段名：{fileInfo.Name}");
                    }
                }
            }

            return isFind;
        }
    }
}

