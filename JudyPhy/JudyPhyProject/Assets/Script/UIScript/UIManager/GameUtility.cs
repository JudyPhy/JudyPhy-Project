using UnityEngine;
using System.Collections;

public class GameUtility 
{

    //添加子节点
    public static void AddChildToTarget(Transform target, Transform child)
    {
        child.parent = target;
        child.localScale = Vector3.one;
        child.localPosition = Vector3.zero;
        child.localEulerAngles = Vector3.zero;

        ChangeChildLayer(child, target.gameObject.layer);
    }

    //修改Layer
    public static void ChangeChildLayer(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            ChangeChildLayer(child, layer);
        }
    }
}
