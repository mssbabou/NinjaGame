using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class ShurikenScript : MonoBehaviour
{
    public float speed;
    public float damage;
    public GameObject model;
    public GameObject trailModel;
    public Vector3 velocity;
    public Vector3 modelRotation;

    private Rigidbody RB;

    private float rotationSpeed = 10f;
    private float destroyDelay = 5f;
    private float lifeTime = 10f;
    
    private Transform trailModelTransform;
    private Transform modelTransform;
    private Vector3 RotVelocity;
    private float timeWhenSpawned;

    private void Start()
    {
        timeWhenSpawned = Time.time;
        velocity *= speed;
        Instantiate(model, transform);
        Instantiate(trailModel, transform);
        modelTransform = transform.GetChild(0);
        trailModelTransform = transform.GetChild(1);
        RotVelocity = new Vector3(rotationSpeed, 0f, 0f);
        trailModelTransform.localScale = new Vector3(trailModelTransform.localScale.x, trailModelTransform.localScale.y, speed*2f);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, velocity, out hit, speed * 5))
        {
            if (hit.distance < speed && !hit.collider.CompareTag("Player"))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    transform.position = hit.point;
                    DieWithDelay(); 
                    return;
                } 
                else if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                    Destroy(gameObject);
                    return;
                }
                else if (hit.collider.CompareTag("Metal"))
                {
                    velocity = Vector3.Reflect(velocity.normalized, hit.normal) * speed;
                    Move();
                    return;
                }
 
            }
 
        }

        if (velocity.magnitude >= 0.001f)
        {
            Move();
        }

        if ((lifeTime + timeWhenSpawned) <= Time.time)
        {
            Destroy(gameObject);
        }
        
    }

    void Move()
    {
        trailModelTransform.rotation = Quaternion.FromToRotation(Vector3.forward, velocity);
        modelTransform.rotation = Quaternion.FromToRotation(Vector3.forward, velocity);
        modelTransform.eulerAngles += new Vector3(0f, 90f, 0f);
        transform.Translate(velocity);
    }

    void DieWithDelay()
    {
        velocity = Vector3.zero;
        RotVelocity = Vector3.zero;
        trailModelTransform.localScale = Vector3.zero;
        Destroy(gameObject, destroyDelay);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
        Gizmos.DrawRay(transform.position, Vector3.down);
    }
}