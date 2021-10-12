using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class ShurikenScript : MonoBehaviour
{
    public float speed;
    public float damage;
    public GameObject model;
    public GameObject trailModel;

    private Rigidbody RB;
    
    private float rotationSpeed = 1f;
    private float destroyDelay = 50f;
    private float lifeTime = 100f;

    [SerializeField] 
    private Transform trailModelTransform;
    private Transform modelTransform;
    private Vector3 velocity;
    private Vector3 RotVelocity;
    private float timeWhenSpawned;

    private void Start()
    {
        timeWhenSpawned = Time.time;
        velocity = new Vector3(0f, 0f, speed);
        Instantiate(model, transform);
        Instantiate(trailModel, transform);
        modelTransform = transform.GetChild(0);
        trailModelTransform = transform.GetChild(1);
        RotVelocity = new Vector3(0f, 0f, rotationSpeed);
        trailModelTransform.localScale = new Vector3(trailModelTransform.localScale.x, trailModelTransform.localScale.y, speed*2f);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed * 5))
        {
            if (hit.distance < speed && !hit.collider.CompareTag("Player"))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    transform.position = hit.point;
                    Die(); 
                }

                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
            else
            {
                transform.Translate(velocity);    
            }
        }
        else
        {
            transform.Translate(velocity);
        }
        
        modelTransform.Rotate(-RotVelocity);

        if ((lifeTime + timeWhenSpawned) <= Time.time)
        {
            Destroy(gameObject);
        }
        
    }

    void Die()
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