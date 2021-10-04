using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI frameText;
    private float updateInterval = 0.3f;
    private float lastTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if ((updateInterval + lastTime) <= Time.time)
        {
            frameText.text = (Mathf.Floor(1f / Time.deltaTime)).ToString();
            lastTime = Time.time;
        }
    }
}