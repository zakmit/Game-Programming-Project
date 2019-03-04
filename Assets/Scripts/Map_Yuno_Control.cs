using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Yuno_Control : MonoBehaviour {

    Map_Control map_ctl;
    Yuno_Control yuno;

    // Use this for initialization
    void Start () {
        map_ctl = FindObjectOfType<Map_Control>();
        yuno = FindObjectOfType<Yuno_Control>();
    }
    
    // Update is called once per frame
    void Update () {
            transform.localPosition = map_ctl.ObjPosToMapPos(yuno.transform.position);
    }
}
