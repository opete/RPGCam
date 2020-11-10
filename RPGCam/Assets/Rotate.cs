using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    Vector3 startEuler;

    // Start is called before the first frame update
    void Start()
    {
        startEuler = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(startEuler.x, startEuler.y + Mathf.Sin(Time.deltaTime), startEuler.z);
    }
}
