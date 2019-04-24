using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "eagleTag")
        {
            Debug.Log("KartalaVuruldu");
           col.gameObject.GetComponent<EagleScript>().eagle.Hit(GameController.Instance.Player.GetComponent<playerMovement>().player.Damage);
        }
        if (col.gameObject.tag =="barbarianTag")
        {
            col.gameObject.GetComponent<BarbarianScript>().barbarianKalitim.Hit(GameController.Instance.Player.GetComponent<playerMovement>().player.Damage);
            col.gameObject.GetComponent<BarbarianScript>().HitFunction();
       
        }
    }
}
