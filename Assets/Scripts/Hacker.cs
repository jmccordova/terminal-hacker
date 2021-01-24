using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ShowIntro();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowIntro()
    {
        Terminal.WriteLine("Otacon: Snake, it's me, Otacon. Sorry to keep you waiting.");
        Terminal.WriteLine("Otacon: Someone locked this laboratory and my codes are not working.");
        Terminal.WriteLine("Otacon: Communication is jammed so my radio doesn't work either.");
        Terminal.WriteLine("Otacon: I managed to get through the terminal. Lucky I caught you.");
    }

}
