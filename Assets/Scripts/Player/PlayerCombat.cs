using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public Transform HoldingPoint;    
    public int itemSlot = 1;
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
    private BoxCollider KatanaHit;
    private List<EnemyHealth> enemies = new List<EnemyHealth>();
    private float cooldown;

    void Start()
    {
        TPCC = GetComponent<ThirdPersonCameraController>();
        PM = GetComponent<PlayerMovement>();
        SS = shurikenObject.GetComponent<ShurikenScript>();

        //Katana
        cooldown = defaultKatana.attackSpeed;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { itemSlot = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { itemSlot = 2; }
        itemSlot += (int)Mathf.Ceil(Input.mouseScrollDelta.y);
        itemSlot = Mathf.Clamp(itemSlot, 1, 2);


        if (itemSlot == 1)
        { 
            KatanaMode();
        }
        
        if (itemSlot == 2)
        {
            ShurikenMode();
        }

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    void KatanaMode()
    {
        TPCC.OverShoulder = false;
        if (Input.GetButtonDown("Fire1") && enemies.Count > 0 && cooldown <= 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].TakeDamage(defaultKatana.damage);
            }
            cooldown = defaultKatana.attackSpeed;
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

            RaycastHit hit;
            if (Physics.Raycast(TPCC.cam.transform.position, TPCC.cam.transform.forward, out hit, 1000f))
            {
                HoldingPoint.LookAt(hit.point);   
            }
            else
            {
                HoldingPoint.rotation = TPCC.cam.transform.rotation;
            }
            
            PM.targetAngle = HoldingPoint.eulerAngles.y;

            SpawnShuriken(shuriken, drawPercent);

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

    void SpawnShuriken(Shuriken _shuriken, float _statMultiplier)
    {
        SS.speed = _shuriken.speed * _statMultiplier;
        SS.damage = _shuriken.damage * _statMultiplier;
        SS.model = _shuriken.model;

        Instantiate(shurikenObject, HoldingPoint.position, HoldingPoint.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.gameObject.GetComponent<EnemyHealth>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject.GetComponent<EnemyHealth>());
        }
    }

}
