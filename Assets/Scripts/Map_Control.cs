using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Control : MonoBehaviour {

    public float road_width;
    public float map_road_width;
    public Vector3 obj_offset, map_offset;

    // Use this for initialization
    void Start () {
        obj_offset = new Vector3(-18f, -5f);
        map_offset = new Vector3(-1.2f, -1.2f);
        road_width = 52f;
        map_road_width = 0.6f;
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    public Vector3 ObjPosToMapPos(Vector3 v){
        return (v - obj_offset) / road_width * map_road_width + map_offset;
    }

    public Vector3 MapPosToObjPos(Vector3 v){
        return (v - map_offset) / map_road_width * road_width + obj_offset;
    }
}
