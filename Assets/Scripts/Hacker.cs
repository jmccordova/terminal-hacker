using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    // Game data
    private int wrongCount = 0;
    private int correctAnswerCount = 0;
    private static string[] easyWords = new string[]{ "terminal", "killer", "fox", "thread", "quiet", "twin", "hostile", "diamond", "blast", "war" };
    private static string[] intermediateWords = new string[]{ "infrastructure", "established", "battlefield", "interactive", "discriminate", "continental", "mother base", "genetic engineering", "responsibility", "headquarters" };
    private static string[] difficultWords = new string[]{ "Talk the Talk", "Metal Gear", "Tough It Out", "Go Out On a Limb", "Keep Your Eyes Peeled", "Control History", "No Hero", "Snake Eater", "War Has Changed", "Play Me Like A fDamn Fiddle" };
    private static string correctWord;

    // Game config
    enum Screen {IntroScreen, BriefingScreen, GameScreen, WinScreen, TimeoutScreen, CheckAnswer, GameOverScreen};
    enum Level {Easy, Intermediate, Difficult};
    Screen currentScreen = Screen.IntroScreen;
    Level currentLevel = Level.Easy;
    private static float timeRemaining = 600;
    private static bool isTimerRunning;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            } else
            {
                isTimerRunning = false;
                timeRemaining = 0;
                currentScreen = Screen.GameOverScreen;
                Debug.Log(timeRemaining);
            }
        }
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
        else if (currentScreen == Screen.GameOverScreen)
        {
            GameOver();
        }
        else if (currentScreen == Screen.WinScreen)
        {
            ShowWinScreen();
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
        if (input == null || input.Length == 0 || input.ToLower().Contains("again") || input.ToLower().Contains("ok") || input.ToLower().Contains("fine"))
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
            Terminal.WriteLine("Snake: '...time runs out'?");
            Terminal.WriteLine("Otacon: Just to make it dramatic.");
            Terminal.WriteLine("Snake: Hnh.");
            Terminal.WriteLine("Otacon: Let's do this.");
            currentScreen = Screen.GameScreen;
            isTimerRunning = true;
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
                ShowIntermediateLevel(input);
                break;
            case Level.Difficult:
                ShowDifficultLevel(input);
                break;
        }

        currentScreen = Screen.CheckAnswer;
    }

    void ShowEasyLevel(string input)
    {
        System.Random rnd = new System.Random();
        // get a random word from the list
        if (easyWords.Length <= 0) { 
            currentScreen = Screen.GameOverScreen; 
        }

        if (currentScreen != Screen.GameOverScreen) {
            correctWord = easyWords[rnd.Next(0, easyWords.Length - 1)];
            easyWords = easyWords.Where(o => o != correctWord).ToArray();
            Terminal.WriteLine("+-------------------------+");
            Terminal.WriteLine(StringExtension.Anagram(correctWord));
            Terminal.WriteLine("+-------------------------+");
        }
       
    }

    void ShowIntermediateLevel(string input)
    {
        System.Random rnd = new System.Random();
        // get a random word from the list
        if (intermediateWords.Length <= 0)
        {
            currentScreen = Screen.GameOverScreen;
        }

        if (currentScreen != Screen.GameOverScreen)
        {
            correctWord = intermediateWords[rnd.Next(0, intermediateWords.Length - 1)];
            intermediateWords = intermediateWords.Where(o => o != correctWord).ToArray();
            Terminal.WriteLine("+-------------------------+");
            Terminal.WriteLine(StringExtension.Anagram(correctWord));
            Terminal.WriteLine("+-------------------------+");
        }

    }

    void ShowDifficultLevel(string input)
    {
        System.Random rnd = new System.Random();
        // get a random word from the list
        if (difficultWords.Length <= 0)
        {
            currentScreen = Screen.GameOverScreen;
        }

        if (currentScreen != Screen.GameOverScreen)
        {
            correctWord = difficultWords[rnd.Next(0, difficultWords.Length - 1)];
            difficultWords = difficultWords.Where(o => o != correctWord).ToArray();
            Terminal.WriteLine("+-------------------------+");
            Terminal.WriteLine(StringExtension.Anagram(correctWord));
            Terminal.WriteLine("+-------------------------+");
        }

    }

    /**
     * Compare answer from the user to the system's chosen word
     */
    void VerifyResult(string answer)
    {
        if (correctWord == answer)
        {
            ++correctAnswerCount;

            Terminal.WriteLine("System: " + correctAnswerCount + " of 5 passwords unlocked.");

            if (correctAnswerCount >= 5)
            {
                // update to next level once correct answer reaches to 5
                // if already in the difficult level, show finale
                if (currentLevel == Level.Easy)
                {
                    Terminal.WriteLine("Otacon: Oh! I think I heard one of the doors.");
                    Terminal.WriteLine("Otacon: Keep going, Snake!");
                    currentLevel = Level.Intermediate;
                }
                else if (currentLevel == Level.Intermediate)
                {
                    Terminal.WriteLine("Otacon: The door's light turned green!");
                    Terminal.WriteLine("Otacon: Last one, Snake.");
                    currentLevel = Level.Difficult;
                }
                else if (currentLevel == Level.Difficult)
                {
                    currentScreen = Screen.WinScreen;
                }

                correctAnswerCount = 0;
            }

            wrongCount = 0;
        } else
        {
            Terminal.WriteLine("System: Incorrect input.");

            wrongCount++;
            if(wrongCount == 3)
                Terminal.WriteLine("Otacon: Ugh. Snake.");
            else if(wrongCount == 5)
                Terminal.WriteLine("Otacon: Really? You can do better than that.");
            else if(wrongCount == 7)
                Terminal.WriteLine("Otacon: I'm done.");
            else if(wrongCount == 10)
                Terminal.WriteLine("Otacon: I REALLY am done, Snake.");
        }

        currentScreen = Screen.GameScreen;
        Terminal.WriteLine("Otacon: Not to put pressure on you, Snake, but you only have " + GetRemainingMinutes() + " minutes and " + GetRemainingSeconds() + " seconds left.");
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
        } else if (currentScreen == Screen.CheckAnswer)
        {
            VerifyResult(input);
        } else if (currentScreen == Screen.GameOverScreen)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("Otacon: Snake. Snake?");
        Terminal.WriteLine("Ş̶͇̣̺̳̪̲̓̚Ṋ̸̡̠̝̦͓̣͈͕̪̄̍͗͒͂̕̚ͅA̶̡̡̳̠͓̬͉͑͌̌̀̏̊̿̿̑̓̕̚͝͠ͅḲ̸̔̏̈̽̄͗̉͝Ȩ̷̨̫̣͔̙͇̩͉̮̂̿̃́̍͐̑̉́́̚̚");
        Terminal.WriteLine("...");
        Terminal.WriteLine("--- Connection lost ---");
        isTimerRunning = false;
        timeRemaining = 0;
    }

    void ShowWinScreen()
    {
        isTimerRunning = false;
        timeRemaining = 0;
    }

    private static string GetRemainingMinutes()
    {
        return Mathf.FloorToInt(timeRemaining / 60).ToString();
    }

    private static string GetRemainingSeconds()
    {
        return Mathf.FloorToInt(timeRemaining % 60).ToString();
    }
}
