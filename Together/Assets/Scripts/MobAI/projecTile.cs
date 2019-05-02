using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projecTile : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector2 target;
    private SpriteRenderer rendr;
    private void Start()
    {
        player = GameController.Instance.Player.transform;
        rendr = GetComponent<SpriteRenderer>();

        target = new Vector2(player.position.x, player.position.y);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,target,speed*Time.deltaTime);


        //TODO: Objectpool ile buda çözülücek
        if (transform.position.x == target.x && transform.position.y == target.y)
            Destroy(this.gameObject);


        //TODO: Karekterin konumuna göre yönlenicek

        if (player.transform.position.x - transform.position.x < 0)
            rendr.flipX = true;
        else
            rendr.flipX = false;

    }
}
