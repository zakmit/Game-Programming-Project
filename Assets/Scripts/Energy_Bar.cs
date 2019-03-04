using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy_Bar : MonoBehaviour {
    Vector2 pos = new Vector2(10, 10);
    Vector2 size = new Vector2(200, 16);

    float barFill = 0.8f;
    string barText = "Health";

    public GUIStyle progress_empty;
    public GUIStyle progress_full;
    
    void Start(){
        Debug.Log("hi");
    }
    void OnGUI () {
        // Create a group container to make positioning easier
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
        
        // Draw the background and fill
        GUI.Box(new Rect(0, 0, size.x, size.y), barText, progress_empty);
        GUI.Box(new Rect(0, 0, size.x * barFill, size.y), barText, progress_full);
        
        GUI.EndGroup();
    }

    void Update () {
        // Poll the player's health from stats object here
        // barFill = 8 / 10;
    }
}