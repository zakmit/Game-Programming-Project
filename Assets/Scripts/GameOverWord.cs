using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWord : MonoBehaviour {
    bool click;
    bool ending_flag;
    bool gameover;
    public Text textBox;
    public GameObject GameOverText;
    public Sprite[] Ending_Sprites;
    public AudioSource stab;
    public AudioSource Beep;
    public GameObject Click_Scene;
    public GameObject Continue;
    public SpriteRenderer Ending_Background;
    string[] goatText = new string[] { "啊呀 親愛的\n你明明已經有我了，為什麼還要去找那邊那個女人呢...\n真是貪心呢... 你只要有我，我也只要有你就夠了....",
    "\n來吧，「永遠的」和我在一起吧..." };
    int currentlyDisplayingText = 0;
    void Awake()
    {
        gameover = false;
        ending_flag = false;
        click = false;
        StartCoroutine(AnimateText(0.3f));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SkipToNextText();
        }
    }
    //This is a function for a button you press to skip to the next text
    public void SkipToNextText()
    {
        if (gameover)
        { //kill button, open menu.
            Click_Scene.SetActive(false);
            Continue.SetActive(true);
        }
        else
        {
            if (!click)
            {
                StopAllCoroutines();
                if (ending_flag)
                {
                    Ending_Background.sprite = Ending_Sprites[2];
                    GameOverText.SetActive(true);
                    gameover = true;
                }
                textBox.text = goatText[currentlyDisplayingText];
                click = true;
            }
            else
            {
                ending(0.7f);
            }
        }
    }
    //Note that the speed you want the typewriter effect to be going at is the yield waitforseconds (in my case it's 1 letter for every      0.03 seconds, replace this with a public float if you want to experiment with speed in from the editor)
    IEnumerator AnimateText(float gap)
    {

        for (int i = 0; i < (goatText[currentlyDisplayingText].Length); i++)
        {
            if (goatText[currentlyDisplayingText][i] != ' ' && goatText[currentlyDisplayingText][i] != '.' && goatText[currentlyDisplayingText][i] != '，' &&
            goatText[currentlyDisplayingText][i] != '\n' && goatText[currentlyDisplayingText][i] != '「' && goatText[currentlyDisplayingText][i] != '」')
            {
                Beep.Play();
            }
            textBox.text = goatText[currentlyDisplayingText].Substring(0, i + 1);


            yield return new WaitForSeconds(gap);
            if (ending_flag)
            {
                if(i == 2)
                {
                    stab.Play();
                    yield return new WaitForSeconds(0.2f);
                    Ending_Background.sprite = Ending_Sprites[0];
                }
                if(i == 7)
                {
                    stab.Play();
                    yield return new WaitForSeconds(0.2f);
                    Ending_Background.sprite = Ending_Sprites[1];
                }
                if (i == 14)
                {
                    stab.Play();
                    yield return new WaitForSeconds(0.3f);
                    stab.Play();
                    yield return new WaitForSeconds(0.2f);
                    Ending_Background.sprite = Ending_Sprites[2];
                }
                if (i == 16)
                {
                    GameOverText.SetActive(true);
                    gameover = true;
                }
            }
        }
        ending(0.7f);
    }
    void ending(float gap)
    {
        ending_flag = true;
        click = false;
        currentlyDisplayingText++;
        if (currentlyDisplayingText <= goatText.Length)
        {
            print("go");
            StartCoroutine(AnimateText(gap));
        }
    }
}
