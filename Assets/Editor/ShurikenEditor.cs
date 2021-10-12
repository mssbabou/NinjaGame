using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ThirdPersonCameraController))]
public class ShurikenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Press!!!!!"))
        {
            Debug.Log("We Pressed");
        }
    }
    
}
