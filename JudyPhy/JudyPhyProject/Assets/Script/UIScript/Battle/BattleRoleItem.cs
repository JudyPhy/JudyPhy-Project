using UnityEngine;
using System.Collections;

public class BattleRoleItem : MonoBehaviour {

    public UISprite Sprite_Hero;
    public UISprite Sprite_Di;
    public UISprite Sprite_Hp;

    private Move MyMove;
    private GameObject MyEnermy;        //本人为攻击方时，被攻击的物体
    private Vector3 MoveBackTargetPos;  //本人为被攻击方，移动的终点坐标

    private RoleData MyRoleData;

	// Use this for initialization
	void Start () {
	}

    public void InitItem(RoleData data) {
        this.gameObject.SetActive(true);
        this.MyMove = this.gameObject.GetComponent<Move>();
        this.MyRoleData = data;
        this.Sprite_Hero.spriteName = data.iconName;
        this.Sprite_Hero.MakePixelPerfect();
        this.Sprite_Hero.transform.localScale = Vector3.one * 0.5f;
    }

    public void SetAttackTarget(GameObject go) {
        this.MyEnermy = go;
    }

    public void AttackStart() {
        this.MyMove.SetOrigPos(this.gameObject.transform.position);
        this.MyMove.SetTargetPos(this.MyEnermy.transform.position);
        BattleMgr.Instance.CurBattleStatus = BattleStatus.BattleIn;       //开始播放动画时，状态切换为战斗中
        this.MyMove.HideAndMove();
    }

    public void SetBeAttackTargetPos(Vector3 pos) {
        this.MoveBackTargetPos = pos;
    }

    public void BeAttackedStart() {
        this.MyMove.SetOrigPos(this.gameObject.transform.position);
        this.MyMove.SetTargetPos(this.MoveBackTargetPos);
        this.MyMove.MoveBackTo();
        ShowDamageValue();      //播放动画时进行计算
    }

    private void ShowDamageValue() {
        //for (int i=0;i<BattleMgr.Instance.BattleEnermyList.Count;i++){
        //    Debug.LogError("ID :" + BattleMgr.Instance.BattleEnermyList[i].baseID + "  hp:" + BattleMgr.Instance.BattleEnermyList[i].hp);
        //}

        float value = (float)BattleMgr.Instance.BeAttackedRoleData.hp / (float)BattleMgr.Instance.BeAttackedRoleData.maxHp;
        Sprite_Hp.fillAmount = value;
    }

    public RoleData GetMyRoleData() {
        return this.MyRoleData;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
