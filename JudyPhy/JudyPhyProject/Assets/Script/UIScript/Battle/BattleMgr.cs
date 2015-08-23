using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//阵型
public enum FormationType {
    Invalid,
    Type1 = 1,
}

//战斗状况
public enum BattleStatus {
    Invalid,
    BattleStart,
    BattleIn,
    BattleOver,
}

public class BattleMgr {
    private static BattleMgr instance;
    public static BattleMgr Instance {
        get {
            if (instance == null)
                instance = new BattleMgr();
            return instance;
        }
    }

    public FormationType MyFormationType;
    public FormationType EnermyFormationType;
    public List<RoleData> BattleRoleList = new List<RoleData>();  
    public bool bStartRun = false;      //速度条是否开始动
    public BattleStatus CurBattleStatus = BattleStatus.Invalid;     //当前战斗状态

    //攻击者信息
    public RoleData AttackRoleData;    

    //受击者信息
    public RoleData BeAttackedRoleData;   

    //本次战斗胜利方
    public RoleType BattleWinerType;

    //测试用
    public void InitHeroData() {
        BattleRoleList.Clear();
        RoleData data1 = new RoleData();
        data1.baseID = 1;
        data1.roleType = RoleType.Hero;
        data1.attackSpeed = 100;
        data1.hp = 100;
        data1.attack = 60;
        data1.maxHp = 100;
        data1.iconName = "lixiaoyao";
        BattleRoleList.Add(data1);

        RoleData data2 = new RoleData();
        data2.baseID = 2;
        data2.roleType = RoleType.Hero;
        data2.attackSpeed = 200;
        data2.hp = 100;
        data2.attack = 60;
        data2.maxHp = 100;
        data2.iconName = "suyao";
        BattleRoleList.Add(data2);

        RoleData data3 = new RoleData();
        data3.baseID = 3;
        data3.roleType = RoleType.Hero;
        data3.attackSpeed = 300;
        data3.hp = 100;
        data3.attack = 60;
        data3.maxHp = 100;
        data3.iconName = "suyu";
        BattleRoleList.Add(data3);
    }

    //测试用
    public void InitEnermyData() {
        EnermyData data1 = new EnermyData();
        data1.baseID = 4;
        data1.roleType = RoleType.Npc;
        data1.attackSpeed = 80;
        data1.hp = 100;
        data1.attack = 60;
        data1.maxHp = 100;
        data1.iconName = "baozi_girl";
        BattleRoleList.Add(data1);

        EnermyData data2 = new EnermyData();
        data2.baseID = 5;
        data2.roleType = RoleType.Npc;
        data2.attackSpeed = 150;
        data2.hp = 100;
        data2.attack = 60;
        data2.maxHp = 100;
        data2.iconName = "baozi_girl";
        BattleRoleList.Add(data2);

        EnermyData data3 = new EnermyData();
        data3.baseID = 6;
        data3.roleType = RoleType.Npc;
        data3.attackSpeed = 280;
        data3.hp = 100;
        data3.attack = 60;
        data3.maxHp = 100;
        data3.iconName = "baozi_girl";
        BattleRoleList.Add(data3);
    }

    public void InitHeroData(List<HeroData> list) {
        BattleRoleList.Clear();
        for (int i = 0; i < list.Count; i++) {
            BattleRoleList.Add(list[i]);
        }
    }

    public void UpdateHeroData(HeroData data) {

    }

    public void CalCurRoundDamege() {
        //BeAttackedRoleData.hp -= AttackRoleData.attack;
        for (int i = 0; i < BattleRoleList.Count; i++) {
            if (BeAttackedRoleData.baseID == BattleRoleList[i].baseID) {
                BattleRoleList[i].hp = BattleRoleList[i].hp - AttackRoleData.attack;
            }
        }
    }

    //获取存活物体索引号
    public List<int> GetAliveItemIndex(RoleType type) {
        List<int> alive = new List<int>();
        for (int i = 0; i < BattleRoleList.Count; i++) {
            if (BattleRoleList[i].roleType == type && BattleRoleList[i].hp > 0) {
                alive.Add(i);
            }
        }
        return alive;
    }

    public bool CheckBeAttackedIsDead() {
        if (BeAttackedRoleData.hp <= 0)
            return true;
        return false;
    }

    public bool IsBattleOver() {
        bool herolose = true;
        for (int i = 0; i < BattleRoleList.Count; i++) {
            if (BattleRoleList[i].roleType == RoleType.Hero && BattleRoleList[i].hp > 0)
                herolose = false;
        }
        bool enermylose = true;
        for (int i = 0; i < BattleRoleList.Count; i++) {
            if (BattleRoleList[i].roleType == RoleType.Npc && BattleRoleList[i].hp > 0)
                enermylose = false;
        }
        if (!herolose && !enermylose)
            return false;
        else if (herolose && !enermylose) {
            BattleWinerType = RoleType.Npc;
            return true;
        } else if (!herolose && enermylose) {
            BattleWinerType = RoleType.Hero;
            return true;
        } else {
            Debug.LogError("双方都死亡，BUG");
        }
        return false;
    }
}
