using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseCharacter 
{

    private float health;
    private float damage;
    private string characterName;
    public bool isDead=false;

   

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

    public void Hit (float hit)
    {
        Health-=hit;
        if (health <= 0)
        {
           // Debug.Log(characterName + "Öldü pij");
            isDead =  true;

        }

    }




}
