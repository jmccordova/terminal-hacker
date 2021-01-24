using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    private static int wrongCount = 0;
    private static int correctAnswerCount = 0;
    enum Screen {IntroScreen, BriefingScreen, GameScreen, WinScreen, TimeoutScreen};
    enum Level {Easy, Intermediate, Difficult};
    Screen currentScreen = Screen.IntroScreen;
    Level currentLevel = Level.Easy;

    // Start is called before the first frame update
    void Start()
    {
        if (currentScreen == Screen.IntroScreen)
        {
            ShowIntro();
        } else if (currentScreen == Screen.BriefingScreen)
        {
            ShowBriefing();
        }
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

    void ShowBriefing()
    {
        Terminal.WriteLine("Otacon: Here's what you should do, Snake.");
        Terminal.WriteLine("Otacon: Ever heard of anagrams?");
    }

    void OnUserInput(string input)
    {
        Terminal.WriteLine("Snake: " + input);
        if (currentScreen == Screen.IntroScreen)
        {
            if (input.ToLower().Contains("ok") || input.ToLower().Contains("fine"))
            {
                Terminal.WriteLine("Otacon: Phew! I owe you one, Snake.");
                Terminal.WriteLine("Otacon: I'll brief you on what you need to do.");
                currentScreen = Screen.BriefingScreen;
            }
            else
            {
                Terminal.WriteLine("Otacon: You don't have a choice, Snake.");
                wrongCount++;
            }

            if (wrongCount > 3)
            {
                Terminal.WriteLine("Otacon: STOP IT. HEAR ME OUT.");
                Terminal.ClearScreen();
                currentScreen = Screen.BriefingScreen;
            }
        } else if (currentScreen == Screen.BriefingScreen)
        {
            if (input.ToLower().Contains("affirmative"))
            {
                Terminal.WriteLine("Otacon: Let's start.");
                currentScreen = Screen.GameScreen;
                Terminal.ClearScreen();
            } else
            {
                if (input.ToLower().Contains("yes"))
                {
                    Terminal.WriteLine("Otacon: Oh, good. This is why I trust you in this.");
                }
                else if (input.ToLower().Contains("no"))
                {
                    Terminal.WriteLine("Otacon: So anagrams are jumbled characters. You need to rearrange them to form a more coherent word.");
                }
                else if (!input.ToLower().Contains("yes") || !input.ToLower().Contains("no") || !input.ToLower().Contains("affirmative"))
                {
                    Terminal.WriteLine("Otacon: I don't understand, Snake. You can just say yes or no.");
                }

                Terminal.WriteLine("Otacon: I need you to solve seven of them before time runs out.");
                Terminal.WriteLine("Snake: Why? What's with the 'time runs out'?");
                Terminal.WriteLine("Otacon: I don't know, Snake. I feel I'm being followed.");
                Terminal.WriteLine("Otacon: Say 'affirmative' if you got that.");
            }
        }
    }
}
