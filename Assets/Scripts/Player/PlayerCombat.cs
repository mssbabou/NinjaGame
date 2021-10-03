using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCombat : MonoBehaviour
{
    public Transform HoldingPoint;
    public Shuriken defaultShuriken;
    public Katana defaultKatana;
    public LayerMask playerLayer;
    public Int16 itemSlot = 1;

    public float health = 100;

    private PlayerMovement PM;
    private ShurikenScript SS;
    private PlayerCameraController PCC;

    void Start()
    {
        SS = defaultShuriken.gameObj.GetComponent<ShurikenScript>();
        PCC = GetComponent<PlayerCameraController>();
        PM = GetComponent<PlayerMovement>();
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            itemSlot = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            itemSlot = 2;
        }
        
        
        if (itemSlot == 1)
        { 
            PCC.IsShoulder = false;
            if (Input.GetButtonDown("Fire1"))
            {
                //HIT
            }
        }
        
        if (itemSlot == 2)
        {
            PCC.IsShoulder = true;
            if (Input.GetButtonDown("Fire1"))
            {
                SS.speed = defaultShuriken.speed;
                SS.damage = defaultShuriken.damage;
                
                RaycastHit hit;
                if (Physics.Raycast(PCC.cam.transform.position, PCC.cam.transform.forward, out hit, 1000f))
                {
                    HoldingPoint.LookAt(hit.point);   
                }
                else
                {
                    
                    HoldingPoint.rotation = PCC.cam.transform.rotation;
                }

                Instantiate(defaultShuriken.gameObj, HoldingPoint.position, HoldingPoint.rotation);
            }

        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Heal(float healing)
    {
        health += Mathf.Clamp(healing, 0, 100);
    }
}
