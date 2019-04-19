using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Player_Control : MonoBehaviour {

    Map_Control map_ctl;
    Player_Control player;

    // Use this for initialization
    void Start () {
        map_ctl = FindObjectOfType<Map_Control>();
        player = FindObjectOfType<Player_Control>();
    }
    
    // Update is called once per frame
    void Update () {
            transform.localPosition = map_ctl.ObjPosToMapPos(player.transform.position);
    }
}
