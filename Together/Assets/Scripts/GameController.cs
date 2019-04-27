using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{


    private static GameController instance;

    private GameObject _player;
    public GameObject _eagle;

    private bool _gameOver = false;

    public float playerHealth = 100;


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

    private void Update()
    {
        if (_gameOver)
        {
            Time.timeScale = 0;
        }
    }

    public GameObject Player
    {
        get
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            return _player;
        }
    }

    public bool GameOver
    {
        get
        {
            return _gameOver;
        }
        set
        {
            _gameOver = value;
        }
    }
}
