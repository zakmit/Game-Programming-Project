using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour {

    public int st_num;
    public bool isHorizontal;
    bool isTriggered = false;

    Scene_Controller scene_controller;

    // Use this for initialization
    void Start () {
        scene_controller = FindObjectOfType<Scene_Controller>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag != "Player")return;
        if(isTriggered)return;
        isTriggered = true;
        if(isHorizontal){
            StartCoroutine(scene_controller.GotoVerticalStreet(st_num));
        }else{
            StartCoroutine(scene_controller.GotoHorizontalStreet(st_num));
        }
    }
}
