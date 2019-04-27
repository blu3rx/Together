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

        //Debug.Log(characterName + ": Kalan can: " + health.ToString());
        Health-=hit;
        if (health <= 0)
        {
            isDead =  true;
        }

    }




}
