using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour
{
    public Vector3 hidden;
    public Vector3 visible;

    LoadAssets loadAssets;

    // Start is called before the first frame update
    void Start()
    {
        loadAssets = FindObjectOfType<LoadAssets>();   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Slerp(transform.position, loadAssets.IsNPCHidden ? hidden : visible, Time.deltaTime);
    }
}
