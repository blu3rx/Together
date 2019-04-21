using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
   

    private static GameController instance;

    private GameObject _player;
    public GameObject _eagle;
    
    

    public void Awake()
    {
        
        if (instance != null)
            Destroy(gameObject);

        _player = GameObject.FindGameObjectWithTag("Player");


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

    public GameObject Player
    {
        get
        {
            return _player;
        }
    }
}
