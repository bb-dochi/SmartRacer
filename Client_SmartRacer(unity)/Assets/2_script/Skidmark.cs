using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skidmark : MonoBehaviour {
    TrailRenderer t;
    ParticleSystem p1, p2;
    ParticleSystem.EmissionModule em1, em2;
	// Use this for initialization
	void Start () {
        t = gameObject.GetComponent<TrailRenderer>();
        t.enabled = false;

        p1 = GameObject.Find("lpaticle").GetComponent<ParticleSystem>();
        p2 = GameObject.Find("rpaticle").GetComponent<ParticleSystem>();

        em1 = p1.emission;
        em2 = p2.emission;

	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.name.Equals("LeftSkid") || gameObject.name.Equals("RightSkid"))
        {
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E) || moveCtrl.DriftOnOff)
                t.enabled = true;
            else
                t.enabled = false;
        }
        else if (gameObject.name.Equals("LBoost") || gameObject.name.Equals("RBoost"))
        {
            if (moveCtrl.BoostState)
            {
                if(t!=null) t.enabled = true;
                if(p1 != null) em1.enabled = true;
                if (p2 != null) em2.enabled = true;
            }
            else
            {
                if (t != null) t.enabled = false;
                if (p1 != null) em1.enabled = false;
                if (p2 != null) em2.enabled = false;
            }
        }
	}
}
