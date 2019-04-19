using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Control : MonoBehaviour {

    //Assigned in Unity
    public float speed;
    public float talk_distance;
    public Talk_animation Talk_ani;
    public GameObject Talk_effect;
    public GameObject star_effect;
    public Slider EnergySlider;
    public Slider Girl_status;
    public float talk_speed;
    Animator PLAnimator;

    public AudioSource BGM;
    public AudioSource saxman;
    public AudioSource HeartBeats;
    public AudioSource star_SE;
    float past_time;

    GameObject talk_eff_use;
    GameObject star_eff_use;

    public int is_facing_left;
    float horizontal_movement;
    public Rigidbody2D rigidBody;
    float vertical_movement;

    NPC_Controller npc_controller;
    Yuno_Control yuno;
    NPC_Walk talk_npc;
    Talk_animation talk_ani;
    public bool is_talking_to_girls;



    public float dicrease = -0.01f;
    float minimun = 0.002f;
    public float LevelBonus = 1;
    public float full_bonus = 5;
    // Use this for initialization
    void Start () {
        vertical_movement = 0f;
        horizontal_movement = 0f;
        npc_controller = FindObjectOfType<NPC_Controller>();
        yuno = FindObjectOfType<Yuno_Control>();
        rigidBody = GetComponent<Rigidbody2D>();
        is_talking_to_girls = false;
        is_facing_left = 0;
        past_time = 0f;
        PLAnimator = GetComponent<Animator>();
        PLAnimator.SetBool("Walk", false);
        print("LevelCount:" + LevelsCount.CurrentLevel);
    }
    
    // Update is called once per frame
    void Update () {
        Move();
        TriggerHeartBeatSound();
        EnergyControll(dicrease * Time.deltaTime);
        if(EnergySlider.value < minimun)//EnergyDrained
        {
            SceneManager.LoadScene("EnergyDrained");
        }
        if (npc_controller == null) npc_controller = FindObjectOfType<NPC_Controller>();
        NPC_Walk npc = npc_controller.GetClosestNPC(transform);
        if(npc != null) InteractWithNPC(npc);
        CheckGameOver();
    }

    void play_interact_music()
    {
        BGM.volume = BGM.volume * 0.1f;
        saxman.Play();
    }

    void InteractWithNPC(NPC_Walk npc){
        float distance = Vector2.Distance(npc.transform.position, transform.position);
        Talk_animation find_ani = FindObjectOfType<Talk_animation>();

        if(is_talking_to_girls){//if is talking to girls
            PLAnimator.SetBool("Walk", false);
            Girl_status.value += talk_speed * Time.deltaTime;
            EnergyControll(talk_npc.power/100 * Time.deltaTime * LevelBonus);
            if(Input.GetKeyDown(KeyCode.Q)) end_conv();
            if(Girl_status.value >= 0.98f){
                star_eff_use = Instantiate(star_effect, Vector3.Lerp(transform.position, npc.transform.position, 0.5f), transform.rotation);
                StartCoroutine(end_star(2.0f));
                EnergyControll(talk_npc.power / 100 * Time.deltaTime * full_bonus);
                end_conv();
            }
        }else if(distance < talk_distance){//if can talk to girls
            if(find_ani == null){//invoke ani
                find_ani = Instantiate(Talk_ani, transform.position, transform.rotation);
                find_ani.transform.parent = gameObject.transform;
                find_ani.transform.localPosition = new Vector3(1.49f, 2.17f, 0);
            }
            if (Input.GetKeyDown(KeyCode.E)){
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                npc.rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                play_interact_music();
                talk_npc = npc;
                talk_npc.StartFlirt();
                is_talking_to_girls = true;
                find_ani.OutOfRange();
                talk_eff_use = Instantiate(Talk_effect, Vector3.Lerp(transform.position, npc.transform.position, 0.5f), transform.rotation);
                StartCoroutine(tmp_interact(10.0f));
            }
        }else{//if no girls to talk
            if(find_ani != null){
                find_ani.OutOfRange();
            }
        }
        
    }
    
    public IEnumerator tmp_interact(float time) {
        // start interact

        yield return new WaitForSeconds(time);
        // end interact
        end_conv();
    }

    public IEnumerator end_star(float time)
    {
        // start interact
        star_SE.Play();
        yield return new WaitForSeconds(time);
        // end interact
        Destroy(star_eff_use);
    }

    void end_conv()
    {
        Destroy(talk_eff_use);
        BGM.volume = BGM.volume * 10f;
        saxman.Stop();
        Girl_status.value = 0;
        StopAllCoroutines();
        is_talking_to_girls = false;
        if(talk_npc != null){
            talk_npc.EndFlirt();
        }
        rigidBody.constraints = RigidbodyConstraints2D.None;
    }

    void Move(){
        //get movement on each direction
        horizontal_movement = Input.GetAxis("Horizontal");
        vertical_movement = Input.GetAxis("Vertical");
        //normalize them, and multiply by speed and time to get the toatal movement
        Vector2 norm = new Vector2(horizontal_movement, vertical_movement).normalized;
        //add movement to the position
        rigidBody.velocity = norm * speed;
        if(Vector2.Distance(norm, Vector2.zero) > 0)
        {
            PLAnimator.SetBool("Walk", true);
        }
        else
        {
            PLAnimator.SetBool("Walk", false);
        }
        // Debug.Log(rigidBody.velocity);
        // change character facing direction
        if (horizontal_movement < 0)
            is_facing_left = 1;
        else if(horizontal_movement > 0)
            is_facing_left = 0;
        transform.localRotation = Quaternion.Euler(0,180*is_facing_left,0);

    }

    void EnergyControll(float change){
        EnergySlider.value += change;
    }

    void TriggerHeartBeatSound(){
        float dis = Vector3.Distance(transform.position, yuno.transform.position);
        past_time += Time.deltaTime;
        if(past_time > dis_to_freq(dis))
        {
            HeartBeats.Play();
            past_time = 0f;
        }
        //TODO: Add sound effects when dis < ?
    }
    
    float dis_to_freq(float dis)
    {
        if (dis > 104)
            return 2f;
        else
            return 0.2f + (dis/104f)*1.8f;
    }

    void CheckGameOver()
    {
        float dis = Vector3.Distance(transform.position, yuno.transform.position);
        if(yuno.found_being_cheated && dis < 1.5f)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
