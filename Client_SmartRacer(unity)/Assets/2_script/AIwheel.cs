using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIwheel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (TimerScript.StartState)
            transform.Rotate(10.0f, 0.0f, 0.0f);
	}
}
