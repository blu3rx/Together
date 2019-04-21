using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseCharacter : MonoBehaviour
{

    private float health;
    private float damage;
    private string characterName;
   

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }
    public float Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }
    public string CharacterName
    {
        get
        {
            return characterName;
        }
        set
        {
            characterName = value;
        }
    }

}
