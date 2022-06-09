using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystems : MonoBehaviour
{
    private bool entry = false;

    [TextArea]
    [SerializeField]
    private string textmessage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && entry == true && TextManager.isTalking == false)
        {
            TextManager.receivetext = textmessage;
            TextManager.textboot = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            entry = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            entry = false;
        }
    }

    
}
