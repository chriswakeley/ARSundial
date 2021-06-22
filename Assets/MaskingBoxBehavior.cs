using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskingBoxBehavior : MonoBehaviour {

    private float StartXLength, StartZLength;

    public float FadeTime;

	// Use this for initialization
	void Start () {
        StartXLength = this.gameObject.transform.localScale.x;
        StartZLength = this.gameObject.transform.localScale.z;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator Fade()
    {
        Vector3 currentScale = this.gameObject.transform.localScale;
        currentScale.x = 0f;
        //currentScale.z = 0f;
        float currentTime = 0f;
        this.gameObject.transform.localScale = currentScale;

        while(!Mathf.Approximately(currentScale.x, StartXLength)){
            currentTime += Time.deltaTime;
            currentScale.x = Mathf.Lerp(currentScale.x, StartXLength, currentTime / FadeTime);
            //currentScale.z = Mathf.Lerp(currentScale.z, StartZLength, currentTime / FadeTime);
            this.gameObject.transform.localScale = currentScale;
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        Vector3 currentScale = this.gameObject.transform.localScale;
        //currentScale.z = 0f;
        float currentTime = 0f;

        while (!Mathf.Approximately(currentScale.x, 0f))
        {
            currentTime += Time.deltaTime;
            currentScale.x = Mathf.Lerp(currentScale.x, 0f, currentTime / FadeTime);
            //currentScale.z = Mathf.Lerp(currentScale.z, StartZLength, currentTime / FadeTime);
            this.gameObject.transform.localScale = new Vector3(currentScale.x, currentScale.x, currentScale.x);
            yield return null;
        }
    }
}
