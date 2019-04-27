using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianSwordScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
           
            col.gameObject.GetComponent<playerMovement>().player.Hit(gameObject.transform.root.GetComponent<BarbarianScript>().barbarianKalitim.Damage);
        }
    }
}
