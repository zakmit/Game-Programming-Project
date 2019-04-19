using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour {
    public GameObject Menu;
    public GameObject BigMap;
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.SetActive(true);
            Time.timeScale = 0f;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            BigMap.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
