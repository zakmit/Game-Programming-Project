using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDrained : MonoBehaviour {
    bool ending_flag, click;
    public GameObject Click_Scene;
    public GameObject Next_Menu;
    public GameObject light_up;
    public GameObject buttom;
    public GameObject PL;
    public GameObject Yuno;
    public GameObject Talk_effect;
    public GameObject Talk_effect_use;
    public int yuno_state;
    public Text textBox;
    string[] goatText = new string[] { "因為缺乏妹子能量，你死了。",
    "親愛的你怎麼變成這樣了", "沒關係，就算你變成這樣，我也會愛著你的喔" };
    int currentlyDisplayingText = 0;
    public AudioSource Beep;
    public AudioSource OpenLight;
    // Use this for initialization
    void Awake () {
        click = false;
        ending_flag = false;
        StartCoroutine(AnimateText(0.3f));
        yuno_state = 0;
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
            Click_Scene.SetActive(false);
            Next_Menu.SetActive(true);
        }
        else
        {
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
                ToNextParagraph(0.3f);
            }
        }
    }

    IEnumerator AnimateText(float gap)
    {
        if(currentlyDisplayingText == 1)
            yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < (goatText[currentlyDisplayingText].Length); i++)
        {
            if(currentlyDisplayingText > 0)
            {
                if (goatText[currentlyDisplayingText][i] != ' ' && goatText[currentlyDisplayingText][i] != '.' && goatText[currentlyDisplayingText][i] != '，' &&
            goatText[currentlyDisplayingText][i] != '\n' && goatText[currentlyDisplayingText][i] != '「' && goatText[currentlyDisplayingText][i] != '」')
                {
                    Beep.Play();
                }
                if(currentlyDisplayingText == 2)
                    if(i == 15)
                    { Talk_effect_use = Instantiate(Talk_effect, Vector3.Lerp(PL.transform.position, Yuno.transform.position, 0.5f), transform.rotation); }
            }
            textBox.text = goatText[currentlyDisplayingText].Substring(0, i + 1);
            yield return new WaitForSeconds(gap);

        }
        yield return new WaitForSeconds(0.4f);
        currentlyDisplayingText++;
        if (currentlyDisplayingText <= goatText.Length)
        {
            print("go");
            ToNextParagraph(gap);
        }
        else
        {
            print("in else");
            ending_flag = true;
        }
    }
    void ToNextParagraph(float gap)
    {

        if (currentlyDisplayingText == 1)//open the light play light sound then make yuno walk
        {
            textBox.text = "";
            light_up.SetActive(true);
            OpenLight.Play();
            Yuno.SetActive(true);
            buttom.SetActive(true);
            PL.SetActive(true);
            yuno_state = 1;
            StartCoroutine(AnimateText(0.2f));
        }
        if (currentlyDisplayingText == 2)//make yuno walk
        {
            textBox.text = "";
            yuno_state = 2;
            StartCoroutine(AnimateText(0.2f));
        }
        if (currentlyDisplayingText == 3)//hide text show menu
        {
            if(Talk_effect_use == null)
                Talk_effect_use = Instantiate(Talk_effect, Vector3.Lerp(PL.transform.position, Yuno.transform.position, 0.5f), transform.rotation);
            ending_flag = true;
        }
    }
}
