using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Text;
using UnityEngine.UI;


public class PostboxCar
{
    private static PostboxCar instance;
    public static PostboxCar GetInstance
    {
        get
        {
            if (instance == null)
                instance = new PostboxCar();
            return instance;
        }
    }

    private Queue<string> GoQueue;
    private Queue<string> SensorQueue;
    private Queue<string> DritfQueue;
    private Queue<string> BoostQueue;
    private PostboxCar()
    {
        GoQueue = new Queue<string>();
        SensorQueue = new Queue<string>();
        DritfQueue = new Queue<string>();
        BoostQueue = new Queue<string>();
    }

    public void PushData(string data)
    {
        if (data.Substring(0, 1) == "1")//센서
            SensorQueue.Enqueue(data);
        else if (data.Substring(0, 1) == "2") //고
            GoQueue.Enqueue(data);
        else if (data.Substring(0, 1) == "3") //드리프트
            DritfQueue.Enqueue(data);
        else if (data.Substring(0, 1) == "4") //부스터
            BoostQueue.Enqueue(data);
        else
            Debug.Log("?");
    }
    public string GetData(int num)
    {
        if (num == 1)
        {
            if (SensorQueue.Count > 0)
                return SensorQueue.Dequeue();
            else
                return string.Empty;
        }
        else if (num == 2)
        {
            if (GoQueue.Count > 0)
                return GoQueue.Dequeue();
            else
                return string.Empty;
        }
        else if (num == 3)
        {
            if (DritfQueue.Count > 0)
                return DritfQueue.Dequeue();
            else
                return string.Empty;
        }
        else if (num == 4)
        {
            if (BoostQueue.Count > 0)
                return BoostQueue.Dequeue();
            else
                return string.Empty;
        }
        return string.Empty;
    }
}

public class moveCtrl : MonoBehaviour
{

    Thread t = null;
    TcpClient client = null;
    public GameObject Marker;
    GameObject needle;
    public float speed = 10.0f; //속도  
    float boostTime = 0.0f;
    float boostSpeed = 0.0f;   
    float rotateSpeed = 0.0f;
    float speedTime = 0.0f; //가속도 관련 시간변수
    float recover = 0; //차 뒤집힘 관리

    bool soundState = true;
    public static bool DriftOnOff = false;
    public static bool BoostState = false; 
    public static int boostCnt = 0;

    void Awake()
    {
        t = new Thread(ListenSocket);
        t.Start();
        
    }
    void Start()
    {
        speedTime = 0.0f;
        speed = 10.0f;
        boostCnt = 0;
        needle = GameObject.Find("needle");
        Mathf.Clamp(this.transform.rotation.y, -90, 90.0f);
    }

    void ListenSocket()
    {
        try
        {
            client = new TcpClient();
            client.Connect("localhost", 9001);
            Debug.Log("연결되었습니다");
            while (true)
            {
                NetworkStream stream = client.GetStream();
                byte[] buf = new byte[1024];
                int count = stream.Read(buf, 0, buf.Length);
                string msg = Encoding.Default.GetString(buf, 0, count);

                EnQueing(msg);

            }
        }
        catch (Exception e)
        {
            Debug.Log("error:" + e.Message);
        }
    }

    void EnQueing(string data)
    {
        PostboxCar.GetInstance.PushData(data);
    }
    

    IEnumerator RotateFunc(string sensor)
    {
        if (sensor != string.Empty)
        {
            float sensorf = float.Parse(sensor.Substring(2, 4));
             if (sensorf < -1.0f)
                iTween.RotateBy(gameObject, iTween.Hash("y", -2, "easeType", iTween.EaseType.linear, "time",25.0f-rotateSpeed));
            else if (sensorf > 1.5)
                iTween.RotateBy(gameObject, iTween.Hash("y", 2, "easeType", iTween.EaseType.linear, "time", 25.0f -rotateSpeed));
            else
            {
                iTween.Stop(gameObject);
            }
        }
        yield return null;
    }

    string Gostate = "T";
    IEnumerator GoFunc(string go)
    {
        if (go != string.Empty)
        {
            Gostate = go.Substring(2, 4).TrimEnd();
        }
        if (Gostate == "T") {}
        if (Gostate=="Go")
        {
            iTween.MoveBy(gameObject, iTween.Hash("z", 50, "easeType", iTween.EaseType.linear, "speed",speed+boostSpeed));

            if (DriftOnOff == false)
            {
                speedTime += 0.5f;
                int temp = Mathf.FloorToInt(speedTime);
                if (speed < 30 && temp % 5 == 0)
                {
                    speed += 0.3f;
                }
                if (speed > 30) speed = 30;
            }
            if(!SoundManager.instance.isPlay(0))
                SoundManager.instance.Play(0);
            soundState = true;


           
        }
        else if(Gostate=="ST")
        {
            iTween.MoveBy(gameObject, iTween.Hash("z", 50, "easeType", iTween.EaseType.linear,"speed",speed+boostSpeed-10));
            speedTime -=0.5f;
            int temp = Mathf.FloorToInt(speedTime);
            if(speed>10 && temp%5==0)
            {
                speed -=0.3f;
            }
            if(speed <10) speed = 10;
            if (SoundManager.instance.isPlay(0))
                SoundManager.instance.Stop(0);
            if (soundState)
            {
                SoundManager.instance.Play(3);
                soundState = false;
            }
        }
        yield return null;
           
    }

    string drState = "T";
    IEnumerator DriftFunc(string dr)
    {
        if (dr != string.Empty)
        {
            drState = dr.Substring(2, 2).TrimEnd();
        }
        if (drState == "T") { /*시작 상태*/}

        if (drState == "Dr")
        {
            if (!SoundManager.instance.isPlay(1))
                SoundManager.instance.Play(1);

            DriftOnOff = true;
            speedTime -= 0.5f;
            int temp = Mathf.FloorToInt(speedTime);
            if (speed > 10 && temp % 5 == 0)
            {
                speed -= 1.0f;
                if(rotateSpeed <10)
                    rotateSpeed += 1.0f;               
            }
            
            
        }
        else if (drState == "ND")
        {
            if (SoundManager.instance.isPlay(1))
                SoundManager.instance.Stop(1);
            DriftOnOff = false;  
            if(speed!=10)
            speed += 5.0f; //드리프트 이후 속도 순간 증감
            if (speed > 30)
                speed = 30;
            rotateSpeed = 0;

            drState = "T";   
        }
        yield return null;

    }

    
    IEnumerator BoostFunc(string bo)
    {    
        string message_b = null;
        if (bo != string.Empty)
        {
            message_b = bo.Substring(2, 2).TrimEnd();
            if (message_b == "Bo" && !BoostState && boostCnt>0)
            {
                boostTime = 0;
                boostSpeed += 10f;
                BoostState = true; //부스트 실행
                boostCnt -= 1;
                Text bscnt = GameObject.Find("bscount").GetComponent<Text>();
                bscnt.text = boostCnt + "/3";

                if (!SoundManager.instance.isPlay(2))
                    SoundManager.instance.Play(2);
            }
        }
        else if(boostTime < 10)
        {
            boostTime += Time.deltaTime;
            if (boostTime > 7)
            {
                boostSpeed = 0;
                BoostState = false;
            }
        }
        yield return null;
    }

    void Update()
    {
        
        
       
    }

    void LateUpdate()
    {
        string sensor = PostboxCar.GetInstance.GetData(1);
        string go = PostboxCar.GetInstance.GetData(2);
        string drift = PostboxCar.GetInstance.GetData(3);
        string boost = PostboxCar.GetInstance.GetData(4);
        
        if (TimerScript.StartState)
        //5초 카운트 전엔 못 움직이게 
        {
            StartCoroutine(RotateFunc(sensor));
            StartCoroutine(GoFunc(go));
            StartCoroutine(DriftFunc(drift));
            StartCoroutine(BoostFunc(boost));
            //코루틴으로 안해도 될거 같기도..?


            /*--------------------------키보드-------------------------*/
            if (Input.GetKey(KeyCode.W))
            {
                this.transform.Translate(Vector3.back * 30.0f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                this.transform.Translate(Vector3.forward * 30.0f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                this.transform.Translate(Vector3.left * 20 * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                this.transform.Translate(Vector3.right * 20 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                this.transform.Rotate(0.0f, -20.0f * Time.deltaTime, 0.0f);
            }

            if (Input.GetKey(KeyCode.E))
            {
                this.transform.Rotate(0.0f, 20.0f * Time.deltaTime, 0.0f);
            }
            /*--------------------------------------------------------*/


            //뒤집어진 상태로 4초 유지 시 제자리 복구.  **델타타임 초기값이 왜 10인지? //210-150은 아예 뒤집어졌을 경우 
            if ((280 > this.transform.eulerAngles.z && this.transform.eulerAngles.z > 70)||Input.GetKey(KeyCode.F))
            {
                recover += Time.deltaTime;
                if (recover > 3)
                {
                    this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x, this.transform.eulerAngles.y, 0);
                    this.transform.position = new Vector3(this.transform.position.x, 3.4f, this.transform.position.z);
                    recover = 0;
                }
            }
            //미니맵 Marker이동
            Marker.transform.position = new Vector3(this.transform.position.x, Marker.transform.position.y, this.transform.position.z);
            Marker.transform.forward = this.transform.forward;

            needle.transform.rotation = Quaternion.Euler(needle.transform.rotation.x, needle.transform.rotation.y, 33-((speed-10)*8)-boostSpeed*3);
            //needle.transform.RotateAround(GameObject.Find("point").transform.position, new Vector3(needle.transform.rotation.x, needle.transform.rotation.y, needle.transform.rotation.z - (speed - 10) * 2),Time.deltaTime);
        }
    }
}

