using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleScript : MonoBehaviour
{
    public float AreaOfStrolling = 5f, velocity = 3f;
    float x;
    bool isFly=false,direction=false;
    void Start()
    {
        x = this.transform.position.x;
        isFly = true;

    }
    void Update()
    {
        if (isFly)
        {
            if (direction)
                transform.Translate(velocity,0,0);
            else
                transform.Translate(-velocity, 0, 0);

            if (transform.position.x > x + AreaOfStrolling)
            {
                direction = !direction;
                this.transform.GetComponent<SpriteRenderer>().flipX = false;
            }

            if (transform.position.x < x)
            {
                direction = !direction;
                this.transform.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }
}
