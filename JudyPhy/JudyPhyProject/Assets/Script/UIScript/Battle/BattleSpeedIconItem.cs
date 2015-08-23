using UnityEngine;
using System.Collections;

public class BattleSpeedIconItem : MonoBehaviour {

    public UISprite Icon;

    private float MySpeed;

    private Vector3 MyOrigPos;

    private int FormationPosIndex;   //阵型中的索引号
    private int SpeedPosIndex;   //速度条中的索引号

    private RoleData MyRoleData;

	// Use this for initialization
	void Start () {
	}

    public void InitItem(RoleData data) {
        this.gameObject.SetActive(true);
        this.MyRoleData = data;
        this.transform.localPosition = new Vector3(-210, 4, 0);
        this.MyOrigPos = this.transform.localPosition;
        this.Icon.spriteName = "t_" + data.iconName;
    }

    public void SetToOrigPos() {
        this.transform.localPosition = new Vector3(-210, 4, 0);
    }

    public RoleData GetMyRoleData() {
        return this.MyRoleData;
    }
	
	// Update is called once per frame
    void Update() {
        if (BattleMgr.Instance.bStartRun) {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x + Time.deltaTime * this.MyRoleData.attackSpeed, this.transform.localPosition.y, 0);
            //Debug.LogError("this.transform.localPosition:" + this.transform.localPosition + "  Time.deltaTime * MySpeed:" + (Time.deltaTime * MySpeed) + "  speed:" + MySpeed);
            if (this.transform.localPosition.x > 210) {
                Debug.LogError("本轮开始:" + this.MyRoleData.baseID);
                BattleMgr.Instance.bStartRun = false;   //暂停跑速，准备播放战斗动画
                BattleMgr.Instance.AttackRoleData = this.MyRoleData;
                UIManager.Instance.SendWindowMsg(WindowID.Battle, WindowMessage.eWindow_BattleStart, this.MyRoleData.roleType);
            }
        }
    }
}
