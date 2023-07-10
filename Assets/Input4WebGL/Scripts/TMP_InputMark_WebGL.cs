using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class TMP_InputMark_WebGL : MonoBehaviour {
#if UNITY_WEBGL  //只在WebGl下生效
    public TMP_InputField inputField = null;
    void Start () {
        inputField = GetComponent<TMP_InputField>();
        //添加unity输入框回调
        inputField.onValueChanged.AddListener(OnValueChanged);
        //添加获得焦点回调
        EventTrigger trigger = inputField.gameObject.GetComponent<EventTrigger>();
        if (null == trigger)
            trigger = inputField.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry e = new EventTrigger.Entry();
        e.eventID = EventTriggerType.PointerDown;
        e.callback.AddListener((data) => { OnFocus((PointerEventData)data); });
        trigger.triggers.Add(e);
    }
    #region ugui回调
    private void OnValueChanged(string arg0)
    {
        //暂时没用
    }
    private void OnFocus(PointerEventData pointerEventData)
    {
#if !UNITY_EDITOR
        WebGLInput.captureAllKeyboardInput = false;
        Input4WebGL.InputShow(gameObject.name, inputField.text);
#endif
}
#endregion

        #region WebGL回调
    public void OnInputText(string text)
    {
        inputField.text = text;
        inputField.caretPosition = inputField.text.Length;
    }
    public void OnInputEnd()
    {
        WebGLInput.captureAllKeyboardInput = true;
    }
    
        #endregion
#endif
    }
