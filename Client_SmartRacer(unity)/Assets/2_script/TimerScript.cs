using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{

    public static float time;
    public static float Startcount;
    public static bool StartState = false;
    Text startText;
    Text timeText;

    int sNum;
    // Use this for initialization

    void Start()
    {
        Startcount = 4f;
        sNum=SceneManager.GetActiveScene().buildIndex;
        switch (sNum)
        {
            case 2:
                time = 60f;
                break;
            case 3:
                time = 100f;
                break;
            case 4:
                time = 180f;
                break;
        }

        startText = GameObject.Find("StartCount").GetComponent<Text>();
        timeText = GameObject.Find("timer").GetComponent<Text>();
        SoundManager.instance.Play(4);
        //SoundManager.instance.PlaySound("startSound");
        StartCoroutine(CountTime()); //카운트다운
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator CountTime()
    {
        while (Startcount>0)
        {
            Startcount -= 1;
            startText.text = "" + Startcount;
            if(Startcount == 0)
                startText.text = "GAME START";
            yield return new WaitForSeconds(1);
        }
        StartState = true;
        StartCoroutine(Time());
        startText.text = "";
    }

    IEnumerator Time()
    {
        int h = (int)time / 60;
        int m = (int)time % 60;
        while (time > 0)
        {
            string m_str = m.ToString();
            if (m < 10) m_str = "0" + m_str;
            timeText.text = "0" + h + ":" + m_str + ":00";

            time -= 1;         
            m -= 1;
            if (m <= 0 && h>0)
            {
                h -= 1;
                m = 59;
            }          
            yield return new WaitForSeconds(1);
        }
        startText.text = "Time Out!!";
        SM.winner = 0;
        SM.prev = sNum;
        Invoke("finishScene", 3);
        StartState = false;

        SoundManager.instance.Play(6);

    } 

    private void finishScene()
    {
        SM.finishScene();
    }
}
