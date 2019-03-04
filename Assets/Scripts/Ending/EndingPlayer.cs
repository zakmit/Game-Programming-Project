using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPlayer : MonoBehaviour {
    public bool ending;
    public float speed = 3.0f;
    public float power;
    public List<GameObject> npc_list;
    public float talk_distance = 3f;
    public GameObject Talk_effect;
    public GameObject talk_eff_use;
    bool talk_to_girl;
    //bool talk_to_girls;
    Vector3 last_interact;
    public GameObject Continue_UI;
    Rigidbody2D rigidBody;
    Animator PLAnimator;
    // Use this for initialization
    void Start () {
        talk_to_girl = false;
        ending = false;
        rigidBody = GetComponent<Rigidbody2D>();
        last_interact = transform.position;
        PLAnimator = GetComponent<Animator>();
        PLAnimator.SetBool("Walk", false);
        if (LevelsCount.CurrentLevel == LevelsCount.TotalLevel)
        { ending = true; }
        Continue_UI.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		if(ending)
        {
            Move();
            GameObject npc = GetClosestNPC(transform);
            if (npc != null) InteractWithNPC(npc);
        }
        if(transform.position.x > 220)
        {
            transform.position = new Vector3(-48,-1.5f,0);
        }
    }
    void Move()
    {
        //add movement to the position
        rigidBody.velocity = Vector3.right * speed;
        PLAnimator.SetBool("Walk", true);
        // Debug.Log(rigidBody.velocity);
    }
    public GameObject GetClosestNPC(Transform t)
    {
        float max_distance = 10000f;
        GameObject target = null;
        foreach (GameObject npc in npc_list)
        {
            if (npc == null)
            {
                npc_list.Remove(npc);
                break;
            }
            float dis = Vector2.Distance(npc.transform.position, t.position);
            if (dis < max_distance)
            {
                target = npc;
                max_distance = dis;
            }
        }
        return target;
    }
    void InteractWithNPC(GameObject npc)
    {
        if(talk_to_girl)
            PLAnimator.SetBool("Walk", false);
        float distance = Vector2.Distance(npc.transform.position, transform.position);
        if (distance < talk_distance && Vector2.Distance(last_interact, transform.position) > 9f)
        {
            if (talk_eff_use == null)//invoke ani
            {
                last_interact = transform.position;
                talk_to_girl = true;
                print("last_interact");
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                //talk_to_girls = true;
                talk_eff_use = Instantiate(Talk_effect, Vector3.Lerp(transform.position, npc.transform.position, 0.5f), transform.rotation);
                StartCoroutine(tmp_interact(7.0f));
            }
        }
    }
    public IEnumerator tmp_interact(float time)
    {
        // start interact
        print("in");
        yield return new WaitForSeconds(time);
        // end interact
        end_conv();
    }
    void end_conv()
    {
        Destroy(talk_eff_use);
        StopAllCoroutines();
        talk_to_girl = false;
        rigidBody.constraints = RigidbodyConstraints2D.None;
    }

}
