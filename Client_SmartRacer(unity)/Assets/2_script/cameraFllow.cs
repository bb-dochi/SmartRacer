using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFllow : MonoBehaviour {

    public Transform player;
    public float dist = 10.0f;
    public float height = 5.0f;
    public float smoothRotate = 5.0f;

    private Transform tr;
	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
	}

	void LateUpdate () {
	//게임의 모든 update로직 마친 뒤 실행하는 마지막 update사이클
        float currYAngle = Mathf.LerpAngle(tr.eulerAngles.y, player.eulerAngles.y, smoothRotate * Time.deltaTime);
        //카메라의 y축에 대한 오일러 각도 부터 타겟의 각도까지 지정한 시간까지 회전하는 것
  
        Quaternion rot = Quaternion.Euler(0, currYAngle, 0);
        //쿼터니언으로 3개의 축을 한번에 회전

        tr.position = player.position - (rot * Vector3.forward * dist) + (Vector3.up * height);
        //타겟의 포지션 값 - (타겟의 회전을 따라잡는 rot 변수 * 앞 방향 * 띄울 거리) - (윗 방향 * 띄울 거리)
        tr.LookAt(player);
	}
}
