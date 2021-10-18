using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (BaseEnemy))]
public class FieldOfViewEditor : Editor {

    void OnSceneGUI() {
        BaseEnemy BE = (BaseEnemy)target;
        Handles.color = Color.white;
        Handles.DrawWireArc (BE.transform.position, Vector3.up, Vector3.forward, 360, BE.viewRadius);
        Handles.color = Color.red;
        Handles.DrawWireArc (BE.transform.position, Vector3.up, Vector3.forward, 360, BE.attackRadius);
        Handles.color = Color.green;
        Handles.DrawWireArc (BE.transform.position, Vector3.up, Vector3.forward, 360, BE.talkRadius);
        Handles.color = Color.white;
        Vector3 viewAngleA = BE.DirFromAngle (-BE.viewAngle / 2, false);
        Vector3 viewAngleB = BE.DirFromAngle (BE.viewAngle / 2, false);

        Handles.DrawLine (BE.transform.position, BE.transform.position + viewAngleA * BE.viewRadius);
        Handles.DrawLine (BE.transform.position, BE.transform.position + viewAngleB * BE.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in BE.visibleTargets) {
            Handles.DrawLine (BE.transform.position, visibleTarget.position);
        }
    }

}