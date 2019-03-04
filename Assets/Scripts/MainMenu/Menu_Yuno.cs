using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Yuno : MonoBehaviour {
    float speed;
    public float end_speed;
    public float wait_time;
    Animator YunoAnimator;
    // Use this for initialization
    void Start () {
        speed = 0;
        YunoAnimator = GetComponent<Animator>();
        YunoAnimator.SetBool("Walk", true);
    }
    
    // Update is called once per frame
    void Update () {
        StartCoroutine(tmp_interact(wait_time));
        if (speed > 0)
        {
            Vector3 new_pos2 = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);

            transform.position = new_pos2;
        }
        if (transform.position.x > 100)
            Destroy(this);
    }
    public IEnumerator tmp_interact(float time)
    {
        // start interact

        yield return new WaitForSeconds(time);
        // start run
        speed = end_speed;
    }
}
