using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LXQJZ
{
    /// <summary>
    /// 1.Input类
    /// 2.事件中心模块
    /// 3.公共Mono模块的使用
    /// </summary>
    public class InputMgr : BaseManager<InputMgr>
    {

		private bool isStartUpdate = false;
		private bool isStartFixedUpdate = false;

        /// <summary>
        /// 构造函数中 添加Updata监听
        /// </summary>
        public InputMgr()
        {
			MonoMgr.GetInstance().AddUpdateListener(MyUpdate);
			MonoMgr.GetInstance().AddFixedUpdateListener(MyFixedUpdate);
		}

        /// <summary>
        /// 是否开启或关闭 我的输入检测
        /// </summary>
        public void StartOrEndCheck(bool isOpen)
        {
			isStartUpdate = isOpen;
			isStartFixedUpdate = isOpen;
        }

        /// <summary>
        /// 用来检测按键抬起按下 分发事件的
        /// </summary>
        /// <param name="key"></param>
        private void CheckKeyCode(KeyCode key)
        {
            //事件中心模块 分发按下抬起事件
            if (Input.GetKeyDown(key))
                EventCenter.GetInstance().EventTrigger(key + "按下");
            if(Input.GetKey(key))
                EventCenter.GetInstance().EventTrigger(key + "保持");
            //事件中心模块 分发按下抬起事件
            if (Input.GetKeyUp(key))
                EventCenter.GetInstance().EventTrigger(key + "抬起");
        }

		private void MyUpdate()
		{
			//没有开启输入检测 就不去检测 直接return
			if (!isStartUpdate)
				return;

            CheckKeyCode(KeyCode.W);
            CheckKeyCode(KeyCode.S);
            CheckKeyCode(KeyCode.A);
            CheckKeyCode(KeyCode.D);

            if (Input.GetMouseButtonDown(1))
                EventCenter.GetInstance().EventTrigger("鼠标右键按下");
            if (Input.GetMouseButtonUp(1))
                EventCenter.GetInstance().EventTrigger("鼠标右键抬起");
        
            EventCenter.GetInstance().EventTrigger<Vector2>("鼠标滑动", new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
        }

        private void MyFixedUpdate()
		{
            if (!isStartFixedUpdate)
                return;

            CheckKeyCode(KeyCode.Space);
            CheckKeyCode(KeyCode.LeftControl);
        }

    }
}

