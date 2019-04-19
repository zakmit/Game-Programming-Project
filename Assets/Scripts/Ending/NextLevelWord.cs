using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelWord : MonoBehaviour {
    bool ending_flag, click;
    public GameObject Click_Scene;
    public GameObject Next_Menu;
    public GameObject Ending_Menu;
    public AudioSource Next_BGM;
    public AudioSource End_BGM;
    public Text textBox;
    EndingPlayer PL;
    string[] goatText = new string[] { "恭喜\n你成功活過了今天\n但是，你和病嬌女友之間的戰爭仍在繼續。\n直到她厭倦你跟你分手為止，你都只能偷偷吸取妹子能量以活下去。",
    "接下來的路，只會越來越艱難", "要繼續嗎" };
    string[] EndingText = new string[] { "恭喜，病嬌女友因為受不了你的冷漠，跟你分手了。", "現在開始，你可以盡情的喇妹子了！"};
    int currentlyDisplayingText = 0;
    public AudioSource Beep;

    void Awake(){
        click = false;
        ending_flag = false;
        goatText[2] = "要繼續嗎" + "("+ LevelsCount.CurrentLevel.ToString("00") + "/" + LevelsCount.TotalLevel.ToString("00") + ")";
        PL = FindObjectOfType<EndingPlayer>();
        if (PL.ending)
        {
            goatText = EndingText;
            End_BGM.Play();
        }
        else
            Next_BGM.Play();
        Beep.Play();
        StartCoroutine(AnimateText(0.2f));

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SkipToNextText();
        }
    }
    public void SkipToNextText()
    {
        if (ending_flag)
        {
            if (PL.ending)
            {
                Ending_Menu.SetActive(true);
            }
            else
            {
                Click_Scene.SetActive(false);
                Next_Menu.SetActive(true);
            }
        }
        if (!click)
        {
            StopAllCoroutines();
            if (currentlyDisplayingText == goatText.Length)
            {
                Beep.Stop();
                ending_flag = true;
            }
            else
            {
                textBox.text = goatText[currentlyDisplayingText];
                click = true;
            }
        }
        else
        {
            click = false;
            currentlyDisplayingText++;
            StartCoroutine(AnimateText(0.2f));
        }
    }
    IEnumerator AnimateText(float gap)
    {

        for (int i = 0; i < (goatText[currentlyDisplayingText].Length); i++)
        {

            textBox.text = goatText[currentlyDisplayingText].Substring(0, i + 1);


            yield return new WaitForSeconds(gap);

        }
        yield return new WaitForSeconds(0.4f);
        currentlyDisplayingText++;
        if (currentlyDisplayingText < goatText.Length)
        {
            print("go");
            StartCoroutine(AnimateText(gap));
        }
        else
        {
            print("in else");
            Beep.Stop();
            ending_flag = true;
        }
    }
}
