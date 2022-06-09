using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    //�����G�̎�ނ̐錾
    public enum standimage
    {
        None,       //����
        eisya,      //�G�C�V��
        hakase,     //���m
        megu,       //���O
        elevgirl,   //�G���x�[�^�[�K�[��
        eisyaC,     //�q�ǂ��G�C�V��
        eisyaR      //��ʍ��z�u�̃G�C�V���i���g�p�j
    }

    //��ʂ̃��C�A�E�g�ɕK�v�ȉ摜�A�e�L�X�g�̃f�[�^�̐錾
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

    //1�����ɕ�����ꂽ���͂̊i�[��
    private static string[] linesplit;

    //�䎌�̎�ƕ����̂𕪂����i�[��
    private static string[] textsplit;

    //�^�╶�̑I�����𕪂����i�[��
    private static string[] answersplit;

    //��b���t���O
    public static bool isTalking = false;
    private bool isShowing = false;
    private bool isAnswering = false;

    //�֗��ȕϐ��Q
    private int i;
    private float j;
    private float k;

    //�����G�̃t�F�[�h�A�E�g�ƃC���̑��x�i�M��ƈʒu���W�������̂ŐG��Ȃ����Ɓj
    private float fadespeed = 0.1f;

    //�O������󂯎�������̓f�[�^
    public static string receivetext;
    //�^�╶�̏ꍇ�����ɂ���ĕς��f�[�^�i�Ȃǁj
    private static string receiveanswertrue;
    private static string receiveanswerfalse;
    private static bool answercounter;
    private static bool questiontrigger = false;

    //�e�X�g�p�̕��̓f�[�^�i�g�p���Ă��Ȃ��j
    private string temporary;
    //���͋N���p�t���O
    public static bool textboot = false;

    //�񋓌^��standimage�^�A�����G�̏����Ǘ�����
    private standimage nextstand;
    private standimage nowstand;


    //�n���ꂽ������𕪉�����
    private void lineloop(string text)
    {
        //�ϐ�i,j�̏�����
        i = 0;
        j = 0.0f;

        //��b���t���O�̊�
        isTalking = true;

        //����������E�B���h�E�̕\��
        linebox.enabled = true;
        namebox.enabled = true;

        //���݂̗����G��"����"���
        nowstand = standimage.None;
        

        //�u@�v�������ɂ��邩�̔��f
        if(text.Contains("@"))
        {
            //�u@�v�őI�������̂��̕��͂𕪂��Ċi�[����
            answersplit = text.Split('@');
            text = answersplit[0];
            receiveanswertrue = answersplit[1];
            receiveanswerfalse = answersplit[2];
        }

        //�u&�v�������ɂ��邩�̔��f
        if(text.Contains("&"))
        {
            //����'&'�ł킯��
            linesplit = text.Split('&');

            //���������̐擪�̔z���readline�ɓn��
            readline(linesplit[0]);
        }
        else
        {
            //�Ȃ��ꍇ���̂܂ܕ���z��̐擪�ɓ����
            linesplit = new string[1];
            linesplit[0] = text;
            readline(linesplit[0]);
        }

    }

    //�n���ꂽ�ꕶ�𔭌��҂Ƒ䎌�ɕ�����
    private void readline(string text)
    {
        //EventCall�ŉ�b���ɃC�x���g�𔭐�������
        if (text.StartsWith("EventCall"))
        {
            //�w��̃C�x���g��n���A�C�x���g�t���O����
            EventMaster.Eventcounter = int.Parse(text.Substring(9, 2));
            EventMaster.Eventtrigger = true;

            //���̕���\��������i�C�x���g�R�[���̂��Ƃɂ͕K���������邱�Ɓj
            i++;
            lineText.text = "";
            readline(linesplit[i]);
        }
        else if(text.Contains("_"))
        {
            //������'_'�ł킯��
            textsplit = text.Split('_');

            string name = textsplit[0];
            string line = textsplit[1];

            //�I�����̔����t���O
            if (name.StartsWith("?"))
            {
                questiontrigger = true;
                name = name.TrimStart('?');
            }

            //����̃t���O���I���ɂ���i�Ō�̕������j
            if (name.StartsWith("F"))
            {
                name = name.TrimStart('F');
                FlagTerminal.Flagcounter = int.Parse(name.Substring(0, 2));
                name = name.Remove(0, 2);
                FlagTerminal.Flagtrigger = true;
            }

            //���O�ɂ����nextstand�̕ύX
            if (name == "�G�C�V��")
            {
                nextstand = standimage.eisya;
            }
            else if (name == "�G�C�V��C")
            {
                name = name.TrimEnd('C');
                nextstand = standimage.eisyaC;
            }
            else if (name == "���m")
            {
                nextstand = standimage.hakase;
            }
            else if (name == "���O")
            {
                nextstand = standimage.megu;
            }
            else if (name == "�G���x�[�^�[�K�[��")
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

            //�����\�����t���O����
            isShowing = true;
            StartCoroutine(showline(line));
        }

    }

    //�����G�̕ύX
    private void standchanger()
    {
        //���̗����G�������̏ꍇ
        if(nextstand == standimage.None)
        {
            //�Ȃ����O�̗����G�������Ŗ����ꍇ
            if (nowstand != standimage.None)
            {
                StartCoroutine(removestandimage());
            }

        }
        else  //�����G������ꍇ
        {
            k = 0f;
            if(nowstand == standimage.None)
            {
                //�t�F�[�h���Ȃ��獶����\��
                StandImagepos.transform.position = new Vector3(1693f, 400, 0);
                changeimagesprite();
                StartCoroutine(bootstandimage());
            }
            else if(nowstand != nextstand)
            {
                //������\��
                StandImagepos.transform.position = new Vector3(1693f, 400, 0);
                changeimagesprite();
                StartCoroutine(changestandimage());
            }

        }
        //���݂̗����G��ύX
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
        //1�������\��
        foreach (char c in line)
        {
            lineText.text += c;
            //textspeed�̕������Ԃ�������
            yield return new WaitForSeconds(textspeed);
        }

        //�����\�����t���O��؂�
        isShowing = false;
    }

    //���̕��̕\��
    private void gotonextline()
    {
        if (i < linesplit.Length - 1)
        {
            i++;
            lineText.text = "";
            readline(linesplit[i]);

        }
        else //�Ō�̕��������Ƃ�
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
        //textboot(�O�����瑀��\)�ɓ��͂��������Ƃ��ɓ���
        if (textboot == true)
        {
            //textboot��؂�
            textboot = false;
            //receivetext(�O������󂯎����������)��lineloop�ɓn���ē�����
            lineloop(receivetext);
        }

        if (isTalking == true)
        {
            if (isShowing == false)
            {
                if (Input.GetKeyDown(KeyCode.Space) && questiontrigger == false)
                {
                    //�����\�����I����Ă���Ƃ��X�y�[�X�L�[�Ŏ��̕���
                    gotonextline();
                }

                if(questiontrigger == true) //�I�����̔����t���O���I���̏ꍇ
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