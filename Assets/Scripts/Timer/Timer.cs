using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {
    public string SceneName;
    public float LevelTime = 180.0f;
    public Text Minutes;
    public Text Seconds;
    public Text Dot;
    // Update is called once per frame
    void Update () {
        LevelTime -= Time.deltaTime;
        Minutes.text = ((int) (LevelTime / 60) ).ToString("00");
        Seconds.text = ((int) (LevelTime % 60) ).ToString("00");
        if (LevelTime % 1 > 0.5)
            Dot.text = ":";
        else
            Dot.text = " ";
        if (LevelTime <= 0f)
        {
            timerEnded();
        }
    }
    void timerEnded()
    {
        SceneManager.LoadScene(SceneName);
    }
}
