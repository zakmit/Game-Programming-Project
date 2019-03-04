using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadSceneOnClick : MonoBehaviour {

    // Use this for initialization
    public void LoadByName(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void LoadByName_Add(string SceneName2)
    {
        SceneManager.LoadSceneAsync(SceneName2, LoadSceneMode.Additive);
    }
    public void LevelCountPlus(int i)
    {
        LevelsCount.CurrentLevel += i;
    }
    public void LevelCounSet(int i)
    {
        LevelsCount.CurrentLevel = i;
    }
}
