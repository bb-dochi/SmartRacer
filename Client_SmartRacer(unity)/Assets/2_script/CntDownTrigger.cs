/*FileName : Trigger
 * 오류 수정한 부분 : 닿지 않았는데도 충돌반응이 일어남 >> Collider를 캡슐모양으로 해놔서 그랬음
 * 기타 수정할 부분 : 게임시작 종료 담당 스크립트를 따로 짜놓고 라인에 닿았을 경우, 시간이 없을경우 모두 종료
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CntDownTrigger : MonoBehaviour
{
    public Transform prefab;

    Renderer rend;
    Text finishText;
    Text track;
    int cnt = 0;
	// Use this for initialization
	void Start () {
        finishText = GameObject.Find("StartCount").GetComponent<Text>();
        track = GameObject.Find("track").GetComponent<Text>();

        rend = GameObject.Find("startAndfinish").GetComponent<Renderer>();
        rend.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {

	}

    void OnTriggerExit(Collider other) 
    {//도착 지점 지나면 1바퀴추가 + 뒷벽다시생성

        if (other.gameObject.tag == "Car" && TimerScript.StartState)
        {
            cnt++;
            if (cnt == 1)
            {
                Material mt = Resources.Load("Textures/finishline_m1", typeof(Material)) as Material;
                rend.sharedMaterial = mt;
                Invoke("noback", 0.5f);   
            }
            else if (cnt < 3)
            {
                GameObject[] tempobj = GameObject.FindGameObjectsWithTag("boost");
                foreach (GameObject ob in tempobj)
                {
                    Destroy(ob); //부스터 전부 삭제
                } 
               
                //부스터 다시 생성
                Instantiate(prefab, new Vector3(95.85f, 36.77f, 130.15f), Quaternion.Euler(90, 0, 242.61f));
                Instantiate(prefab, new Vector3(-160.597f, 29.034f, -38.6f), Quaternion.Euler(90, 90, 90));
                Instantiate(prefab, new Vector3(194.28f, 34.17f, -164.66f), Quaternion.Euler(90, 0, 149.513f));

                Material mt = Resources.Load("Textures/finishline_m"+cnt, typeof(Material)) as Material;
                rend.sharedMaterial = mt;
                finishText.text = "LAP "+(cnt-1);
                track.text = cnt-1 + "/2";
                Invoke("noback", 0.5f);   
            }
            else
            {
                SoundManager.instance.Play(6);
                finishText.text = "Success!";
                track.text = cnt-1 + "/2";
                Debug.Log("도착");
                TimerScript.StartState = false;
                SM.winner = 3;
                SM.prev = SceneManager.GetActiveScene().buildIndex;
                Invoke("finishScene", 3);
            }
        }
        else
        {
            //Debug.Log("충돌객체:" + other.gameObject.tag);
        }      
        
    }

    private void noback()
    {
        finishText.text = " ";
        //Debug.Log("cnt파일의 noback함수");
        Collider lineC = GameObject.Find("line").GetComponent<Collider>();
        lineC.isTrigger = false;
    }

    private void finishScene()
    {
        SM.finishScene();
    }
    
}

