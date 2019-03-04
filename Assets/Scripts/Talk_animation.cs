using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk_animation : MonoBehaviour {
    private Animator talk_Ani;
    public float ani_end_time = 1.5f;
    // Use this for initialization
    void Start () {
        talk_Ani = GetComponent<Animator> ();
    }
    
    // Update is called once per frame
    void Update () {
    }

    IEnumerator Delete_Ani(float ani_end_time) {
        yield return new WaitForSeconds(ani_end_time);
        Destroy(gameObject);
    }

    public void OutOfRange(){
        talk_Ani.SetBool("out_of_range",true);
        StartCoroutine("Delete_Ani", ani_end_time);
    }
}
