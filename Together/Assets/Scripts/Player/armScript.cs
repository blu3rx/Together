using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armScript : MonoBehaviour
{
    void Awake()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "eagleTag")
        {
           col.gameObject.GetComponent<EagleScript>().eagle.Hit(GameController.Instance.Player.GetComponent<playerMovement>().player.Damage);
        }
    }
}
