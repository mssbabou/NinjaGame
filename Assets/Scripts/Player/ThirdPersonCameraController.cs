using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ThirdPersonCameraController : MonoBehaviour
{
    [HideInInspector] public Camera cam;
    [HideInInspector] public bool OverShoulder;
    public LayerMask playerLayer;
    [Space(5)]
    public GameObject Crosshair;
    public Transform CameraFollow;
    public float ShoulderTransitionTime = 0.1f;
    public Vector2 Sensitivity = new Vector2(0.02f, 0.02f);
    [Space(10)]
    public Vector2 DampRotation;
    [Space(10)]
    public Vector3 OffsetPostion = new Vector3(0f, 4f, -8.5f);
    [Range(-5, 5)] public float Side;

    [Space(5)] 
    public Vector2 ClampY; 

    private float SmoothTransition;
    private Transform CameraLookAt;
    private Transform CameraDestination;
    private Vector3 SmoothVelocity;
    private Vector2 SmoothAngle;
    private Vector2 look;
    private Vector2 angleDst;
    private Vector2 angle;

    private Stopwatch SW;

    void Start()
    {
        SW = Stopwatch.StartNew();
        cam = Camera.main;
        CameraLookAt = CameraFollow.GetChild(0);
        CameraDestination = CameraLookAt.GetChild(0);
    }

    void Update()
    {
        Crosshair.SetActive(OverShoulder);
        
        if (OverShoulder)
        {
            float target = Mathf.SmoothDamp(CameraLookAt.localPosition.x, Side, ref SmoothTransition, ShoulderTransitionTime);
            CameraLookAt.localPosition = new Vector3(target, 0f, 0f);
        }
        else
        {          
            float target = Mathf.SmoothDamp(CameraLookAt.localPosition.x, 0f, ref SmoothTransition, ShoulderTransitionTime);
            CameraLookAt.localPosition = new Vector3(target, 0f, 0f);
        }
        
        // Get player input
        look.x = UnityEngine.Input.GetAxisRaw("Mouse X");
        look.y = UnityEngine.Input.GetAxisRaw("Mouse Y");
        
        angle.x += look.x * Sensitivity.x;
        angle.y += look.y * Sensitivity.y;
        angle.y = Mathf.Clamp(angle.y, ClampY.y, ClampY.x);  

        angleDst.x = Mathf.SmoothDampAngle(angleDst.x, angle.x, ref SmoothAngle.x, DampRotation.x);
        angleDst.y = Mathf.SmoothDampAngle(angleDst.y, angle.y, ref SmoothAngle.y, DampRotation.y);
        
        CameraDestination.localPosition = OffsetPostion;  
        CameraFollow.eulerAngles = new Vector3(0f, angleDst.x, 0f);
        CameraLookAt.localEulerAngles = new Vector3(-angleDst.y, 0f, 0f);
        CameraDestination.LookAt(CameraLookAt);
        
        Vector3 rayDir = CameraDestination.position - CameraLookAt.position;
        RaycastHit CameraHit;
        if (Physics.Raycast(CameraLookAt.position, rayDir, out CameraHit, rayDir.magnitude))
        {
            if (!CameraHit.collider.CompareTag("Player"))
            {
                cam.transform.position = CameraHit.point + (-rayDir * 0.1f);   
            }
            else
            {
                cam.transform.position = CameraDestination.position;
            }
        }
        else
        {
            cam.transform.position = CameraDestination.position;
        }


        cam.transform.rotation = CameraDestination.rotation;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(CameraFollow.position, CameraFollow.GetChild(0).transform.position);
        Gizmos.DrawLine(CameraFollow.GetChild(0).transform.position, cam.transform.position);
        Gizmos.DrawSphere(CameraFollow.position, .1f);
        Gizmos.DrawSphere(CameraFollow.GetChild(0).transform.position, .1f);
        Gizmos.DrawSphere(cam.transform.position, 0.1f);
    }
}
