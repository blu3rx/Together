using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{


    private static GameController instance;

    private GameObject _player;
    private GameObject _eagle;
    private GameObject _opossum;
    private GameObject _wizard;

    private bool _gameOver = false;


    [Header("Player")]
    public float playerHealth = 100f;
    public float playerDamage = 20f;
    [Header("Wizard")]
    public float wizardHealth = 100f;
    public float wizardDamage = 30f;
    [Header("Opossum")]
    public float opossumHealth = 10f;
    public float opossumDamage = 110f;


    public void Awake()
    {

        if (instance != null)
            Destroy(gameObject);




    }
    private void Start()
    {
        Player.GetComponent<playerMovement>().Health = playerHealth;
        Player.GetComponent<playerMovement>().Damage = playerDamage;

        Wizard.GetComponent<wizardBrain>().Health = wizardHealth;
        Wizard.GetComponent<wizardBrain>().Damage = wizardDamage;

        if (Opossum != null)
        {
            Opossum.GetComponent<opossumBrain>().Health = opossumHealth;
            Opossum.GetComponent<opossumBrain>().Damage = opossumDamage;

        }

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
    public GameObject Opossum
    {
        get
        {
            _opossum = GameObject.FindGameObjectWithTag("opossumTag");
            return _opossum;
        }
    }
    public GameObject Wizard
    {
        get
        {
            _wizard = GameObject.FindGameObjectWithTag("wizardTag");
            return _wizard;
        }
    }
    public GameObject Eagle
    {
        get
        {
            _eagle = GameObject.FindGameObjectWithTag("eagleTag");
            return _eagle;
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
