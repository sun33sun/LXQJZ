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
        private const string CANVAS_ENVIROMENT = "Canvas (Environment)"; //˫��UGUI��Ԥ���壬��Hierarchy����,��Ԥ����ĸ��ڵ���Ϊ�������

        [MenuItem("GameObject/FindReference", false, 30)]
        public static void FindReferenced()
        {
            //Resources.FindObjectsOfTypeAll()�����ҵ������岻������ Hierarchy���ڣ�Ҳ����Project�������Ԥ���塣����������˵�Ԥ����
            var objs = Resources.FindObjectsOfTypeAll(typeof(Transform));

            GameObject selectedGo = Selection.activeGameObject;
            if (selectedGo == null)
            {
                Debug.Log("����ѡ��Ҫ���ҵ�����");
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
                Debug.LogWarning("û���ҵ�����");
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
                    //���˵�Ԥ����
                    if (IsInHierarchy(tran.gameObject))
                    {
                        parentTranList.Add(tran);
                    }
                }
            }

            return parentTranList;
        }

        /// <summary>
        /// �ж�һ��GameObject�Ƿ��ڳ����С� ���ڹ��˵�Ԥ����
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
            //��ȡ����ڵ�����ӽڵ���������
            Component[] coms = rootTran.GetComponentsInChildren<Component>();

            bool isFind = false;
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] == null)
                {
                    continue;
                }

                //����һ������������ֶ�
                var fileList = coms[i].GetType().GetFields().ToList<FieldInfo>();
                for (int j = 0; j < fileList.Count; j++)
                {
                    var fileInfo = fileList[j];
                    var fileValue = fileInfo.GetValue(coms[i]); //�ؼ����룬���һ���ֶε����ö���
                    GameObject fileGo = null;

                    if (fileValue == null)
                    {
                        continue;
                    }

                    //�Ƿ���GameObject
                    if (typeof(GameObject) == fileValue.GetType())
                    {
                        fileGo = (fileValue as GameObject)?.gameObject;
                    }
                    //�����Ƿ�̳�Component���
                    else if (typeof(Component).IsAssignableFrom(fileValue.GetType()))
                    {
                        var tempComp = (fileValue as Component);
                        if (tempComp != null)
                            fileGo = tempComp.gameObject;
                        // fileGo = (fileValue as Component)?.gameObject; ������ô˼�д�����ܻᱨ UnassignedReferenceException �쳣...
                    }

                    if (fileGo != null && fileGo == curSelectedGo)
                    {
                        isFind = true;
                        //��Hierarchy������ʾ�鵽������
                        EditorGUIUtility.PingObject(coms[i]);
                        Debug.Log($"�ҵ�����,������������{coms[i]} �ֶ�����{fileInfo.Name}");
                    }
                }
            }

            return isFind;
        }
    }
}

