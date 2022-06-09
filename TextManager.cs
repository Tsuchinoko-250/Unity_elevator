using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    //立ち絵の種類の宣言
    public enum standimage
    {
        None,       //無し
        eisya,      //エイシャ
        hakase,     //博士
        megu,       //メグ
        elevgirl,   //エレベーターガール
        eisyaC,     //子どもエイシャ
        eisyaR      //画面左配置のエイシャ（未使用）
    }

    //画面のレイアウトに必要な画像、テキストのデータの宣言
    [SerializeField]
    private Text lineText;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text selectText;
    [SerializeField]
    private Image linebox;
    [SerializeField]
    private Image namebox;
    [SerializeField]
    private Image selectbox;
    [SerializeField]
    private Image selectIconY;
    [SerializeField]
    private Image selectIconN;
    [SerializeField]
    private Image StandImageR;
    [SerializeField]
    private Transform StandImagepos;
    [SerializeField]
    private float textspeed;

    //1文ずつに分けられた文章の格納先
    private static string[] linesplit;

    //台詞の主と文自体を分けた格納先
    private static string[] textsplit;

    //疑問文の選択肢を分けた格納先
    private static string[] answersplit;

    //会話中フラグ
    public static bool isTalking = false;
    private bool isShowing = false;
    private bool isAnswering = false;

    //便利な変数群
    private int i;
    private float j;
    private float k;

    //立ち絵のフェードアウトとインの速度（弄ると位置座標がずれるので触らないこと）
    private float fadespeed = 0.1f;

    //外部から受け取った文章データ
    public static string receivetext;
    //疑問文の場合答えによって変わるデータ（など）
    private static string receiveanswertrue;
    private static string receiveanswerfalse;
    private static bool answercounter;
    private static bool questiontrigger = false;

    //テスト用の文章データ（使用していない）
    private string temporary;
    //文章起動用フラグ
    public static bool textboot = false;

    //列挙型のstandimage型、立ち絵の情報を管理する
    private standimage nextstand;
    private standimage nowstand;


    //渡された文字列を分解する
    private void lineloop(string text)
    {
        //変数i,jの初期化
        i = 0;
        j = 0.0f;

        //会話中フラグの活
        isTalking = true;

        //文字を入れるウィンドウの表示
        linebox.enabled = true;
        namebox.enabled = true;

        //現在の立ち絵を"無し"状態
        nowstand = standimage.None;
        

        //「@」が文中にあるかの判断
        if(text.Contains("@"))
        {
            //「@」で選択したのちの文章を分けて格納する
            answersplit = text.Split('@');
            text = answersplit[0];
            receiveanswertrue = answersplit[1];
            receiveanswerfalse = answersplit[2];
        }

        //「&」が文中にあるかの判断
        if(text.Contains("&"))
        {
            //文を'&'でわける
            linesplit = text.Split('&');

            //分けた文の先頭の配列をreadlineに渡す
            readline(linesplit[0]);
        }
        else
        {
            //ない場合そのまま文を配列の先頭に入れる
            linesplit = new string[1];
            linesplit[0] = text;
            readline(linesplit[0]);
        }

    }

    //渡された一文を発言者と台詞に分ける
    private void readline(string text)
    {
        //EventCallで会話中にイベントを発生させる
        if (text.StartsWith("EventCall"))
        {
            //指定のイベントを渡し、イベントフラグを活
            EventMaster.Eventcounter = int.Parse(text.Substring(9, 2));
            EventMaster.Eventtrigger = true;

            //次の文を表示させる（イベントコールのあとには必ず文を入れること）
            i++;
            lineText.text = "";
            readline(linesplit[i]);
        }
        else if(text.Contains("_"))
        {
            //文字を'_'でわける
            textsplit = text.Split('_');

            string name = textsplit[0];
            string line = textsplit[1];

            //選択肢の発生フラグ
            if (name.StartsWith("?"))
            {
                questiontrigger = true;
                name = name.TrimStart('?');
            }

            //特定のフラグをオンにする（最後の文推奨）
            if (name.StartsWith("F"))
            {
                name = name.TrimStart('F');
                FlagTerminal.Flagcounter = int.Parse(name.Substring(0, 2));
                name = name.Remove(0, 2);
                FlagTerminal.Flagtrigger = true;
            }

            //名前によってnextstandの変更
            if (name == "エイシャ")
            {
                nextstand = standimage.eisya;
            }
            else if (name == "エイシャC")
            {
                name = name.TrimEnd('C');
                nextstand = standimage.eisyaC;
            }
            else if (name == "博士")
            {
                nextstand = standimage.hakase;
            }
            else if (name == "メグ")
            {
                nextstand = standimage.megu;
            }
            else if (name == "エレベーターガール")
            {
                nextstand = standimage.elevgirl;
            }
            else
            {
                nextstand = standimage.None;
            }

            standchanger();

            nameText.text = name;
            lineText.text = " ";

            //文字表示中フラグを活
            isShowing = true;
            StartCoroutine(showline(line));
        }

    }

    //立ち絵の変更
    private void standchanger()
    {
        //次の立ち絵が無しの場合
        if(nextstand == standimage.None)
        {
            //なおかつ前の立ち絵が無しで無い場合
            if (nowstand != standimage.None)
            {
                StartCoroutine(removestandimage());
            }

        }
        else  //立ち絵がある場合
        {
            k = 0f;
            if(nowstand == standimage.None)
            {
                //フェードしながら左から表示
                StandImagepos.transform.position = new Vector3(1693f, 400, 0);
                changeimagesprite();
                StartCoroutine(bootstandimage());
            }
            else if(nowstand != nextstand)
            {
                //左から表示
                StandImagepos.transform.position = new Vector3(1693f, 400, 0);
                changeimagesprite();
                StartCoroutine(changestandimage());
            }

        }
        //現在の立ち絵を変更
        nowstand = nextstand;
    }

    IEnumerator removestandimage()
    {
        if (j > 0)
        {
            StandImageR.color = new Color(1f, 1f, 1f, j);
            j -= fadespeed;
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(removestandimage());
        }

    }

    IEnumerator bootstandimage()
    {
        if (j < 1.2)
        {
            StandImagepos.transform.position = new Vector3(1693f - k, 400, 0);
            k += 3f;
            //Debug.Log("a");
            StandImageR.color = new Color(1f, 1f, 1f, j);
            j += fadespeed;
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(bootstandimage());
        }

    }

    IEnumerator changestandimage()
    {
        if(k <= 33)
        {
            StandImagepos.transform.position = new Vector3(1693f - k, 400, 0);
            k += 3f;
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(changestandimage());
        }

    }

    private void changeimagesprite()
    {
        switch (nextstand)
        {
            case standimage.eisya:
                StandImageR.sprite = Resources.Load<Sprite>("eisya_02R");
                break;

            case standimage.eisyaC:
                StandImageR.sprite = Resources.Load<Sprite>("child_01");
                break;

            case standimage.elevgirl:
                StandImageR.sprite = Resources.Load<Sprite>("elevgirl_03");
                break;

            case standimage.hakase:
                StandImageR.sprite = Resources.Load<Sprite>("hakase_01");
                break;

            case standimage.megu:
                StandImageR.sprite = Resources.Load<Sprite>("megu_04");
                break;
        }

    }

    IEnumerator showline(string line)
    {
        //1文字ずつ表示
        foreach (char c in line)
        {
            lineText.text += c;
            //textspeedの分だけ間をあける
            yield return new WaitForSeconds(textspeed);
        }

        //文字表示中フラグを切る
        isShowing = false;
    }

    //次の文の表示
    private void gotonextline()
    {
        if (i < linesplit.Length - 1)
        {
            i++;
            lineText.text = "";
            readline(linesplit[i]);

        }
        else //最後の文だったとき
        {
            lineText.text = "";
            nameText.text = "";


            linebox.enabled = false;
            namebox.enabled = false;
            StartCoroutine(endtext());
        }

    }

    IEnumerator endtext()
    {
        StartCoroutine(removestandimage());
        yield return new WaitForSeconds(0.1f);
        isTalking = false;
    }

    private void selectstart()
    {
        selectbox.enabled = true;
        selectText.enabled = true;
        selectIconY.enabled = true;
        selectIconN.enabled = false;
        answercounter = true;
    }

    private void selectend()
    {
        selectbox.enabled = false;
        selectText.enabled = false;
        selectIconY.enabled = false;
        selectIconN.enabled = false;
        questiontrigger = false;
        isShowing = false;
        isAnswering = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //textboot(外部から操作可能)に入力があったときに動く
        if (textboot == true)
        {
            //textbootを切る
            textboot = false;
            //receivetext(外部から受け取った文字列)をlineloopに渡して動かす
            lineloop(receivetext);
        }

        if (isTalking == true)
        {
            if (isShowing == false)
            {
                if (Input.GetKeyDown(KeyCode.Space) && questiontrigger == false)
                {
                    //文字表示が終わっているときスペースキーで次の文へ
                    gotonextline();
                }

                if(questiontrigger == true) //選択肢の発生フラグがオンの場合
                {
                    if (isAnswering == false)
                    {
                        isAnswering = true;
                        selectstart();
                    }
                    else if (isAnswering == true)
                    {
                        if (Input.GetKeyDown(KeyCode.UpArrow) && answercounter == false)
                        {
                            answercounter = true;
                            selectIconY.enabled = true;
                            selectIconN.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow) && answercounter == true)
                        {
                            answercounter = false;
                            selectIconY.enabled = false;
                            selectIconN.enabled = true;
                        }
                        else if (Input.GetKeyDown(KeyCode.Space))
                        {
                            selectend();

                            if (answercounter == true)
                            {
                                lineloop(receiveanswertrue);
                            }
                            else
                            {
                                lineloop(receiveanswerfalse);
                            }

                        }

                    }
                }
                
            }
            
        }

    }  
    
}