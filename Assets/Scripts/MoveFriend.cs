using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFriend : MonoBehaviour {
    public int Dot_Number;
    Big_Map_Control BigMap;
    public FriendWalk[] Friends;
    void Start()
    {
        BigMap = FindObjectOfType<Big_Map_Control>();
    }
    public void SetDotNumber(int n)
    {
        Dot_Number = n;
    }
    public void SetDotX(int x)
    {
        Friends[Dot_Number].SetGoalX( x * BigMap.road_width - 18f);
    }
    public void SetDotY(int y)
    {
        Friends[Dot_Number].SetGoalY( y * BigMap.road_width - 5f);
    }
}
