using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCombat : MonoBehaviour
{
    public Transform HoldingPoint;    
    public Int16 itemSlot = 1;
    [Space(5)]
    public GameObject shurikenObject;
    public Shuriken shuriken;
    [Space(5)]
    public Katana defaultKatana;

    private PlayerMovement PM;
    private ThirdPersonCameraController TPCC;
    private ShurikenScript SS;
        
    // Shuriken
    [Space(20)]
    private float drawPercent;
    private float drawTime;
    private float timeWhenDraw;
    
    
    // Katana
    
    void Start()
    {
        TPCC = GetComponent<ThirdPersonCameraController>();
        PM = GetComponent<PlayerMovement>();
        SS = shurikenObject.GetComponent<ShurikenScript>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { itemSlot = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { itemSlot = 2; }

        if (itemSlot == 1)
        { 
            KatanaMode();
        }
        
        if (itemSlot == 2)
        {
            ShurikenMode();
        }
    }

    void KatanaMode()
    {
        TPCC.OverShoulder = false;
        if (Input.GetButtonDown("Fire1"))
        {
            //HIT
        }
    }
    
    void ShurikenMode()
    {
        TPCC.OverShoulder = true;
        if (Input.GetButtonDown("Fire1"))
        {
            timeWhenDraw = Time.time;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            SS.speed = shuriken.speed * drawPercent;
            SS.damage = shuriken.damage * drawPercent;

            RaycastHit hit;
            if (Physics.Raycast(TPCC.cam.transform.position, TPCC.cam.transform.forward, out hit, 1000f))
            {
                HoldingPoint.LookAt(hit.point);   
            }
            else
            {
                HoldingPoint.rotation = TPCC.cam.transform.rotation;
            }

            Instantiate(shurikenObject, HoldingPoint.position, HoldingPoint.rotation);

            drawTime = 0f;
            drawPercent = 0f;
            timeWhenDraw = 0f;
        }
            
        if (Input.GetButton("Fire1"))
        {
            drawTime = Time.time - timeWhenDraw + shuriken.maxDrawTime / 2f;
            drawPercent = drawTime / shuriken.maxDrawTime;
            drawPercent = Mathf.Clamp(drawPercent, 0f, 1f);
        }
    }
    
}
