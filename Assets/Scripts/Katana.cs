using UnityEngine;


    [CreateAssetMenu(fileName = "Katana", menuName = "Weapon/Katana")]
    public class Katana : ScriptableObject
    {
        public new string name;
    
        public GameObject gameObj;
        public float damage;
        public float range;
        public float attackSpeed; 
    }