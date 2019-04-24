using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleScript : MonoBehaviour
{

    
    //Show Debug Logs On/Off
    public bool ShowDebugLogs = false;

   

    public float AreaOfStrolling = 5f, velocity = 3f, velocityForEagleAtack = 0.03f,DistanceOfAttack=2f,WaitForAgainAttackSecond=3f;
    float StartPositionX,StartPositionY;
    float smoothFactor = 0.01f, velo = 0.1f, WaitForAgainAttackSecondCurrent=0f;
    bool BOOLIsFly = false, BOOLDirection = false, BOOLIsEnemySpoted = false, BOOLReadyForAttack = false,BOOLAfterHit=false,BOOLAtackIsComplete=false,BOOLDead=false;
    GameObject GOCharacter;
    Vector2 VECTOR2characterWhenSpoted;
    Vector3 currentAngle;
    public Sprite SPRITEWhenAttack;

    public baseCharacter eagle; //kalıtım
    
    void Awake()
    {
        eagle = new baseCharacter();
        eagle.CharacterName = "eagle";

        eagle.Health = 200;
        eagle.Damage = 5;

        GOCharacter = GameController.Instance.Player;
    }
    void Start()
    {


        
        StartPositionX = this.transform.position.x;
        StartPositionY = this.transform.position.y;
        BOOLIsFly = true;
        currentAngle = transform.eulerAngles;
        BOOLAtackIsComplete = true;

    }
    void Update()
    {
        #region Dead
        if (eagle.isDead)
        {
            eagle.isDead = false;
            BOOLDead = true;
            BOOLAfterHit = false;
            BOOLAtackIsComplete = false;
            BOOLIsEnemySpoted = false;
            BOOLIsFly = false;
            BOOLReadyForAttack = false;
            
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;

            if (gameObject.GetComponent<SpriteRenderer>().flipX)
                transform.rotation = new Quaternion(0, 0, 270,0);
            else
                transform.rotation = new Quaternion(0, 0, 90, 0);

            gameObject.GetComponent<Animator>().enabled = false;
            Destroy(gameObject, 5f);
        }
        if (BOOLDead)
        {
            transform.Translate(0, velocity * Time.deltaTime * 2, 0);
        }

        #endregion
        if (Mathf.Abs(Mathf.Abs(gameObject.transform.position.x) - Mathf.Abs(GOCharacter.gameObject.transform.position.x)) < DistanceOfAttack  && WaitForAgainAttackSecond<=WaitForAgainAttackSecondCurrent)
        {
            BOOLIsEnemySpoted = true;
            velo = 0.1f;
            WaitForAgainAttackSecondCurrent = 0f;
            BOOLAtackIsComplete = false;
            if (ShowDebugLogs)
            Debug.Log("Player Alana Girdi.");
        }
        if (BOOLAtackIsComplete)
        {
            WaitForAgainAttackSecondCurrent += Time.deltaTime;
            if (ShowDebugLogs)
            Debug.Log("Timer Wait For Again Attack: "+WaitForAgainAttackSecondCurrent);
            if (WaitForAgainAttackSecondCurrent > WaitForAgainAttackSecond)
                BOOLAtackIsComplete = false;
        }

        #region EnemySpoted


        if (BOOLIsEnemySpoted)
        {
            BOOLIsFly = false;


           
            VECTOR2characterWhenSpoted = new Vector2(GOCharacter.gameObject.transform.position.x,GOCharacter.gameObject.transform.position.y);
           



            int eksi1 = 1, angle90 = 180 + 90;
            if (this.gameObject.GetComponent<SpriteRenderer>().flipX == true)
            {
                eksi1 = -eksi1;
                angle90 = 0;
            }
            // get the angle
            Vector3 normalizedTarget = (new Vector3(VECTOR2characterWhenSpoted.x,VECTOR2characterWhenSpoted.y,0) - transform.position).normalized;
            float angle = Mathf.Atan2(normalizedTarget.x, normalizedTarget.y) * Mathf.Rad2Deg;
            // rotate to angle
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(0, 0, (eksi1 * (angle - 90 + angle90)));

            transform.eulerAngles = currentAngle;

            if (transform.position.x < GOCharacter.gameObject.transform.position.x)
            {
                // Kartal Karakterin Solunda.
                if (ShowDebugLogs)
                Debug.Log("Kartal Karakterin Solunda.");
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }

            else
            {
                // Kartal KArakterin Sağında
                if (ShowDebugLogs)
                Debug.Log("Kartal KArakterin Sağında");
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }

            

            if (((Mathf.Abs(currentAngle.z) + Mathf.Abs(rotation.eulerAngles.z)) > 355 && transform.position.x < VECTOR2characterWhenSpoted.x) || (Mathf.Abs((Mathf.Abs(currentAngle.z) - Mathf.Abs(rotation.eulerAngles.z))) < 5 && transform.position.x > VECTOR2characterWhenSpoted.x))
            {
                if (ShowDebugLogs)
                Debug.Log("Finish Enemy Spoted.");
                BOOLReadyForAttack = true;
                BOOLIsEnemySpoted = false;
                gameObject.GetComponent<Animator>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().sprite = SPRITEWhenAttack;
            }
            else
            {
                currentAngle = new Vector3(
                Mathf.LerpAngle(currentAngle.x, rotation.eulerAngles.x, velo),
                Mathf.LerpAngle(currentAngle.y, rotation.eulerAngles.y, velo),
                Mathf.LerpAngle(currentAngle.z, rotation.eulerAngles.z, velo));
            }

        }
        #endregion
        #region Attack


        if (BOOLReadyForAttack)//Attack Yapılacak Pozisyona geçildi.
        {
            smoothFactor += velocityForEagleAtack;
            if (ShowDebugLogs)
            Debug.Log(BOOLIsEnemySpoted + "--"+VECTOR2characterWhenSpoted.x.ToString());
            Vector2 KafaPayliPosition = new Vector2(VECTOR2characterWhenSpoted.x, VECTOR2characterWhenSpoted.y + 0.3f/*karakterin tam kafasına saldırması için kafa payı*/);
            transform.position = Vector3.Lerp(transform.position, KafaPayliPosition, Time.deltaTime * smoothFactor);
            if (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(VECTOR2characterWhenSpoted.x)) < 0.05f && Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(VECTOR2characterWhenSpoted.y + 0.3f/*KafaPayı*/)) < 0.05f)
            {
                BOOLAfterHit = true;
                gameObject.GetComponent<Animator>().enabled = true;
                BOOLReadyForAttack = false;
                if (ShowDebugLogs)
                Debug.Log("Çarpışma Gerçekleşti");
                smoothFactor = 0.1f;
            }

        }

        #endregion
        #region AfterAttack
        if (BOOLAfterHit)//Playere Kafa Attıktan sonra yerine dönülecek
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            if (!(StartPositionY <=this.transform.position.y))
            {
               
                gameObject.transform.Translate(0, velocity * Time.deltaTime, 0);    
                
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = BOOLDirection;
                if (!(gameObject.transform.position.x < StartPositionX)&&!(gameObject.transform.position.x > StartPositionX + AreaOfStrolling))
                {
                    BOOLAfterHit = false;
                    BOOLIsFly = true;
                    BOOLAtackIsComplete = true;
                   
                }
                
            }

            if (gameObject.transform.position.x < StartPositionX)
            {
                gameObject.transform.Translate(velocity * Time.deltaTime, 0, 0);
            }
            else if (gameObject.transform.position.x > StartPositionX + AreaOfStrolling)
            {
                gameObject.transform.Translate(-velocity * Time.deltaTime, 0, 0);
            }
            
        }
        #endregion
        #region Fly

        if (BOOLIsFly)
        {
            if (BOOLDirection)
            {
                transform.Translate(velocity * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.Translate(-velocity * Time.deltaTime, 0, 0);
            }

            if (transform.position.x > StartPositionX + AreaOfStrolling)
            {
                BOOLDirection = !BOOLDirection;
                this.transform.GetComponent<SpriteRenderer>().flipX = false;
            }

            if (transform.position.x < StartPositionX)
            {
                BOOLDirection = !BOOLDirection;
                this.transform.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        #endregion
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<playerMovement>().player.Hit(eagle.Damage);
        }
        else
        {
            BOOLAfterHit = true;
            gameObject.GetComponent<Animator>().enabled = true;
            BOOLReadyForAttack = false;
            if (ShowDebugLogs)
                Debug.Log("Duvara Çarptı");
            smoothFactor = 0.1f;
        }
    }


}
