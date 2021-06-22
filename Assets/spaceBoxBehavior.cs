using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceBoxBehavior : MonoBehaviour {
    public GameObject arCamera;
	// Use this for initialization
	void Start () {
        this.gameObject.transform.position = arCamera.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.position = arCamera.transform.position;
    }
}
