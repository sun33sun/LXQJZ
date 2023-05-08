using System.Collections;
using UnityEngine;

namespace LXQJZ
{
    // 实现WaitForEndOfAnim迭代器
    public class WaitForEndOfAnimr : IEnumerator
    {
        AnimatorStateInfo m_animState;
        string fullName;

        public WaitForEndOfAnimr(AnimatorStateInfo animState, string clipName)
        {
            m_animState = animState;
            fullName = clipName;
        }
        public object Current
        {
            get
            {
                return null;
            }
        }
        public bool MoveNext()
        {
            Debug.Log(fullName + " " + m_animState.IsName(fullName) + " " + m_animState.normalizedTime);
            return m_animState.IsName(fullName) && m_animState.normalizedTime < 1f;
        }
        public void Reset()
        {
        }
    }
}