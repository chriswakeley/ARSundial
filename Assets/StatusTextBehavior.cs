using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusTextBehavior : MonoBehaviour {
    private Text statusText;
    public float FadeTime;
	// Use this for initialization
	void Start () {
        statusText = this.gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    

    public IEnumerator ChangeText(string newText)
    {
        float currentTime = 0f;
        Color currentColor = statusText.color;
        while(currentTime < FadeTime)
        {
            currentTime += Time.deltaTime;
            currentColor.a = Mathf.Lerp(currentColor.a, 0f, currentTime / FadeTime);
            statusText.color = currentColor;
            yield return null;
        }

        statusText.text = newText;
        currentTime = 0f;
        while (currentTime < FadeTime)
        {
            currentTime += Time.deltaTime;
            currentColor.a = Mathf.Lerp(currentColor.a, 1f, currentTime / FadeTime);
            statusText.color = currentColor;
            yield return null;
        }
    }
}
