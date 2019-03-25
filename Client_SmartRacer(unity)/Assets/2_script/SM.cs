using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SM : MonoBehaviour {
    public static int winner;
    public static int prev;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeScene(int map)
    {
        SceneManager.LoadScene(map);
    }//인덱스로

    public static void finishScene()
    {
        SceneManager.LoadScene("finish");
    }

    public void restart()
    {
        SceneManager.LoadScene(prev);
    }

}
