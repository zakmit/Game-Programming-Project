using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yuno_Control : MonoBehaviour {

    //Assigned in Unity
    public Player_Control player;
    public Map_Control map_ctl;
    public float speed;
    public bool found_being_cheated;


    Rigidbody2D rigidBody;
    Vector3[] dir_n;
    int[] dir_x;
    int[] dir_y;
    enum StrategyType {wander, running, chasing, stop_and_find, stick};
    StrategyType strategy_type;
    float pause_time;
    Animator YunoAnimator;

    //Wander
    Vector3 target_idx;
    float[, ] heatmap;
    Vector3 heatmap_last_update;
    //Running
    Vector3 spot_pos;

    //Chasing

    //MultiThreading?
    bool mutex_lock;

    // Use this for initialization
    void Start () {
        YunoAnimator = GetComponent<Animator>();
        YunoAnimator.SetBool("Walk", true);
        found_being_cheated = false;
        rigidBody = GetComponent<Rigidbody2D>();
        Random.InitState((int)System.DateTime.Now.Ticks);
        Random.Range(0,1);
        dir_n = new Vector3[4]{Vector3.left, Vector3.right, Vector3.up, Vector3.down};
        dir_x = new int[4]{-1, 1, 0, 0};
        dir_y = new int[4]{0, 0, 1, -1};
        target_idx = new Vector3(0,1);
        strategy_type = StrategyType.wander;
        mutex_lock = false;
        heatmap = new float[5, 5];
        for(int i = 0; i < 5; i++){
            for(int j = 0; j < 5; j++){
                heatmap[i,j] = 0.04f;
            }
        }
        heatmap_last_update = new Vector3(0, 0);
    }
    
    // Update is called once per frame
    void Update () {
        Vector3 dir = new Vector3(0,0,0);
        if(mutex_lock)return;

        if(CanSeePlayer() && player.is_talking_to_girls){
            if(Vector3.Distance(transform.position, player.transform.position) < 30f){
                strategy_type = StrategyType.chasing;
            }else{
                strategy_type = StrategyType.running;
                spot_pos = GetNearestIntersection(player.transform.position);
            }
            found_being_cheated = true;
        }else if(CanSeePlayer(1.3f) && !found_being_cheated){
            strategy_type = StrategyType.stick;
        }else if(found_being_cheated && CanSeePlayer(0.5f)){
            strategy_type = StrategyType.chasing;
        }

        UpdateHeatmap();
        if(strategy_type == StrategyType.stick){
            dir = GetStickDirection().normalized;
        }else if(strategy_type == StrategyType.wander){
            dir = GetWanderDirection().normalized;
        }else if(strategy_type == StrategyType.running){
            dir = GetRunningDirection().normalized;
            if(Vector3.Distance(spot_pos, transform.position) < 0.5f){
                strategy_type = StrategyType.stop_and_find;
            }
        }else if(strategy_type == StrategyType.chasing){
            StartCoroutine(Chase());
        }else if(strategy_type == StrategyType.stop_and_find){
            StartCoroutine(StopAndFind(3.0f));
        }
        
        rigidBody.velocity = dir * speed;
        if(rigidBody.velocity.x < 0)
            transform.localRotation = Quaternion.Euler(0,180,0);
        else if(rigidBody.velocity.x > 0)
            transform.localRotation = Quaternion.Euler(0,0,0);
        if(strategy_type == StrategyType.stick)rigidBody.velocity = dir * player.speed * 0.6f;
        if(Vector2.Distance(rigidBody.velocity, Vector2.zero) < 0.1f)
        {
            YunoAnimator.SetBool("Walk", false);
        }
        else
        {
            YunoAnimator.SetBool("Walk", true);
        }
        // rigidBody.velocity = (Intersections(new Vector3(1,1)) - transform.position).normalized * speed;
    }

    IEnumerator StopAndFind(float wait_time){
        mutex_lock = true;
        Debug.Log("StopAndFind");
        YunoAnimator.SetBool("Walk", false);
        yield return new WaitForSeconds(wait_time);
        YunoAnimator.SetBool("Walk", true);
        if (CanSeePlayer() && player.is_talking_to_girls){
            if(Vector3.Distance(transform.position, player.transform.position) < 30f){
                strategy_type = StrategyType.chasing;
            }else{
                strategy_type = StrategyType.running;
                spot_pos = GetNearestIntersection(player.transform.position);
            }
            found_being_cheated = true;
        }else if(CanSeePlayer()){
            if(Vector3.Distance(transform.position, player.transform.position) < 30f){
                strategy_type = StrategyType.chasing;
            }else{
                strategy_type = StrategyType.running;
                Vector3 spot_idx = GetNearestIntersectionIdx(spot_pos);
                Vector3 player_idx = spot_idx + dir_n[0];
                for(int i = 0; i < 4; i++){
                    if((spot_idx + dir_n[i]).x < 0 || (spot_idx + dir_n[i]).x >= 5 || (spot_idx + dir_n[i]).y < 0 || (spot_idx + dir_n[i]).y >= 5)continue;
                    if(Vector3.Distance(Intersections(player_idx), player.transform.position) > Vector3.Distance(Intersections(spot_idx + dir_n[i]), player.transform.position) ){
                        player_idx = spot_idx + dir_n[i];
                    }
                }
                Debug.Log("StopAndFind : " + player_idx.x + " " + player_idx.y);
                spot_pos = Intersections(player_idx);
            }
        }else{
            strategy_type = StrategyType.wander;
            target_idx = GetNearestIntersectionIdx(transform.position);
        }
        mutex_lock = false;
    }

    IEnumerator Chase(){
        Debug.Log("Chase!");
        mutex_lock = true;
        float chasing_time = Time.time + 10f;
        while(Time.time < chasing_time){
            rigidBody.velocity = (player.transform.position - transform.position).normalized * speed * 1.4f;
            yield return null;
        }
        chasing_time = Time.time + 5f;
        while(Time.time < chasing_time){
            rigidBody.velocity = (player.transform.position - transform.position).normalized * speed * 0.8f;
            yield return null;
        }
        strategy_type = StrategyType.stop_and_find;
        mutex_lock = false;
    }

    Vector3 GetStickDirection(){
        if(!CanSeePlayer(1.3f)){
            strategy_type = StrategyType.wander;
            target_idx = GetNearestIntersectionIdx(transform.position);
            return player.transform.position - transform.position;
        }

        if(Vector3.Distance(player.transform.position, transform.position) > 2f){
            return player.transform.position - transform.position;
        }else{
            Vector3 ret = (player.transform.position - transform.position);
            return new Vector3(0, 0);
        }
    }

    Vector3 GetRunningDirection(){
        return spot_pos - transform.position;
    }

    Vector3 GetWanderDirection(){
        Vector3 target_pos = Intersections(target_idx);
        if(Vector3.Distance(target_pos, transform.position) < 0.5f){
            // Vector3 new_target;
            // do{
            //     new_target = dir_n[Random.Range(0,4)] + target_idx;
            // }while(new_target.x < 0 || new_target.x >= 5 || new_target.y < 0 || new_target.y >= 5);
            // target_idx = new_target;
            Vector3 new_target, tmp;
            tmp = new Vector3(0, 0);
            float tmp_heat = 0f;
            float heat_sum = 0f;
            for(int i = 0; i < 5; i++){
                for(int j = 0; j < 5; j++){
                    heat_sum += heatmap[i,j];
                    if(heatmap[i,j] > tmp_heat){
                        tmp = new Vector3(i, j);
                        tmp_heat = heatmap[i,j];
                    }
                }
            }
            new_target = target_idx + dir_n[0];
            float dis = 10000f;
            for(int i = 0; i < 4; i++){
                int x = (int)target_idx.x + dir_x[i], y = (int)target_idx.y + dir_y[i];
                if(x < 0 || x >= 5 || y < 0 || y >= 5)continue;
                if(dis > Vector3.Distance(tmp, target_idx + dir_n[i])){
                    dis = Vector3.Distance(tmp, target_idx + dir_n[i]);
                    new_target = target_idx + dir_n[i];
                }
            }
            // Debug.Log("max at " + (int)tmp.x + " " + (int)tmp.y + " :" + tmp_heat + " total: " + heat_sum);
            target_idx = new_target;
        }
        return Intersections(target_idx) - transform.position;
    }

    Vector3 Intersections(Vector3 p){
        return p * map_ctl.road_width + map_ctl.obj_offset;
    }

    Vector3 GetNearestIntersection(Vector3 p){
        p = p - map_ctl.obj_offset;
        int x = (int)(p.x / map_ctl.road_width + 0.5f);
        int y = (int)(p.y / map_ctl.road_width + 0.5f);
        return Intersections(new Vector3(x, y));
    }

    Vector3 GetNearestIntersectionIdx(Vector3 p){
        p = p - map_ctl.obj_offset;
        int x = (int)(p.x / map_ctl.road_width + 0.5f);
        int y = (int)(p.y / map_ctl.road_width + 0.5f);
        return new Vector3(x, y);
    }

    bool CanSeePlayer(float see_block = 2f){
        float see_range = see_block * map_ctl.road_width;
        Vector3 yuno_pos = transform.position;
        Vector3 pl_pos = player.transform.position;
        if( InHRoadIdx(yuno_pos) >= 0 && 
            InHRoadIdx(pl_pos) == InHRoadIdx(yuno_pos) && 
            Vector3.Distance(yuno_pos, pl_pos) < see_range){
                return true;
        }
        if( InVRoadIdx(yuno_pos) >= 0 && 
            InVRoadIdx(pl_pos) == InVRoadIdx(yuno_pos) && 
            Vector3.Distance(yuno_pos, pl_pos) < see_range){
                return true;
        }
        return false;
    }

    public int InVRoadIdx(Vector3 p){//Returns vertical road num p is in. return null if not in vertical road.
        p += new Vector3(7, 7);
        p -= map_ctl.obj_offset;
        float tmp = p.x % map_ctl.road_width;
        if(tmp > 14f)return -1;
        return (int)(p.x / map_ctl.road_width);
    }

    public int InHRoadIdx(Vector3 p){//Returns horizontal road num p is in. return null if not in horizontal road.
        p += new Vector3(7, 5) - map_ctl.obj_offset;
        float tmp = p.y % map_ctl.road_width;
        if(tmp > 14f)return -1;
        return (int)(p.y / map_ctl.road_width);
    }

    void UpdateHeatmap(){
        Vector3 now_idx = GetNearestIntersectionIdx(transform.position);
        if(now_idx == heatmap_last_update || Vector3.Distance(Intersections(now_idx), transform.position) > 1f)return;
        heatmap_last_update = now_idx;
        //transform
        float[,] oldheatmap = new float[5,5];
        for(int i = 0; i < 5; i++){
            for(int j = 0; j < 5; j++){
                oldheatmap[i, j] = heatmap[i, j];
                heatmap[i, j] = 0f;
            }
        }
        for(int i = 0; i < 5; i++){
            for(int j = 0; j < 5; j++){
                int dir_num = 4;
                if(i == 0 || i == 4)dir_num--;
                if(j == 0 || j == 4)dir_num--;
                float leave_rate = 0.2f;
                float stay_rate = 1f - leave_rate * dir_num;
                heatmap[i, j] += stay_rate * oldheatmap[i, j];
                for(int k = 0; k < 4; k++){
                    if(i + dir_x[k] < 0 || i + dir_x[k] >= 5)continue;
                    if(j + dir_y[k] < 0 || j + dir_y[k] >= 5)continue;
                    heatmap[i + dir_x[k], j + dir_y[k]] += oldheatmap[i, j] * leave_rate;
                }
            }
        }
        float heat_sum = 0f;
        for(int i = 0; i < 5; i++){
            for(int j = 0; j < 5; j++){
                heat_sum += heatmap[i,j];
            }
        }
        // Debug.Log(" total1: " + heat_sum);
        if(CanSeePlayer(1.3f)){
            for(int i = 0; i < 5; i++){
                for(int j = 0; j < 5; j++){
                    heatmap[i,j] = 0.0001f;
                }
            }
            Vector3 player_idx = now_idx + dir_n[0];
            for(int i = 0; i < 4; i++){
                if((now_idx + dir_n[i]).x < 0 || (now_idx + dir_n[i]).x >= 5 || (now_idx + dir_n[i]).y < 0 || (now_idx + dir_n[i]).y >= 5)continue;
                if(Vector3.Distance(Intersections(player_idx), player.transform.position) > Vector3.Distance(Intersections(now_idx + dir_n[i]), player.transform.position) ){
                    player_idx = now_idx + dir_n[i];
                }
            }
            heatmap[(int)player_idx.x, (int)player_idx.y] = 0.9976f;
        }else{
            float sum = heatmap[(int)now_idx.x, (int)now_idx.y] * 0.99f;
            heat_sum -= heatmap[(int)now_idx.x, (int)now_idx.y];
            for(int i = 0; i < 4; i++){
                int x = (int)now_idx.x + dir_x[i], y = (int)now_idx.y + dir_y[i];
                if(x < 0 || x >= 5 || y < 0 || y >= 5)continue;
                sum += heatmap[x, y] * 0.3f;
                heat_sum -= heatmap[x, y];
            }
            for(int i = 0; i < 5; i++){
                for(int j = 0; j < 5; j++){
                    oldheatmap[i, j] = heatmap[i, j];
                    heatmap[i, j] = 0f;
                }
            }
            for(int i = 0; i < 5; i++){
                for(int j = 0; j < 5; j++){
                    heatmap[i, j] = oldheatmap[i, j] * (heat_sum + sum) / heat_sum;
                }
            }

            for(int i = 0; i < 4; i++){
                int x = (int)now_idx.x + dir_x[i], y = (int)now_idx.y + dir_y[i];
                if(x < 0 || x >= 5 || y < 0 || y >= 5)continue;
                heatmap[x, y] = oldheatmap[x, y] * 0.7f;
            }
            heatmap[(int)now_idx.x, (int)now_idx.y] = oldheatmap[(int)now_idx.x, (int)now_idx.y] * 0.01f;        
        }
        // heat_sum = 0f;
        // for(int i = 0; i < 5; i++){
        //     for(int j = 0; j < 5; j++){
        //         heat_sum += heatmap[i,j];
        //     }
        // }
        // Debug.Log(" total2: " + heat_sum);
        // for(int i = 0; i < 5; i++){
        //     for(int j = 0; j < 5; j++){

        //         Debug.Log(i + " " + j + " " +heatmap[i, j]);
        //     }
        // }
    }
}