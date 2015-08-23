using UnityEngine;
using System.Collections;

public class UIMainMenu : UIBaseWindow {

    public GameObject Btn_Battle;

	// Use this for initialization
	void Start () {
        UIEventListener.Get(Btn_Battle).onClick += OnClickBattle;
	}

    private void OnClickBattle(GameObject go) {
        UIManager.Instance.ShowWindow(WindowID.Battle);
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
