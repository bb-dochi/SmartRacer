using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AImove : MonoBehaviour
{
    public GameObject Marker;
    public string StagePath="map1_path";
    public float time;
    public int n=0;
    bool AiState = true;
    Vector3 Pre;
    Vector3 Now;
    Vector3 relativePos;
    void Start()
    {
       /* switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 2:
                time = 55f;
                break;
            case 3:
                time = 110f;
                break;
            case 4:
                time = 180f;
                break;
        }*/
        if (SceneManager.GetActiveScene().buildIndex == 2) //첫번째 맵
            StagePath = "map1_path";
        else if (SceneManager.GetActiveScene().buildIndex == 3) //두번째 맵
            StagePath = "map2_path";

    }
    // Update is called once per frame
    void Update()
    {
        if (TimerScript.StartState && AiState)
        {
            iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(StagePath), "easeType", iTween.EaseType.linear, "time", time, "orienttopath", true));
            AiState = false;
        }
        Marker.transform.position = new Vector3(this.transform.position.x, Marker.transform.position.y, this.transform.position.z);
        Marker.transform.forward = this.transform.forward;
    }

    void LateUpdate()
    {
        if (TimerScript.StartState)
        {
            Now = this.transform.position;  //현재 위치
            Pre = iTweenPath.GetPath(StagePath)[n]; //다음 노드의 위치
            if ((Pre.x - 2.0f < Now.x && Now.x < Pre.x + 2.0f) && n < iTweenPath.GetPath(StagePath).Length - 1)
                n++;

            relativePos = Pre - Now; //현재 위치 - 다음 위치
            //relativePos = (Pre - Now).normalized; //현재 위치 - 다음 위치

            Quaternion To = Quaternion.LookRotation(relativePos); //타켓의 rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, To, Time.deltaTime);
        }
        else
        {
            iTween.Stop();
        }
    }
}
