/*FileName : Trigger
 * 오류 수정한 부분 : 닿지 않았는데도 충돌반응이 일어남 >> Collider를 캡슐모양으로 해놔서 그랬음
 * 기타 수정할 부분 : 게임시작 종료 담당 스크립트를 따로 짜놓고 라인에 닿았을 경우, 시간이 없을경우 모두 종료
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Trigger : MonoBehaviour {
    Text finishText;
    int count = 0;
    Renderer rend;
    
	// Use this for initialization
	void Start () {
        rend = GameObject.Find("startAndfinish").GetComponent<Renderer>();
        rend.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {  

	}

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Car" && TimerScript.StartState)
        {
            if (count <2)
            {
                Material mt = Resources.Load("Textures/finishline_m3", typeof(Material)) as Material;
                rend.sharedMaterial = mt;
                count++;
                Invoke("noback", 1);
                //처음 닿으면 시작라인을 끝라인으로 재질 바꿔주고 뒤로 못가게하기
            }
            else
            {                
                finishText = GameObject.Find("StartCount").GetComponent<Text>();
                SoundManager.instance.Play(6);
                if (other.gameObject.name.Substring(0, 6).TrimEnd() == "player")
                {
                    finishText.text = "Player Win !";
                    SM.winner = 1;
                    SM.prev = SceneManager.GetActiveScene().buildIndex;
                    Invoke("finishScene", 3);
                }
                else
                {
                    finishText.text = "Player Lose !";                   
                    SM.winner = 2;
                    SM.prev = SceneManager.GetActiveScene().buildIndex;
                    Invoke("finishScene", 3);
                }               
                TimerScript.StartState = false;
            }
        }
        else
        {
            //Debug.Log("충돌객체:" + other.gameObject.tag);
        }
    }
    private void noback()
    {
        Collider lineC = GameObject.Find("line").GetComponent<Collider>();
        lineC.isTrigger = false;
    }
    private void finishScene()
    {
        SM.finishScene();
    }
}
