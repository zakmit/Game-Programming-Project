using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Walk : MonoBehaviour {


    public float speed = 1.0f;
    public float power;
    
    public Rigidbody2D rigidBody;
    SpriteRenderer sprite;
    Scene_Controller scene_controller;
    Vector3[,] dir_arr;
    Vector3 direction;
    NPC_Controller npc_controller;
    Animator GirlAnimator;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        GirlAnimator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        GirlAnimator.SetBool("Walk", true);
        npc_controller = FindObjectOfType<NPC_Controller>();
        scene_controller = FindObjectOfType<Scene_Controller>();
        dir_arr = new Vector3[2,2]{{Vector3.up, Vector3.down}, {Vector3.right, Vector3.left}};
        direction = dir_arr[scene_controller.isHorizontal ? 1 : 0, Random.Range(0,2)];
    }
    
    // Update is called once per frame
    void Update () {
        rigidBody.velocity = direction * speed;
        //reach edge change direction
        if(rigidBody.velocity.x < 0)
            transform.localRotation = Quaternion.Euler(0,180,0);
        else if(rigidBody.velocity.x > 0)
            transform.localRotation = Quaternion.Euler(0,0,0);
    }
    public void StartFlirt()
    {
        GirlAnimator.SetBool("Walk", false);
    }
    public void EndFlirt(){
        float duration = 3f;
        GirlAnimator.SetBool("Walk", true);
        if (npc_controller == null)npc_controller = FindObjectOfType<NPC_Controller>();
        npc_controller.RemoveNPC(this);
        rigidBody.constraints = RigidbodyConstraints2D.None;
        StartCoroutine(BecomeTransparent(duration));
    }

    IEnumerator BecomeTransparent(float duration){
        for(int i = 0; i < 100; i++){
            sprite.color = new Color (1f, 1f, 1f, 1f - (float)i / 100f * .7f);
            yield return new WaitForSeconds(duration/100f);
        }
    }
}
