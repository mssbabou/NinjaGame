using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float speed = 1f;
    public float amplitude = 1f;
    private Rigidbody rb;
    // Start is called before the first frame update

    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(startPos.x + Mathf.Sin(Time.time*speed)*amplitude, startPos.y, startPos.z);
    }
}
