using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenScriptV2 : MonoBehaviour
{

    private Vector3 velocity;
    
    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0f, 0f, 0.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(velocity);
    }
}
