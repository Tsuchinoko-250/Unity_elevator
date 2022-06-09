using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalFloorChange : MonoBehaviour
{
    [SerializeField]
    private Vector3 vector3;


    private bool ison = false;
    private SpriteRenderer SpriteRenderer ;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ison == true)
        {
            FloorManager.NextPoint = vector3;
            FloorManager.areachangetrigger = true;
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
            ison = true;
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
            ison = false;
        }
    }
}
