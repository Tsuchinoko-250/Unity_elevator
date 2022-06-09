using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorManager : MonoBehaviour
{
    public enum EreaName
    {
        Free,           //無し
        Entrance,       //玄関
        Hall,           //廊下
        Laboratory01,   //研究室1
        Laboratory02,   //研究室2
        Westernroom,    //洋室
        Kitchen,        //キッチン
        Bedroom,        //寝室
        Bathroom,       //風呂場
        Toilet,         //トイレ
        Washroom,       //洗面所
        Study,          //書斎
        Elevator,       //エレベーター
    }

    [SerializeField]
    public EreaName NowRoom;
    [SerializeField]
    public EreaName NextRoom;
    [SerializeField]
    public Transform charactor;
    [SerializeField]
    public Image Blackout;
    [SerializeField]
    private float fadespeed;


    public static bool areachangetrigger = false;
    public static Vector3 NextPoint;
    private float alfa = 0;
    public static bool isWarping = false;



    // Update is called once per frame
    void Update()
    {
        if(areachangetrigger == true)
        {
            areachangetrigger = false;
            isWarping = true;
            StartCoroutine(BlackFadeIn());
        }
    }

    IEnumerator BlackFadeIn()
    {
        if(alfa <= 1.2)
        {
            Blackout.color = new Color(0.0f, 0.0f, 0.0f, alfa);
            yield return new WaitForSeconds(0.1f);
            alfa += fadespeed;
            StartCoroutine(BlackFadeIn());
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            charactor.transform.position = NextPoint;
            StartCoroutine(BlackFadeOut());
        }
        
    }

    IEnumerator BlackFadeOut()
    {
        if(alfa >= 0)
        {
            Blackout.color = new Color(0.0f, 0.0f, 0.0f, alfa);
            yield return new WaitForSeconds(0.1f);
            alfa -= fadespeed;
            StartCoroutine(BlackFadeOut());
        }
        else
        {
            isWarping = false;
        }
    }

}
