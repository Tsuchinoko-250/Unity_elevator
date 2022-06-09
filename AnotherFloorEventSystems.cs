using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnotherFloorEventSystems : MonoBehaviour
{
    private bool entry = false;
    private SpriteRenderer SpriteRenderer;

    [TextArea]
    [SerializeField]
    private string F1textmessage;

    [TextArea]
    [SerializeField]
    private string F2textmessage;

    [TextArea]
    [SerializeField]
    private string F3textmessage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && entry == true && TextManager.isTalking == false)
        {
            if(FlagTerminal.FloorCounter == 1)
            {
                TextManager.receivetext = F1textmessage;
                TextManager.textboot = true;
            }
            else if(FlagTerminal.FloorCounter == 2)
            {
                TextManager.receivetext = F2textmessage;
                TextManager.textboot = true;
            }
            else if(FlagTerminal.FloorCounter == 2)
            {
                TextManager.receivetext = F3textmessage;
                TextManager.textboot = true;
            }


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            //Debug.Log("on");
            //âèÇ™åıÇÈÇÊÇ§Ç…Ç∑ÇÈ
            SpriteRenderer.color = new Color(255f, 255f, 255f, 0.75f);
            entry = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            //Debug.Log("off");
            //åıÇ¡ÇƒÇ¢ÇÈâèÇå≥Ç…ñﬂÇ∑
            SpriteRenderer.color = new Color(255f, 255f, 255f, 0f);
            entry = false;
        }
    }
}
