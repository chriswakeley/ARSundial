using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatitudeLongitudeVisBehavior : MonoBehaviour {
    private GameObject latitudeVis;
    private GameObject longitudeVis;

    public float width;
    public int numSegments;
    public float scale;

	// Use this for initialization
	void Start () {
        latitudeVis = this.transform.Find("LatitudeVis").gameObject;
        longitudeVis = this.transform.Find("LongitudeVis").gameObject;

        longitudeVis.GetComponent<LineRenderer>().positionCount = numSegments+1;
        longitudeVis.GetComponent<LineRenderer>().startWidth = width;
        longitudeVis.GetComponent<LineRenderer>().endWidth = width;
        longitudeVis.GetComponent<LineRenderer>().positionCount = numSegments + 1;
        longitudeVis.GetComponent<LineRenderer>().startWidth = width;
        longitudeVis.GetComponent<LineRenderer>().endWidth = width;

        for (int i = 0; i <= numSegments; i++)
        {
            longitudeVis.GetComponent<LineRenderer>().SetPosition(i, new Vector3(0f, Mathf.Sin(2 * Mathf.PI / (numSegments) * i) * scale, Mathf.Cos(2 * Mathf.PI / (numSegments)* i) * scale));
        }



    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setLatitudeLongitude(float lat, float lon)
    {

    }
}
