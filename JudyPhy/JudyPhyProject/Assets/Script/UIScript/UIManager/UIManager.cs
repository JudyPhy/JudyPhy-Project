using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//====================================================
//      
//                      单例
//
//====================================================


public class UIManager : MonoBehaviour {

    private static UIManager instance;
    public static UIManager Instance {
        get {
            return instance;
        }
    }

    public GameObject UIRoot;
    public GameObject UIPanelRoot; 

    //当前窗口
    public WindowID CurWindow = WindowID.Invaild;

    //所有窗口
    private static Dictionary<WindowID, UIBaseWindow> AllWindowScript = new Dictionary<WindowID, UIBaseWindow>();

    private float TIMERANGE = 5f;

    void Awake() {
        instance = this;
        Debug.Log("UIManager is call awake.");        
    }

    // Use this for initialization
    void Start() {
        UIResourceDefine.Instance.AddAllWindowPrefabPath();
        ShowWindow(WindowID.MainMenu);
    }

    //显示窗口（关闭其余所有界面，显示新界面）
    public void ShowWindow(WindowID id) {
        Debug.Log("ShowWindow [" + id + "]");
        HideAllWindow();
        if (AllWindowScript.ContainsKey(id) && AllWindowScript[id].gameObject != null) {
            AllWindowScript[id].gameObject.SetActive(true);
        } else {
            string prefabPath = UIResourceDefine.Instance.windowPrefabPath[id];
            GameObject window = NGUITools.AddChild(UIPanelRoot, prefabPath);
            UIBaseWindow script = window.GetComponent<UIBaseWindow>();
            script.windowID = id;
            script.preWindowID = CurWindow;
            if (AllWindowScript.ContainsKey(id))
                AllWindowScript[id] = script;
            else
                AllWindowScript.Add(id, script);
        }
        CurWindow = id; 
    }

    //返回主界面
    public void ReturnToMainWindow() {
        bool hasMainWindow = false;
        foreach (WindowID id in AllWindowScript.Keys) {
            if (id != WindowID.MainMenu && AllWindowScript[id].gameObject != null && AllWindowScript[id].gameObject.activeSelf) {
                CloseWindow(id);
            } else if (id == WindowID.MainMenu && AllWindowScript[id].gameObject != null) {
                hasMainWindow = true;
                ShowWindow(WindowID.MainMenu);
            }
        }
        if (!hasMainWindow) {
            ShowWindow(WindowID.MainMenu);
        }
    }

    public void HideAllWindow() {
        foreach (WindowID id in AllWindowScript.Keys) {
            if (AllWindowScript[id].gameObject != null && AllWindowScript[id].gameObject.activeSelf) {
                CloseWindow(id);
            }
        }
    }

    public void CloseWindow(WindowID id) {
        Debug.Log("CloseWindow [" + id + "]");
        if (!AllWindowScript.ContainsKey(id)) {
            Debug.LogError("窗口 " + id + " 不存在");
            return;
        }
        if (!AllWindowScript[id].gameObject) {
            Debug.LogError("窗口  " + id + "  物体为空");
            return;
        }
        AllWindowScript[id].gameObject.SetActive(false);
        AllWindowScript[id].DisableTime = Time.time;
    }

    public void SendWindowMsg(WindowID targetWindow, WindowMessage msg, params object[] args) {
        if (!AllWindowScript.ContainsKey(targetWindow)) {
            return;
        }
        if (!AllWindowScript[targetWindow]) {
            return;
        }
        AllWindowScript[targetWindow].SendWindowMsg(msg, args);
    }
    
    // Update is called once per frame
    void Update() {
        foreach (WindowID id in AllWindowScript.Keys) {
            float time = Time.time - AllWindowScript[id].DisableTime;
            if (AllWindowScript[id].DisableTime != 0 && time > TIMERANGE && AllWindowScript[id] != null && AllWindowScript[id].gameObject != null) {
                DestroyImmediate(AllWindowScript[id].gameObject);
            }
        }
    }
}
