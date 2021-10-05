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

    private PlayerMovement PM;
    private ThirdPersonCameraController PCC;
    public GameObject ShurikenObject;
    private ShurikenScript SS;
    private GameObject ShurikenGFX;

    void Start()
    {
        PCC = GetComponent<ThirdPersonCameraController>();
        PM = GetComponent<PlayerMovement>();
        SS = ShurikenObject.GetComponent<ShurikenScript>();
        ShurikenGFX = GetComponentInChildren<GameObject>();
    }
    
    void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
        {
            itemSlot = 1;
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
        {
            itemSlot = 2;
        }
        
        
        if (itemSlot == 1)
        { 
            PCC.OverShoulder = false;
            if (UnityEngine.Input.GetButtonDown("Fire1"))
            {
                //HIT
            }
        }
        
        if (itemSlot == 2)
        {
            PCC.OverShoulder = true;
            if (UnityEngine.Input.GetButtonDown("Fire1"))
            {
                SS.speed = defaultShuriken.speed;
                SS.damage = defaultShuriken.damage;
                ShurikenGFX = defaultShuriken.model;
                
                RaycastHit hit;
                if (Physics.Raycast(PCC.cam.transform.position, PCC.cam.transform.forward, out hit, 1000f))
                {
                    HoldingPoint.LookAt(hit.point);   
                }
                else
                {
                    
                    HoldingPoint.rotation = PCC.cam.transform.rotation;
                }

                Instantiate(ShurikenObject, HoldingPoint.position, HoldingPoint.rotation);
            }

        }
    }
}
