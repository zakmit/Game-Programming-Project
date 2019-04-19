using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLevelCount : MonoBehaviour {

	// Use this for initialization
	public void SetCount(int value)
    {
        LevelsCount.CurrentLevel = value;
        print(LevelsCount.CurrentLevel);
    }
}
