using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light light;
    float rnd;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        rnd = Random.Range(0f, 1000f);
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = 1 + (Mathf.Sin(Time.deltaTime + rnd)) * 1f;
    }
}
