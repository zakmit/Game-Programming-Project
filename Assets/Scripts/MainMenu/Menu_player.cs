using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_player : MonoBehaviour {
    public float speed;
    public float end_point_x;
    public GameObject Talk_effect;
    public GameObject Love;
    public GameObject npc;
    GameObject talk_eff_use;
    public float talk_co;
    public float run_speed;
    public int totallevel;
    float end_speed = 0;
    Animator PLAnimator;
    // Use this for initialization
    void Start () {
        LevelsCount.CurrentLevel = 1;
        LevelsCount.TotalLevel = totallevel;
        PLAnimator = GetComponent<Animator>();
        PLAnimator.SetBool("Walk", true);
    }
    
    // Update is called once per frame
    void Update () {
        Vector3 new_pos = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);
        transform.position = new_pos;
        if (new_pos.x >= end_point_x && speed > 0)
        {
            PLAnimator.SetBool("Walk", false);
            speed = 0;
            talk_eff_use = Instantiate(Talk_effect, Vector3.Lerp(transform.position, npc.transform.position, 0.5f), transform.rotation);
            StartCoroutine(tmp_interact(talk_co));
        }
        if(end_speed > 0)
        {
            Vector3 new_pos2 = transform.position + new Vector3(end_speed * Time.deltaTime, 0, 0);

            transform.position = new_pos2;
        }
        if (transform.position.x > 100)
            Destroy(this);
    }
    public IEnumerator tmp_interact(float time)
    {
        // start interact

        yield return new WaitForSeconds(time);
        Destroy(talk_eff_use);
        yield return new WaitForSeconds(1);
        PLAnimator.SetBool("Walk", true);
        // start run
        end_speed = run_speed;
    }
}
