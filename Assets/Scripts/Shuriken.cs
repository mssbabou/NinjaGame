using UnityEngine;

[CreateAssetMenu(fileName = "Shuriken", menuName = "Weapon/Shuriken")]
public class Shuriken : ScriptableObject
{
    public new string name;
    
    public GameObject model;
    public float damage;
    public float speed;
    public float maxDrawTime;
}
