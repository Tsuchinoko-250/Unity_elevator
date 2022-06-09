using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //移動スピード
    [SerializeField]
    private float speed;

    //アニメーション
    public static string upAnime = "PlayerUp";
    public static string downAnime = "PlayerDown";
    public static string rightAnime = "PlayerRight";
    public static string leftAnime = "PlayerLeft";
    public static string upStop = "StopUp";
    public static string downStop = "StopDown";
    public static string rightStop = "StopRight";
    public static string leftStop = "StopLeft";

    //前のアニメ
    public static string oldAnime = "";
    //次のアニメ
    private string nextAnime = "";

    public static bool ismoving = false;

    private Vector2 axis; //方向宣言
    private bool pushX = false;
    private bool pushY = false;

    Rigidbody2D rbody; //Rigidbody2D


    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2Dの入手
        rbody = GetComponent<Rigidbody2D>();
        //アニメ
        oldAnime = downAnime;
        nextAnime = downStop;
    }

    // Update is called once per frame
    void Update()
    {
        if (TextManager.isTalking == true || FloorManager.isWarping == true || ismoving == true)
        {
            axis.x = 0.0f;
            axis.y = 0.0f;
        }
        else
        {
            axis.x = Input.GetAxisRaw("Horizontal");
            axis.y = Input.GetAxisRaw("Vertical");
        }
        
        //上下キーのみが入った場合
        if (axis.x == 0.0f && axis.y != 0.0f)
        {
            pushX = false;
            pushY = true;
            if(axis.y < 0.0f)
            {
                nextAnime = downAnime;
            }
            else if(axis.y > 0.0f)
            {
                nextAnime = upAnime;
            }
        }     
        //左右キーのみが入った場合
        else if (axis.x != 0.0f && axis.y == 0.0f)
        {
            pushX = true;
            pushY = false;
            if(axis.x < 0.0f)
            {
                nextAnime = leftAnime;
            }
            else if(axis.x > 0.0f)
            {
                nextAnime = rightAnime;
            }
        }
        //移動停止中のアニメ変更
        else if (axis.x == 0.0f && axis.y == 0.0f)
        {
            pushX = false;
            pushY = false;
            if(oldAnime == downAnime)
            {
                nextAnime = downStop;
            }
            else if(oldAnime == upAnime)
            {
                nextAnime = upStop;
            }                
            else if(oldAnime == rightAnime)
            {
                nextAnime = rightStop;
                
            }                
            else if(oldAnime == leftAnime)
            {
                nextAnime = leftStop;
            }
                
        }
        //両方のキーが入った場合
        else if (axis.x != 0.0f && axis.y != 0.0f)
        {
            if (pushX == true && pushY == false)
            {
                axis.x = 0.0f;
                pushX = true;
                pushY = true;
                if (axis.y < 0.0f)
                {
                    nextAnime = downAnime;
                }
                else
                {
                    nextAnime = upAnime;
                }
            }
            else if(pushX == true && pushY == true)
            {
                pushX = true;
                pushY = true;
                if(oldAnime == rightAnime || oldAnime == leftAnime)
                {
                    axis.y = 0.0f;
                    if (axis.x < 0.0f)
                    {
                        nextAnime = leftAnime;
                    }
                    else if(axis.x > 0.0f)
                    {
                        nextAnime = rightAnime;
                    }
                }
                else
                {
                    axis.x = 0.0f;
                    if (axis.y < 0.0f)
                    {
                        nextAnime = downAnime;
                    }
                    else if(axis.y > 0.0f)
                    {
                        nextAnime = upAnime;
                    }
                }
            }
            else
            {
                axis.y = 0.0f;
                pushX = true;
                pushY = true;
                if (axis.x < 0.0f)
                {
                    nextAnime = leftAnime;
                }
                else if(axis.x > 0.0f)
                {
                    nextAnime = rightAnime;
                }
            }
        }
                
        
        if(oldAnime != nextAnime)
        {
            oldAnime = nextAnime;
            GetComponent<Animator>().Play(nextAnime);
        }
    }

    void FixedUpdate()
    {
        //移動速度計算
        rbody.velocity = axis.normalized * speed;


    }
}
