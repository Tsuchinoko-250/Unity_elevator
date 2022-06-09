using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagTerminal : MonoBehaviour
{
    public static bool[] mainflag = new bool[5];
    

    public static int FloorCounter = 1;
    public static bool Flagtrigger = false;
    public static int Flagcounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Flagtrigger == true)
        {
            Flagtrigger = false;
            mainflag[Flagcounter] = true;
        }
    }
}
