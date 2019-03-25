/*FileName : Trigger
 * 오류 수정한 부분 : 닿지 않았는데도 충돌반응이 일어남 >> Collider를 캡슐모양으로 해놔서 그랬음
 * 기타 수정할 부분 : 게임시작 종료 담당 스크립트를 따로 짜놓고 라인에 닿았을 경우, 시간이 없을경우 모두 종료
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarTrigger : MonoBehaviour
{
    GameObject line;
    Collider lineC;
    Rigidbody a;
    // Use this for initialization
    void Start()
    {
        a = gameObject.GetComponent<Rigidbody>();
        //a.centerOfMass = new Vector3(0, -0.2f, 0);
        a.freezeRotation= true;
        line = GameObject.Find("line");
        lineC = line.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {      

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.TrimEnd() == "checkP" && TimerScript.StartState)
        {//도착 전 체크에 차가 닿으면 방어막 없애줌
            lineC.isTrigger = true;
        }
        else if (other.gameObject.tag == "boost")
        {
            if (this.gameObject.name == "player")
            {//플레이어가 먹었을 때만 갯수 증가
                moveCtrl.boostCnt += 1; //부스터 개수 증가해주기
                Text bscnt = GameObject.Find("bscount").GetComponent<Text>();
                bscnt.text = moveCtrl.boostCnt + "/3";
                Destroy(other.gameObject);

                SoundManager.instance.Play(5);
            }     
        }
    }

}

