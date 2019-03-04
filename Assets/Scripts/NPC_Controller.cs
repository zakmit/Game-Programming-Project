using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour {

    //Assigned by Unity.
    public NPC_Walk[] npc_prefab;

    List<NPC_Walk> npc_list;
    Scene_Controller scene_controller;
    float spawn_cd;
    Vector3 npc_spawn_offset;
    Vector3 npc_spawn_range;
    Player_Control player;

    // Use this for initialization
    void Start () {
        // npc_prefab = Resources.Load("Assets/Prefabs/Girl_NPC_Proto") as GameObject;

        Random.InitState((int)System.DateTime.Now.Ticks);
        npc_list = new List<NPC_Walk>();
        scene_controller = FindObjectOfType<Scene_Controller>();
        npc_spawn_offset = new []{new Vector3(-7,-39), new Vector3(-34, -10)}[scene_controller.isHorizontal?1:0];
        npc_spawn_range = new []{new Vector3(10,234), new Vector3(243, 12)}[scene_controller.isHorizontal?1:0];
        player = FindObjectOfType<Player_Control>();

        spawn_cd = 0f;
        SpawnNPC();
        SpawnNPC();
    }
    
    // Update is called once per frame
    void Update () {
        if(Time.time > spawn_cd){
            spawn_cd = spawn_cd * 1.5f + 10f;
            SpawnNPC();
        } 
    }

    public void RemoveNPC(NPC_Walk npc){
        npc_list.Remove(npc);
    }

    void SpawnNPC(){
        var idx = Random.Range(0, npc_prefab.Length);
        NPC_Walk new_npc = Instantiate(npc_prefab[idx]) as NPC_Walk;
        Vector3 pos;
        while(true){
            pos = npc_spawn_offset + new Vector3(Random.Range(0, npc_spawn_range.x), Random.Range(0, npc_spawn_range.y));
            pos = pos + this.transform.position;
            bool check = true;
            if(Vector3.Distance(pos, player.transform.position) < 20f)continue;
            foreach(NPC_Walk npc in npc_list){
                if(Vector3.Distance(pos, npc.transform.position) < 10f)check = false;
            }
            if(check)break;
        }
        new_npc.transform.SetParent(this.transform);
        new_npc.transform.position = pos;

        npc_list.Add(new_npc);
    }

    public NPC_Walk GetClosestNPC(Transform t){
        float max_distance = 10000f;
        NPC_Walk target = null;
        foreach(NPC_Walk npc in npc_list){
            if(npc == null){
                npc_list.Remove(npc);
                break;
            }
            float dis = Vector2.Distance(npc.transform.position, t.position);
            if(dis < max_distance){
                target = npc;
                max_distance = dis;
            }
        }
        return target;
    }
}
