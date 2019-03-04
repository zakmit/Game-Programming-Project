using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public Player_Control Player;
    public GameObject Point;
    public float offset_smoothing = 5.0f;
    public float not_buffer = 0.3f;
    public bool plus_direction, change_direction;

    Scene_Controller scene_ctl;

    Vector3[] offset;
    Vector3 center;
    Vector3 dis_buffer;
    float buffer_decay;

    Vector3 limit_checker;
    float max_x;
    float min_x;

    // Use this for initialization
    void Start () {
        offset = new Vector3[] {transform.position - Player.transform.position, transform.position - Point.transform.position};
        center = (offset[0] + offset[1])/2;
        dis_buffer = Vector3.zero;
        buffer_decay = 0.05f;
        scene_ctl = FindObjectOfType<Scene_Controller>();
        plus_direction = false;
        change_direction = false;
    }
    
    // Update is called once per frame
    void LateUpdate() {
        
        min_x = scene_ctl.getCameraMinX();
        max_x = scene_ctl.getCameraMaxX();

        Vector3 new_pos;
        if(scene_ctl.isHorizontal){
            new_pos = Player.transform.position + offset[Player.is_facing_left];
        }else{//halven x offset if vertical
            new_pos = Player.transform.position + (offset[Player.is_facing_left] - center)/4f + center;
        }
        new_pos.x = (float)Mathf.Min(new_pos.x, max_x); 
        new_pos.x = (float)Mathf.Max(new_pos.x, min_x); 
        // Reset buffer if direction changed
        if (Vector3.Distance(transform.position - dis_buffer, new_pos) > 0.5f)
            dis_buffer = transform.position - new_pos;
        if (dis_buffer.magnitude > 0.05f)
        {
            // Debug.Log("old: " + transform.position.x + "  new: " + new_pos.x + "   buf: " + dis_buffer.x);
            dis_buffer -= dis_buffer * buffer_decay;
            transform.position = new_pos + dis_buffer;
        }
        else
        {
            dis_buffer = Vector3.zero;
            transform.position = new_pos;
        }
    }
}
