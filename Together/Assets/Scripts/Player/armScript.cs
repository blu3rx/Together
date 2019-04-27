using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armScript : MonoBehaviour
{
    private GameObject _player;


    private void Awake()
    {
        _player = GameController.Instance.Player;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "eagleTag")
        {
           col.gameObject.GetComponent<EagleScript>().eagle.Hit(_player.GetComponent<playerMovement>().player.Damage);
        }
        if (col.gameObject.tag =="barbarianTag")
        {
            col.gameObject.GetComponent<BarbarianScript>().barbarianKalitim.Hit(_player.GetComponent<playerMovement>().player.Damage);
            col.gameObject.GetComponent<BarbarianScript>().HitFunction();
       
        }
    }
}
