using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSlightly : MonoBehaviour
{
    Vector3 startPos;
    public Transform lookAt;
    public float xMultiplier = 1;
    public float speedMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(startPos.x + Mathf.Sin(Time.time / speedMultiplier) * xMultiplier, startPos.y, startPos.z);
        transform.LookAt(lookAt);
    }
}
