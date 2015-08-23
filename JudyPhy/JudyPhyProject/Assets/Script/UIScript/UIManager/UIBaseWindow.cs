using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//=========================================================
//
//                     窗口继承基类
//
//=========================================================

public class UIBaseWindow : MonoBehaviour {
    
    //窗口ID
    [HideInInspector]
    public WindowID windowID = WindowID.Invaild;

    //指向上一级界面ID
    [HideInInspector]
    public WindowID preWindowID = WindowID.Invaild;

    ////当前窗口打开方式
    //[HideInInspector]
    //public UIWindowShowMode showMode = UIWindowShowMode.Invaild;

    //当前窗口对应的导航链
    [HideInInspector]
    public List<GameObject> BackSeqWindows = new List<GameObject>();

    //窗口关闭时计时，超过指定时长销毁
    [HideInInspector]
    public float DisableTime = 0;

    ////Return处理逻辑
    //public delegate bool BoolDelegate();
    //private event BoolDelegate returnPreLogic = null;

    //窗口层级
    private int minDepth = 1;
    public int MinDepth {
        get { return minDepth; }
        set { minDepth = value; }
    }

    //该窗口Panel列表（调整层级用）
    private List<UIPanel> panelList = new List<UIPanel>();

    public WindowID GetID {
        get {
            if (this.windowID == WindowID.Invaild)
                Debug.LogError("window id is " + WindowID.Invaild);
            return windowID;
        }
        private set { windowID = value; }
    }

    protected virtual void Awake() {
        InitWindowOnAwake();
    }

    /// <summary>
    /// 在Awake中调用，初始化界面（给界面元素赋值操作）
    /// </summary>
    public virtual void InitWindowOnAwake() {
        //尽量不在这里添加方法，界面卡帧
    }

    void OnEnable() {
        OnInitWindow();
    }

    //每次打开窗口时执行
    public virtual void OnInitWindow() {
    }

    /// <summary>
    /// 重置窗口
    /// </summary>
    public virtual void ReshowWindow() {
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    //每次关闭窗口时执行
    public void OnDisable() {
        OnClose();
    }

    public virtual void OnClose() {
    }

    public virtual void OnDestroy() { 
    }

    public virtual void SendWindowMsg(WindowMessage msg, params object[] args) {
    }
}
