using UnityEngine;
using System.Collections;

public enum RoleType{
    Invalid,
    Hero,
    Npc,
}

public class RoleData {

    public int typeID;     //通用ID
    public int baseID;     //唯一ID
    public RoleType roleType = RoleType.Invalid;

    public int maxHp;
    public int hp;
    public int attack;
    public float attackSpeed;

    public string iconAtlas;
    public string iconName;

    public virtual void UpdateRoleAttr(/*服务器传值RoleData data*/) {
        this.hp = 100;
        this.attack = 60;
        this.maxHp = 100;
    }
}
