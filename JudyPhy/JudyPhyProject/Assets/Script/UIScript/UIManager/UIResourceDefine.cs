using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//==========================================
//
//      *****配表与枚举对应*****
//
//==========================================
public enum WindowID
{
    Invaild    =   0,
    MainMenu   =   1,
    Battle,
    //...

    WindowID_Max,
}

public enum WindowMessage {
    eWindow_Invalid,
    eWindow_BeAttacked,
    eWindow_BattleStart,
    eWindow_CurRoundOver,
}

public class UIResourceDefine {
    private static UIResourceDefine instance;
    public static UIResourceDefine Instance {
        get {
            if (instance == null)
                instance = new UIResourceDefine();
            return instance;
        }
    }

    //读表获取窗口路径（prefab必须放在Resource下）
    public Dictionary<WindowID, string> windowPrefabPath = new Dictionary<WindowID, string>();

    public void AddAllWindowPrefabPath() {
        windowPrefabPath.Add(WindowID.MainMenu, "UIMainMenu");
        windowPrefabPath.Add(WindowID.Battle, "UIBattle");
    }
}
