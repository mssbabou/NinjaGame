using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [HideInInspector] public Camera cam;
    [HideInInspector] public bool IsShoulder;
    [Space(5)]
    public GameObject Crosshair;
    public Transform CameraFollow;
    public Vector2 CameraSpeed;
    [Space(10)]
    public Vector2 DampRotation;
    public Vector3 DampPostion;
    [Space(10)]
    public Vector3 OffsetPostion;
    public Vector3 cameraFollowOfsset;
    [Range(-5, 5)] public float Side;

    [Space(5)] 
    public float ClampUp;
    public float ClampDown;
    private Transform CameraLookAt;
    private Transform CameraDestination;
    private Vector3 SmoothVelocity;
    private Vector2 SmoothAngle;
    private Vector2 look;
    private Vector2 angle;
    private bool useDampening;

    void Start()
    {
        cam = Camera.main;
        CameraLookAt = CameraFollow.GetChild(0);
        CameraDestination = CameraLookAt.GetChild(0);
        
        if (DampPostion == Vector3.zero && DampRotation == Vector2.zero)
        {
            useDampening = false;
        }
        else
        {
            useDampening = true;
        }
    }
    
    void Update()
    {
        Crosshair.SetActive(IsShoulder);

        if (IsShoulder)
        {
            CameraFollow.position = transform.position + cameraFollowOfsset;
            CameraLookAt.localPosition = new Vector3(Side, 0f, 0f);
        }
        else
        {
            CameraFollow.position = transform.position + cameraFollowOfsset;
            CameraLookAt.localPosition = Vector3.zero;
        }
        
        // Get player input
        look.x = Input.GetAxisRaw("Mouse X") * CameraSpeed.x;
        look.y = Input.GetAxisRaw("Mouse Y") * CameraSpeed.y;
        
        angle.x += look.x;
        angle.y += look.y;
        angle.y = Mathf.Clamp(angle.y, ClampDown, ClampUp);
        
        CameraDestination.localPosition = OffsetPostion;
        CameraFollow.eulerAngles = new Vector3(0f, angle.x, 0f);
        CameraLookAt.localEulerAngles = new Vector3(-angle.y, 0f, 0f);
        CameraDestination.LookAt(CameraLookAt);

        if (useDampening)
        {
            SmoothTransform();   
        }
        else
        {
            cam.transform.rotation = CameraDestination.rotation;
            cam.transform.position = CameraDestination.position;
        }
    }

    void SmoothTransform()
    {
        Vector3 NewPosition = cam.transform.position;
        Vector3 NewRotation = cam.transform.eulerAngles;

        NewPosition.x = Mathf.SmoothDamp(NewPosition.x, CameraDestination.position.x, ref SmoothVelocity.x, DampPostion.x);
        NewPosition.y = Mathf.SmoothDamp(NewPosition.y, CameraDestination.position.y, ref SmoothVelocity.y, DampPostion.y);
        NewPosition.z = Mathf.SmoothDamp(NewPosition.z, CameraDestination.position.z, ref SmoothVelocity.z, DampPostion.z);

        NewRotation.x = Mathf.SmoothDampAngle(NewRotation.x, CameraDestination.eulerAngles.x, ref SmoothAngle.x, DampRotation.x);
        NewRotation.y = Mathf.SmoothDampAngle(NewRotation.y, CameraDestination.eulerAngles.y, ref SmoothAngle.y, DampRotation.y);

        cam.transform.eulerAngles = NewRotation;
        cam.transform.position = NewPosition;
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
