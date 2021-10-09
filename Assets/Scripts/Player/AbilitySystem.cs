using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    public Dictionary<string, bool> Abilities = new Dictionary<string, bool>();

    private PlayerCombat PC;
    private PlayerMovement PM;

    public float defaultMoveSpeed = 6f;
    public float defaultSprintSpeed = 12f;

    // Start is called before the first frame update
    void Start()
    {
        PC = GetComponent<PlayerCombat>();
        PM = GetComponent<PlayerMovement>();
        
        Abilities.Add("tripleJump", false);
        Abilities.Add("megaSpeed", false);
    }

    void Update()
    {
        ApplyAbilities();
    }

    void ApplyAbilities()
    {
        if (Abilities["tripleJump"].Equals(true))
        { PM.jumpAmount = 3; }
        else { PM.jumpAmount = 2; }

        if (Abilities["megaSpeed"].Equals(true))
        { PM.MoveSpeed = 15f; PM.SprintSpeed = 30f; }
        else { PM.MoveSpeed = defaultMoveSpeed; PM.SprintSpeed = defaultSprintSpeed; }
    }

    public List<string> GetAbilitiesLeft()
    {
        List<string> abilitiesLeft = new List<string>();

        foreach (var item in Abilities)
        {
            if (item.Value == false)
            {
                abilitiesLeft.Add(item.Key);
            }
        }
        
        return abilitiesLeft;
    }
}