using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class UIBattle : UIBaseWindow {

    public GameObject HeroNode;
    public GameObject EnermyNode;

    public GameObject SpeedBar;

    private List<GameObject> RoleItems = new List<GameObject>();

    private List<GameObject> SpeedRoleItems = new List<GameObject>();

    private GameObject AttackRoleGo;
    private GameObject BeAttackedRoleGo;
    
    public override void InitWindowOnAwake() {
        base.InitWindowOnAwake();
    }

	// Use this for initialization
	void Start () {
	}

    public override void OnInitWindow() {
        base.OnInitWindow();
        PrepareBattle();
        //开始跑速
        BattleMgr.Instance.bStartRun = true;
    }

    //战斗前准备
    private void PrepareBattle() {
        //测试用
        BattleMgr.Instance.MyFormationType = FormationType.Type1;
        BattleMgr.Instance.EnermyFormationType = FormationType.Type1;
        BattleMgr.Instance.InitHeroData();
        BattleMgr.Instance.InitEnermyData();

        UpdateTeamUI();
        InitSpeedBar();
    }

    private void UpdateTeamUI() {        
        PrepareRoleItems();
        SetRoleItemPos(BattleMgr.Instance.MyFormationType, RoleType.Hero);
        SetRoleItemPos(BattleMgr.Instance.EnermyFormationType, RoleType.Npc);
    }

    private void PrepareRoleItems() {
        int heroCount = GetRoleCountByFormationType(BattleMgr.Instance.MyFormationType);
        int enermyCount = GetRoleCountByFormationType(BattleMgr.Instance.EnermyFormationType);
        while (RoleItems.Count < (heroCount + enermyCount)) {
            AddNewRoleItem();
        }
        int n = 1;
        while (RoleItems.Count - n + 1 > 3) {
            RoleItems[RoleItems.Count - n].SetActive(false);
            n++;
        }
        int i = 0;
        for (; i < heroCount; i++) {
            RoleItems[i].transform.parent = HeroNode.transform;
        }
        for (; i < heroCount + enermyCount; i++) {
            RoleItems[i].transform.parent = EnermyNode.transform;
        }
        for (int j = 0; j < BattleMgr.Instance.BattleRoleList.Count; j++) {
            BattleRoleItem script = RoleItems[j].GetComponent<BattleRoleItem>();
            script.InitItem(BattleMgr.Instance.BattleRoleList[j]);
        }
    }

    private int GetRoleCountByFormationType(FormationType type) {
        switch (type) {
            case FormationType.Type1:
                return 3;
            default:
                return 0;
        }
    }

    private void AddNewRoleItem() {
        GameObject itemGo = NGUITools.AddChild(HeroNode, "BattleRoleItem");
        RoleItems.Add(itemGo);
    }

    //根据阵型摆放角色
    private void SetRoleItemPos(FormationType type, RoleType roletype) {
        switch (type) {
            case FormationType.Type1:
                if (roletype == RoleType.Hero) {
                    Vector3 startPos = new Vector3(-140, -700, 0);
                    float offset = 140;
                    int n = 0;
                    for (int i = 0; i < RoleItems.Count; i++) {
                        if (RoleItems[i].GetComponent<BattleRoleItem>().GetMyRoleData().roleType == roletype) {
                            RoleItems[i].transform.localPosition = new Vector3(startPos.x + offset * n++, startPos.y, 0);
                        }
                    }
                } else if (roletype == RoleType.Npc) {
                    Vector3 startPos = new Vector3(-140, -380, 0);
                    float offset = 140;
                    int n = 0;
                    for (int i = 0; i < RoleItems.Count; i++) {
                        if (RoleItems[i].GetComponent<BattleRoleItem>().GetMyRoleData().roleType == roletype) {
                            RoleItems[i].transform.localPosition = new Vector3(startPos.x + offset * n++, startPos.y, 0);
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    //初始化速度条UI
    private void InitSpeedBar() {
        int allCount = BattleMgr.Instance.BattleRoleList.Count;
        while (SpeedRoleItems.Count < allCount) {
            AddNewRoleSpeedItem();
        }
        int n = 1;
        while (SpeedRoleItems.Count - n + 1 > allCount) {
            SpeedRoleItems[SpeedRoleItems.Count - n].SetActive(false);
            n++;
        }
        for (int i = 0; i < BattleMgr.Instance.BattleRoleList.Count; i++) {
            BattleSpeedIconItem script = SpeedRoleItems[i].GetComponent<BattleSpeedIconItem>();
            script.InitItem(BattleMgr.Instance.BattleRoleList[i]);
        }
    }

    private void AddNewRoleSpeedItem() {
        GameObject itemGo = NGUITools.AddChild(SpeedBar, "BattleSpeedIconItem");
        SpeedRoleItems.Add(itemGo);
    }

    //设置当前攻击的攻击者和受击者
    private void SetAttackBothSide(RoleType type) {
        //攻击者
        for (int i = 0; i < BattleMgr.Instance.BattleRoleList.Count; i++) {
            if (BattleMgr.Instance.BattleRoleList[i].baseID == BattleMgr.Instance.AttackRoleData.baseID) {
                this.AttackRoleGo = RoleItems[i];
                break;
            }
        }
        //受击者
        List<int> index = new List<int>();      //存活的受击方索引号
        switch (type) {
            case RoleType.Hero:
                index = BattleMgr.Instance.GetAliveItemIndex(RoleType.Npc);
                break;
            case RoleType.Npc:
                index = BattleMgr.Instance.GetAliveItemIndex(RoleType.Hero);
                break;
            default:
                break;
        }
        int beAttackedIndex = index[Random.Range(0, index.Count - 1)];
        BattleMgr.Instance.BeAttackedRoleData = BattleMgr.Instance.BattleRoleList[beAttackedIndex];
        this.BeAttackedRoleGo = RoleItems[beAttackedIndex];
    }

    private void BattleStart() {
        BattleMgr.Instance.CurBattleStatus = BattleStatus.BattleStart;

        //攻击者攻击
        BattleRoleItem script = this.AttackRoleGo.GetComponent<BattleRoleItem>();
        script.SetAttackTarget(this.BeAttackedRoleGo);
        script.AttackStart();

        BattleMgr.Instance.CalCurRoundDamege();     //计算本次攻击伤害，更新双方信息
    }

    private void BeforeNextRound() {
        //战斗未结束时，下一轮开始前，移除死亡物体，重置攻击者速度条
        if (BattleMgr.Instance.CheckBeAttackedIsDead()) {
            for (int i = 0; i < SpeedRoleItems.Count; i++) {
                if (SpeedRoleItems[i].GetComponent<BattleSpeedIconItem>().GetMyRoleData().hp <= 0) {
                    SpeedRoleItems[i].SetActive(false);
                }
                if (SpeedRoleItems[i].GetComponent<BattleSpeedIconItem>().GetMyRoleData().baseID == BattleMgr.Instance.AttackRoleData.baseID) {
                    Debug.LogError("1");
                    SpeedRoleItems[i].GetComponent<BattleSpeedIconItem>().SetToOrigPos();
                }
            }
            BeAttackedRoleGo.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	}

    public override void SendWindowMsg(WindowMessage msg, params object[] args) {
        base.SendWindowMsg(msg, args);
        switch (msg) { 
            case WindowMessage.eWindow_BeAttacked:
                //受击者被攻击
                BattleRoleItem script = this.BeAttackedRoleGo.GetComponent<BattleRoleItem>();
                script.SetBeAttackTargetPos((Vector3)args[0]);
                script.BeAttackedStart();
                break;
            case WindowMessage.eWindow_BattleStart:
                SetAttackBothSide((RoleType)args[0]); 
                BattleStart();
                break;
            case WindowMessage.eWindow_CurRoundOver:
                BeforeNextRound();
                if (BattleMgr.Instance.IsBattleOver()) {
                    BattleMgr.Instance.CurBattleStatus = BattleStatus.Invalid;
                    return;
                }
                BattleMgr.Instance.bStartRun = true;   //本轮攻击结束，开始跑速
                break;
            default:
                break;
        }
    }
}
