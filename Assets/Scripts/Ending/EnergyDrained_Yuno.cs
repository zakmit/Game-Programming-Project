using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrained_Yuno : MonoBehaviour {
    EnergyDrained Scene;
    Rigidbody2D rigidBody;
    Animator YunoAnimator;
    public float speed = 1.0f;
    // Use this for initialization
    void Start () {
        Scene = FindObjectOfType<EnergyDrained>();
        rigidBody = GetComponent<Rigidbody2D>();
        YunoAnimator = GetComponent<Animator>();
        YunoAnimator.SetBool("Walk", true);
    }
	
	// Update is called once per frame
	void Update () {
        rigidBody.velocity = Vector3.zero;
        if (Scene.yuno_state == 1)
        {
            if (transform.position.x > 5.2)
            {
                rigidBody.velocity = Vector3.left * speed;
                print(">5.2");
            }
            else
            {
                YunoAnimator.SetBool("Walk", false);
            }
        }
        else if (Scene.yuno_state == 2)
        {
            if (transform.position.x > 1.37)
            {
                YunoAnimator.SetBool("Walk", true);
                rigidBody.velocity = Vector3.left * speed;
            }
            else
            {
                YunoAnimator.SetBool("Walk", false);
            }
        }
    }
}
