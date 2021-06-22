using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earthBehavior : MonoBehaviour {

    private float size;

    public bool isFading;
    public float FadeTime;
    public bool isSpinning;
    public float spinStartTime;
    public float spinSpeed;

	// Use this for initialization
	void Start () {
        isFading = false;
        isSpinning = false;
        size = this.gameObject.transform.localScale.x;
        this.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
    }
	
	// Update is called once per frame
	void Update () {
        if (isSpinning)
        {
            this.gameObject.transform.Rotate(new Vector3(0f, -spinSpeed * Time.deltaTime, 0f));
        }
	}

    public IEnumerator Fade()
    {
        Vector3 earthScale = new Vector3(0f, 0f, 0f);
        this.gameObject.transform.localScale = earthScale;
        float currentTime = 0f;
        isFading = true;
        float newScaleMagnitude = 0f;
       
        while (!Mathf.Approximately(this.gameObject.transform.localScale.x, size)){
            currentTime += Time.deltaTime;
            newScaleMagnitude = Mathf.Lerp(newScaleMagnitude, size, currentTime / FadeTime);
            earthScale.Set(newScaleMagnitude, newScaleMagnitude, newScaleMagnitude);
            this.gameObject.transform.localScale = earthScale;
            yield return null;
        }
        isFading = false;
    }

    public IEnumerator FadeOut()
    {
        Vector3 earthScale = this.gameObject.transform.localScale;
        float currentTime = 0f;
        float newScaleMagnitude = this.transform.localScale.x;

        while (!Mathf.Approximately(this.gameObject.transform.localScale.x, 0f))
        {
            currentTime += Time.deltaTime;
            newScaleMagnitude = Mathf.Lerp(newScaleMagnitude, size, currentTime / FadeTime);
            earthScale.Set(newScaleMagnitude, newScaleMagnitude, newScaleMagnitude);
            this.gameObject.transform.localScale = earthScale;
            yield return null;
        }
        isFading = false;
    }

    public IEnumerator StartSpin()
    {
        isSpinning = false;
        float currentSpeed = 0f;
        float currentTime = 0f;
        while (!Mathf.Approximately(currentSpeed, spinSpeed))
        {
            currentTime += Time.deltaTime;
            currentSpeed = Mathf.Lerp(currentSpeed, spinSpeed, currentTime / spinStartTime);
            this.gameObject.transform.Rotate(new Vector3(0f, -currentSpeed * Time.deltaTime, 0f));
            yield return null;
        }
        isSpinning = true;
    }

    public IEnumerator StopSpin()
    {
        isSpinning = false;
        float currentSpeed = spinSpeed;
        float currentTime = 0f;
        while (!Mathf.Approximately(currentSpeed, 0f))
        {
            currentTime += Time.deltaTime;
            currentSpeed = Mathf.Lerp(currentSpeed, 0, currentTime / spinStartTime);
            this.gameObject.transform.Rotate(new Vector3(0f, -currentSpeed * Time.deltaTime, 0f));
            yield return null;
        }
        //isSpinning = false;
    }

    public IEnumerator AlignLongitude(float longitude)
    {
        if (isSpinning)
        {
            yield return StartCoroutine(StopSpin());
        }
        float currentTime = 0f;
        while (!Mathf.Approximately(this.transform.localEulerAngles.y, 180f + longitude))
        {
            currentTime += Time.deltaTime;
            this.transform.localEulerAngles = new Vector3(0f, Mathf.Lerp(this.transform.localEulerAngles.y, 180f + longitude, currentTime / spinStartTime), 0f);
            yield return null;
        }
    }

    public IEnumerator AlignLatitude(float latitude)
    {
        float currentTime = 0f;
        while (!Mathf.Approximately(this.transform.GetChild(0).localEulerAngles.z, 90f - latitude))
        {
            currentTime += Time.deltaTime;
            this.transform.GetChild(0).localEulerAngles = new Vector3(0f , 0f, Mathf.Lerp(this.transform.GetChild(0).localEulerAngles.z, 90f - latitude, currentTime / spinStartTime));
            yield return null;
        }
    }

    public IEnumerator Descend()
    {
        float currentTime = 0f;
        while (!Mathf.Approximately(this.transform.localPosition.y, -this.transform.localScale.y))
        {
            currentTime += Time.deltaTime;
            this.transform.localPosition = new Vector3(0f, Mathf.Lerp(this.transform.localPosition.y, -this.transform.localScale.y, currentTime / spinStartTime), 0f);
            yield return null;
        }
    }
}
