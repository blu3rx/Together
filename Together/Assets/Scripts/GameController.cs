using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
   

    private static GameController instance;

    public GameObject _player;

    public void Awake()
    {
        if (instance != null)
            Destroy(gameObject);


    }

    public static GameController Instance
    {
        get
        {
            instance = FindObjectOfType<GameController>();
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "GameController";
                instance = go.AddComponent<GameController>();

            }
            return instance;
        }
    }
}
