using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendWalk : MonoBehaviour {
    float GoalX;
    float GoalY;
    bool WalkX, WalkY;
    public void SetGoalX(float x)
    { GoalX = x; }
    public void SetGoalY(float y)
    { GoalY = y; }
    public float speed = 3f;
    // Use this for initialization
    void Start () {
        WalkX = false;
        WalkY = false;
        GoalX = transform.position.x;
        GoalY = transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        if (!WalkX && !WalkY)
        {
            if (System.Math.Abs(transform.position.x - GoalX) > 0.1f)
            {
                WalkX = true;
            }
            else if (System.Math.Abs(transform.position.y - GoalY) > 0.1f)
            {
                WalkY = true;
            }
        }
        if(WalkX)
        { WalkToGoalX(); }
        if(WalkY)
        { WalkToGoalY(); }
    }
    void WalkToGoalX()
    {
        print("WalkX");
        if(transform.position.x - GoalX > 0)
            transform.position += Vector3.left * speed * Time.deltaTime;
        if (transform.position.x - GoalX < 0)
            transform.position += Vector3.right * speed * Time.deltaTime;
        if (System.Math.Abs(transform.position.x - GoalX) < 0.1f)
        {
            WalkX = false;
        }
    }
    void WalkToGoalY()
    {
        print("WalkY");
        if (transform.position.y - GoalY > 0)
            transform.position += Vector3.down * speed * Time.deltaTime;
        if (transform.position.y - GoalY < 0)
            transform.position += Vector3.up * speed * Time.deltaTime;
        if (System.Math.Abs(transform.position.y - GoalY) < 0.1f)
        {
            WalkY = false;
        }
    }
}
