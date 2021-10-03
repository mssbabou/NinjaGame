using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Transform playerTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
