using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    Player_Control PL;
    Yuno_Control Yuno;
	// Use this for initialization
	void Start () {
        PL = FindObjectOfType<Player_Control>();
        Yuno = FindObjectOfType<Yuno_Control>();
        PL.dicrease *= 1 + (LevelsCount.CurrentLevel - 1)*0.4f;
        PL.full_bonus *= 1 + LevelsCount.CurrentLevel * 0.3f;
        PL.LevelBonus *= 1 + (LevelsCount.CurrentLevel - 1) * 0.1f;
        Yuno.speed += Yuno.speed + (LevelsCount.CurrentLevel - 1) * 0.3f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
