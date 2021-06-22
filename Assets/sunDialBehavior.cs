using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunDialBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setAngle(float latitude)
    {
        this.transform.localEulerAngles = new Vector3(-latitude, 0f, 0f);
    }
}
