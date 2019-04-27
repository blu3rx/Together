using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianAiDetector : MonoBehaviour
{
    bool isDetectEngel = false;
    string detectorTagName = "";
    float WhenDetectEngelHeight = 0f, WhenOutEngelHeight = 0f;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "groundTag"&&!isDetectEngel)
        {
            detectorTagName = col.gameObject.tag;
            WhenDetectEngelHeight = GetComponent<CircleCollider2D>().offset.y;
            isDetectEngel = true;
            gameObject.transform.root.GetComponent<BarbarianScript>().BOOLAiAttackAble = false;
        }
        if (col.gameObject.tag != "groundTag" || col.gameObject == null) { gameObject.transform.root.GetComponent<BarbarianScript>().BOOLAiAttackAble = true; }
    }
    void Update()
    {
        if (isDetectEngel)
        {
            GetComponent<CircleCollider2D>().offset = new Vector2(GetComponent<CircleCollider2D>().offset.x, GetComponent<CircleCollider2D>().offset.y+0.1f);
            
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {

        if (isDetectEngel && col.gameObject.tag == detectorTagName)
        {
            isDetectEngel = false;
            WhenOutEngelHeight = GetComponent<CircleCollider2D>().offset.y;
            GetComponent<CircleCollider2D>().offset = new Vector2(GetComponent<CircleCollider2D>().offset.x,WhenDetectEngelHeight);
           if (WhenOutEngelHeight - WhenDetectEngelHeight <= 0.6f) gameObject.transform.root.GetComponent<BarbarianScript>().BOOLAiJump = true;
        }
    }
}
