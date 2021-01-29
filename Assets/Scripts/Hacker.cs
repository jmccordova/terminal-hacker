using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    private static int wrongCount = 0;
    private static int correctAnswerCount = 0;
    enum Screen {IntroScreen, BriefingScreen, GameScreen, WinScreen, TimeoutScreen};
    enum Level {Easy, Intermediate, Difficult};
    Screen currentScreen = Screen.IntroScreen;
    Level currentLevel = Level.Easy;
    private static Timer timer = new System.Timers.Timer(600000);   // 10-minute timer
    private static string[] easyWords = { "sample1", "sample2", "sample3", "sample4", "sample5" };
    private static string[] intermediateWords = { "", "", "", "", "" };
    private static string[] difficultWords = { "", "", "", "", "" };

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
        timer.Elapsed += OnTimedEvent;      // elapsed event for the timer
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowMainMenu()
    {
        Terminal.ClearScreen();
        if (currentScreen == Screen.IntroScreen)
        {
            ShowIntro();
        }
        else if (currentScreen == Screen.BriefingScreen)
        {
            ShowBriefing();
        }
    }

    void ShowIntro(string input = null)
    {
        if(input == null || input.Length == 0)
        {
            Terminal.WriteLine("Otacon: Snake, it's me, Otacon. Sorry to keep you waiting.");
            Terminal.WriteLine("Otacon: Someone locked this laboratory and my codes are not working.");
            Terminal.WriteLine("Otacon: Communication is jammed so my radio doesn't work either.");
            Terminal.WriteLine("Otacon: I managed to get through the terminal. Lucky I caught you.");
            Terminal.WriteLine("Otacon: Will you help me, Snake?");
        } else if (input.ToLower().Contains("ok") || input.ToLower().Contains("fine") || input.ToLower().Contains("ye"))
        {
            Terminal.WriteLine("Otacon: Phew! I owe you one, Snake.");
            Terminal.WriteLine("Otacon: I'll brief you on what you need to do.");
            currentScreen = Screen.BriefingScreen;
        } else
        {
            Terminal.WriteLine("Otacon: You don't have a choice, Snake. You can just say 'ok' or 'fine'.");
            wrongCount++;
        }

        if (wrongCount > 3)
        {
            Terminal.WriteLine("Otacon: UGH. Why do I even bother?");
            currentScreen = Screen.BriefingScreen;
        }
    }

    void ShowBriefing(string input = null)
    {
        if (input == null || input.Length == 0 || input.ToLower().Contains("again"))
        {
            Terminal.ClearScreen();
            Terminal.WriteLine("Otacon: Here's what you should do, Snake. Ever heard of anagrams?");
            Terminal.WriteLine("Otacon: You know, whatever.");
            Terminal.WriteLine("Otacon: Anagrams are jumbled characters. You need to rearrange them to form a more coherent word.");
            Terminal.WriteLine("Otacon: I need you to solve seven of them before time runs out.");
            Terminal.WriteLine("Otacon: Say 'affirmative' if you got that.");
        } 
        else if (input.ToLower().Contains("affirmative"))
        {
            Terminal.WriteLine("Snake: But why? What's with the 'time runs out'?");
            Terminal.WriteLine("Otacon: I don't know, Snake. I feel like I'm being followed.");
            Terminal.WriteLine("Otacon: Let's do this.");
            currentScreen = Screen.GameScreen;
            timer.Enabled = true;
        }
        else
        {
            Terminal.WriteLine("Otacon: I don't understand. You can just say 'affirmative' or 'again'.");
        }
    }

    void ShowGameScreen(string input = null)
    {
        Terminal.ClearScreen();
        switch(currentLevel)
        {
            case Level.Easy:
                ShowEasyLevel(input);
                break;
            case Level.Intermediate:
                break;
            case Level.Difficult:
                break;
        }
    }

    void ShowEasyLevel(string input)
    {
        System.Random rnd = new System.Random();
        // get a random word from the list
        string correctWord = easyWords[rnd.Next(0, easyWords.Length -1)];
        easyWords = easyWords.Where(o => o != correctWord).ToArray();
        Terminal.WriteLine("+-------------------------+");
        Terminal.WriteLine(StringExtension.Anagram(correctWord));
        Terminal.WriteLine("+-------------------------+");
    }

    void VerifyResult(string answer)
    {
        if (easyWords.Contains(answer))
        {
            ++correctAnswerCount;
            if (correctAnswerCount > 5 && currentLevel == Level.Easy)
            {
                currentLevel = Level.Intermediate;
                correctAnswerCount = 0;
            }
        }
    }

    void OnUserInput(string input)
    {
        if(input.Length > 0) Terminal.WriteLine("Snake: " + input);
        if (currentScreen == Screen.IntroScreen)
        {
            ShowIntro(input);
            
        } else if (currentScreen == Screen.BriefingScreen)
        {
            ShowBriefing(input);
        } else if (currentScreen == Screen.GameScreen)
        {
            ShowGameScreen(input);
            VerifyResult(input);
        }
    }


    private static void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("Otacon: Snake. Snake?");
        Terminal.WriteLine("Ş̶͇̣̺̳̪̲̓̚Ṋ̸̡̠̝̦͓̣͈͕̪̄̍͗͒͂̕̚ͅA̶̡̡̳̠͓̬͉͑͌̌̀̏̊̿̿̑̓̕̚͝͠ͅḲ̸̔̏̈̽̄͗̉͝Ȩ̷̨̫̣͔̙͇̩͉̮̂̿̃́̍͐̑̉́́̚̚");
        Terminal.WriteLine("...");
        Terminal.WriteLine("--- Connection lost ---");
    }
}
