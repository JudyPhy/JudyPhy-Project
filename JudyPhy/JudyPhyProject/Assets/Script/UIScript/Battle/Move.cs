using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    private Vector3 OrigPos;     //起始坐标

    private Vector3 TargetPos;       //目标坐标

    private Vector3[] MovePath;

    private Vector3 MoveBackVec;    //被攻击者移动目标坐标

    private Vector3 TempPos;    //闪现目标坐标

    private bool SendBackMsg = false;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    public void SetOrigPos(Vector3 origPos) {
        this.OrigPos = origPos;
    }

    public void SetTargetPos(Vector3 targetPos) {
        this.TargetPos = targetPos;
    }

    ////跳跃+移动
    //private void SetJumpPath() {
    //    iTweenPath path = this.gameObject.GetComponent<iTweenPath>();
    //    if (!path)
    //        path = this.gameObject.AddComponent<iTweenPath>();
    //    path.pathName = "pathJump";     //初始有2个点
    //    path.nodes[0] = OrigPos;

    //    if (TargetPos.y < OrigPos.y) {
    //        //取前3/5做抛物线，后线性
    //        float rate = 0.8f;
    //        Vector3 def = TargetPos - OrigPos;
    //        Vector3 tempPos = new Vector3(OrigPos.x + def.x * rate, OrigPos.y, 0);
    //        path.nodes[1] = new Vector3(OrigPos.x + (tempPos.x - OrigPos.x) * 0.5f, 0.5f, 0);
    //        path.nodes.Add(TargetPos);
    //        PathToVec(path);
    //        //计算被攻击者退后的方向
    //        def = TargetPos - tempPos;
    //        float k = def.y / def.x;
    //        float a = TargetPos.y - TargetPos.x * k;
    //        float newX = TargetPos.x + (TargetPos.x > OrigPos.x ? 0.1f : -0.1f);
    //        float newY = k * newX + a;
    //        MoveBackVec = new Vector3(newX, newY, 0);
    //    } else if (TargetPos.y == OrigPos.y) {
    //        path.nodes[1] = OrigPos;
    //    } else if (TargetPos.y > OrigPos.y) {
    //        path.nodes[1] = OrigPos;
    //    }
    //}

    //private void PathToVec(iTweenPath path) {
    //    MovePath = new Vector3[path.nodes.Count];
    //    for (int i = 0; i < path.nodes.Count; i++) {
    //        MovePath[i] = path.nodes[i];
    //    }
    //}

    public void HideAndMove() {
        this.gameObject.SetActive(false);
        float y = (TargetPos.y > OrigPos.y) ? TargetPos.y - 0.5f : TargetPos.y + 0.5f;
        float y_back = (TargetPos.y > OrigPos.y) ? TargetPos.y + 0.2f : TargetPos.y - 0.2f;
        if (TargetPos.x == OrigPos.x) {
            this.transform.position = new Vector3(TargetPos.x, y, 0);
            MoveBackVec = new Vector3(TargetPos.x, y_back, 0);
        } else {
            float k = (TargetPos.y - OrigPos.y) / (TargetPos.x - OrigPos.x);       //战斗双方y坐标不能相同
            float x = (y - (OrigPos.y - k * OrigPos.x)) / k;
            this.transform.position = new Vector3(x, y, 0);
            x = (y_back - (OrigPos.y - k * OrigPos.x)) / k;
            MoveBackVec = new Vector3(x, y_back, 0);
        }
        Invoke("ShownAndAttack", 0.1f);
    }

    private void ShownAndAttack() {
        this.gameObject.SetActive(true);
        SendBackMsg = true;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.TargetPos, "time", 0.2f, "easyType", iTween.EaseType.easeInQuad,
            "onupdate", "IsMoveBack", "oncomplete", "MoveFrom"));
    }

    public void MoveFrom() {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.OrigPos, "time", 0.4f, "easyType", iTween.EaseType.linear));
    }

    private void IsMoveBack() {
        if (SendBackMsg && Mathf.Abs(this.transform.position.x - TargetPos.x) < 0.17f && Mathf.Abs(this.transform.position.y - TargetPos.y) < 0.17f) {
            UIManager.Instance.SendWindowMsg(WindowID.Battle, WindowMessage.eWindow_BeAttacked, MoveBackVec);
            SendBackMsg = false;
        }
    }

    public void MoveBackTo() {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.TargetPos, "time", 0.4f, "easyType", iTween.EaseType.easeInQuad,
                 "oncomplete", "MoveBackFrom"));
    }

    public void MoveBackFrom() {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.OrigPos, "time", 0.4f, "easyType", iTween.EaseType.linear,
            "oncomplete", "CurRoundOver"));
    }

    public void CurRoundOver() {
        BattleMgr.Instance.CurBattleStatus = BattleStatus.BattleOver;
        UIManager.Instance.SendWindowMsg(WindowID.Battle, WindowMessage.eWindow_CurRoundOver);
    }


    ////闪现+移动
    //public void FadeIn() {
    //    this.gameObject.SetActive(false);
    //    Invoke("FadeOut", 0.2f);
    //}

    //private void FadeOut() {
    //    this.gameObject.transform.position = GetReshowTempPos();
    //    this.gameObject.SetActive(true);
    //    SendBackMsg = true;
    //    MoveBackVec = new Vector3(this.TargetPos.x + (TargetPos.x > OrigPos.x ? 0.4f : -0.4f), this.TargetPos.y, 0);
    //    iTween.MoveTo(this.gameObject, iTween.Hash("position", this.TargetPos, "time", 0.4f, "easyType", iTween.EaseType.easeInQuad,
    //       "onupdate", "IsMoveBack", "oncomplete", "MoveFrom"));
    //}

    //private Vector3 GetReshowTempPos() {
    //    Vector3 tempPos = new Vector3(TargetPos.x + (TargetPos.x > OrigPos.x ? -0.6f : 0.6f), TargetPos.y, 0);
    //    return tempPos;
    //}


}
