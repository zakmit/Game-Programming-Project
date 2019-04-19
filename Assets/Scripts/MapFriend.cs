using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFriend : MonoBehaviour {

    Map_Control map_ctl;
    public GameObject Friend;

    // Use this for initialization
    void Start () {
        map_ctl = FindObjectOfType<Map_Control>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = map_ctl.ObjPosToMapPos(Friend.transform.position);
    }
}
