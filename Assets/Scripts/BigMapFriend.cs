using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMapFriend : MonoBehaviour {

    Big_Map_Control map_ctl;
    public GameObject Friend;
    RectTransform RectTrans;
    // Use this for initialization
    void Start()
    {
        RectTrans = GetComponent<RectTransform>();
        map_ctl = FindObjectOfType<Big_Map_Control>();
    }

    // Update is called once per frame
    void Update()
    {
        RectTrans.localPosition = map_ctl.ObjPosToMapPos(Friend.transform.position);
    }
}
