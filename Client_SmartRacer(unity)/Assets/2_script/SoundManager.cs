using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour {
    AudioSource[] mySound=new AudioSource[7]; //audioSorce 컴포넌트를 변수로
    public AudioClip[] clip = new AudioClip[7];
    public static SoundManager instance; //자기자신을 변수로

    /*
     * 0-엔진
     * 1-드리프트
     * 2-부스터
     * 3-엔진오프
     * 4-스타트
     * 5-아이템획득
     * 6-타임오버
     */

    void Awake()
    {
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
        }
        for (int i = 0; i < 7; i++)
        {
            mySound[i] = gameObject.AddComponent<AudioSource>();
            mySound[i].Stop();
        }
        
        for (int j = 0; j < 7; j++)
        {
            mySound[j].clip = clip[j];
            mySound[j].loop = false;
            mySound[j].playOnAwake = false;
        }

        /*볼륨조절*/
        mySound[0].volume = 0.7f;
        mySound[4].volume = 0.5f;
        mySound[6].volume = 0.3f;
    }

    // Use this for initialization
    public void Play(int index)
    {
        mySound[index].PlayOneShot(clip[index]);
    }

    public bool isPlay(int index)
    {
        if (mySound[index].isPlaying)
            return true;
        return false;
    }
    public void Stop(int index)
    {
        mySound[index].Stop();
    }
    // Update is called once per frame
	void Update () {
		
	}
}
