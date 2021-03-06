using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXVGlassyShineControl : MonoBehaviour
{
    private float shineSpeed = 1.0f;

    private int reflectionTimePropery;

    private bool isShining = false;

    private float shineTime = 0.0f;

    private Renderer[] renderers;

    void Start ()
    {
        reflectionTimePropery = Shader.PropertyToID("_ReflectionTime");

        renderers = GetComponentsInChildren<Renderer>();
    }

	void Update ()
    {
        if (isShining)
        {
            shineTime += Time.deltaTime * shineSpeed;

            if (shineTime >= 1.0f)
            {
                isShining = false;
                shineTime = 0.0f;
            }

            foreach (Renderer r in renderers)
            {
                r.material.SetFloat(reflectionTimePropery, shineTime);
            }
        }
	}

    public void DoShine(float speed)
    {
        shineSpeed = speed;
        isShining = true;
    }
}
