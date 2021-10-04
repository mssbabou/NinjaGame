using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI textMesh;
    
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        textMesh.text = health.ToString();
    }
    
    public void SetHealth(float health)
    {
        slider.value = health;
        textMesh.text = health.ToString();
    }
}
