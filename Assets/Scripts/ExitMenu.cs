using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour {
    public GameObject Menu;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
