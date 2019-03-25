using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whellCtrl : MonoBehaviour {

    void Start()
    {
        //gameObject.GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -9, 0);
    }
	
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(10.0f,0.0f,0.0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Rotate(-10.0f, 0.0f, 0.0f);
        }
	}
}
