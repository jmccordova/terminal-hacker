using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Hacker : MonoBehaviour
{
    // Game data
    private int wrongCount = 0;
    private int correctAnswerCount = 0;
    private static string[] easyWords = new string[]{ "terminal", "killer", "fox", "thread", "quiet", "twin", "hostile", "diamond", "blast", "stealth" };
    private static string[] intermediateWords = new string[]{ "infrastructure", "established", "battlefield", "interactive", "discriminate", "continental", "mother base", "genetic engineering", "responsibility", "headquarters" };
    private static string[] difficultWords = new string[]{ "Talk the Talk", "Metal Gear", "Tough It Out", "Go Out On a Limb", "Keep Your Eyes Peeled", "Control History", "No Hero", "Snake Eater", "War Has Changed", "Play Me Like A Damn Fiddle" };
    private static string correctWord;

    // Game config
    enum Screen { IntroScreen, BriefingScreen, GameScreen, WinScreen, TimeoutScreen, CheckAnswer, GameOverScreen, RestartOrEndScreen };
    enum Level {Easy, Intermediate, Difficult};
    Screen currentScreen = Screen.IntroScreen;
    Level currentLevel = Level.Easy;
    private static float timeRemaining = 420;
    private static bool isTimerRunning;
    private static bool showMessage = false;
    private static Color origColor;
    private static GameObject monitorScreen;
    private static GameObject codecScreen;
    private static Text screenText;


    // Start is called before the first frame update
    void Start()
    {
        monitorScreen = GameObject.Find("WM2000");
        codecScreen = GameObject.Find("FinaleVideo");
        screenText = GameObject.Find("Text").GetComponent<Text>();
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

                // "Technical" mishaps that look like being hacked
                // 6 mins - screen flickers
                // 5 mins 30 sec - screen offs for a longer time
                // 4 mins 50 sec - color it red
                // 3 mins 45 sec - flood

                int t = (int)timeRemaining;
                
                if(t <= 360 && t >= 356)    // Turn off screen at 6:00 mark
                {
                    switch(t)
                    {
                        case 360:
                            Debug.Log("Should be off");
                            screenText.enabled = false;
                            break;
                        case 358:
                            screenText.enabled = true;
                            break;
                        case 357:
                            if (!showMessage)
                            {
                                Terminal.WriteLine("Snake: Hmm.");
                                Terminal.WriteLine("Otacon: Something wrong?");
                                Terminal.WriteLine("Snake: Nothing.");
                                Terminal.WriteLine("Otacon: Oh. Ok. Get back to guessing, then.");
                                showMessage = true;
                            }
                            break;
                        case 356:
                            showMessage = false;
                            break;
                    }
                } else if (t <= 330 && t >= 318)    // Turn off screen at 5:30 mark
                {
                    switch (t)
                    {
                        case 330:
                            screenText.enabled = false;
                            break;
                        case 320:
                            screenText.enabled = true;
                            break;
                        case 319:
                            if (!showMessage)
                            {
                                Terminal.WriteLine("Otacon: SNAKEEEEEEEEE!");
                                Terminal.WriteLine("Otacon: I lost you for 10 seconds! What happened?");
                                Terminal.WriteLine("Snake: The screen went off suddenly. Are you sure the time's just for drama only?");
                                Terminal.WriteLine("Otacon: I don't know anymore! HURRY UP!");
                                showMessage = true;
                            }
                            break;
                        case 318:
                            showMessage = false;
                            break;
                    }
                } else if (t <= 290 && t >= 280)    // Color the text red at 4:50 mark
                {
                    switch(t)
                    {
                        case 290:
                            origColor = screenText.color;
                            screenText.color = Color.red;
                            if (!showMessage)
                            {
                                Terminal.WriteLine("SNAKE.");
                                showMessage = true;
                            }
                            break;
                        case 280:
                            screenText.color = origColor;
                            showMessage = false;
                            break;
                    }
                } else if (t <= 120 && t >= 115)    // Flood message at 2:00
                {
                    screenText.color = Color.red;
                    switch (t)
                    {
                        case 120:
                            Terminal.WriteLine("S");
                            break;
                        case 119:
                            Terminal.WriteLine("S N");
                            break;
                        case 118:
                            Terminal.WriteLine("S N A");
                            break;
                        case 117:
                            Terminal.WriteLine("S N A K");
                            break;
                        case 116:
                            Terminal.WriteLine("S N A K E");
                            break;
                    }
                } else if (t <= 60 && t >= 10)
                {
                    screenText.fontSize = t;
                }
            } else
            {
                isTimerRunning = false;
                timeRemaining = 0;
                currentScreen = Screen.GameOverScreen;
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
        else if (currentScreen == Screen.RestartOrEndScreen)
        {
            ShowRestartOrEndScreen();
        }
    }

    void ShowIntro(string input = null)
    {
        if(input == null || input.Length == 0)
        {
            Terminal.WriteLine("Otacon: Snake, it's me, Otacon. Have I kept you waiting?");
            Terminal.WriteLine("Otacon: Someone locked this laboratory and my codes are not working.");
            Terminal.WriteLine("Otacon: Radio signal is jammed, too.");
            Terminal.WriteLine("Otacon: I managed to get through the terminal but I don't have the correct access rights.");
            Terminal.WriteLine("Otacon: You seem to have what I need. Will you help me?");
        } else if (input.ToLower().Contains("ok") || input.ToLower().Contains("fine") || input.ToLower().Contains("ye"))
        {
            Terminal.WriteLine("Otacon: I never doubted you, Snake.");
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
        if (currentScreen == Screen.GameScreen)
        {
            switch (currentLevel)
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
    }

    void ShowEasyLevel(string input)
    {
        System.Random rnd = new System.Random();
        // get a random word from the list
        if (easyWords.Length <= 0) { 
            currentScreen = Screen.GameOverScreen; 
        }

        if (currentScreen != Screen.GameOverScreen) {
            correctWord = easyWords[rnd.Next(easyWords.Length)];
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
            correctWord = intermediateWords[rnd.Next(intermediateWords.Length)];
            intermediateWords = intermediateWords.Where(o => o != correctWord).ToArray();
            Terminal.WriteLine("+-------------------------+");
            Terminal.WriteLine(StringExtension.Anagram(correctWord));
            Terminal.WriteLine("+-------------------------+");

            // give three hints on the character on a random index
            if (correctAnswerCount == 0)
            {
                Terminal.WriteLine("Otacon: This is going to get difficult, Snake, so starting here, I'll help you out.");
                Terminal.WriteLine("Snake: You think I'm");
            }

            int hint1 = rnd.Next(correctWord.Length), hint2, hint3;
            do
            {
                hint2 = rnd.Next(correctWord.Length);
                hint3 = rnd.Next(correctWord.Length);
            } while (hint1 == hint2 || hint2 == hint3 || hint1 == hint3);   // repeat randomization until all numbers are different
            
            Terminal.WriteLine("Otacon: The " + GetOrdinal(hint1) + ", " + GetOrdinal(hint2) + ", and " + GetOrdinal(hint3) + " letters are '" + correctWord[hint1] + "', '" + correctWord[hint2] + "', and '" + correctWord[hint3] + "'.");
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
            correctWord = difficultWords[rnd.Next(difficultWords.Length)];
            difficultWords = difficultWords.Where(o => o != correctWord).ToArray();
            Terminal.WriteLine("+-------------------------+");
            Terminal.WriteLine(StringExtension.Anagram(correctWord));
            Terminal.WriteLine("+-------------------------+");

            // give one hint word of a random phrase index
            if (correctAnswerCount == 0)
            {
                Terminal.WriteLine("Otacon: Snake, I remember this one.");
                Terminal.WriteLine("Snake: Harder than the last one? You'll give me clues again?");
                Terminal.WriteLine("Otacon: I didn't mean to- It's not that you're- I-");
                Terminal.WriteLine("Snake: Hnh.");
            }

            int hint1 = rnd.Next(correctWord.Split(' ').Length);
            Terminal.WriteLine("Otacon: " + GetOrdinal(hint1) + " word is " + correctWord.Split(' ')[hint1] + ".");
        }
    }

    /**
     * Compare answer from the user to the system's chosen word
     */
    void VerifyResult(string answer)
    {
        if (correctWord.ToLower() == answer.ToLower())
        {
            ++correctAnswerCount;
            currentScreen = Screen.GameScreen;
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
                    Terminal.WriteLine("Otacon: Suck it, claustrophobia! I'm heading out.");
                    Terminal.WriteLine("Snake: Change pants before going.");
                    Terminal.WriteLine("Otacon: Hehe. Very funny.");
                    currentScreen = Screen.WinScreen;
                }

                wrongCount = 0;
                correctAnswerCount = 0;
            }
        } else
        {
            Terminal.WriteLine("System: Incorrect input.");

            wrongCount++;
            currentScreen = Screen.GameScreen;

            if (wrongCount == 3)
                Terminal.WriteLine("Otacon: Ugh. Snake.");
            else if(wrongCount == 5)
                Terminal.WriteLine("Otacon: Really? You can do better than that.");
            else if(wrongCount == 7)
                Terminal.WriteLine("Otacon: I'm done.");
            else if(wrongCount == 10)
                Terminal.WriteLine("Otacon: I REALLY am done, Snake.");

        }

        // Debug.Log("Wrong: " + wrongCount + " Correct: " + correctAnswerCount);

        // If all the list was exhausted, show game over
        if (wrongCount > 5 && wrongCount + correctAnswerCount == 10)
        {
            currentScreen = Screen.GameOverScreen;
        }

        // Only show timer before the WinScreen
        if(correctAnswerCount <= 5 && currentLevel != Level.Difficult)
            Terminal.WriteLine("Otacon: You only have " + GetRemainingMinutes() + " minutes and " + GetRemainingSeconds() + " seconds left, Snake.");
    }

    void OnUserInput(string input = null)
    {
        if(input != null && input.Length > 0) Terminal.WriteLine("Snake: " + input);

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
        } else if (currentScreen == Screen.WinScreen)
        {
            ShowWinScreen();
        } else if (currentScreen == Screen.RestartOrEndScreen)
        {
            ShowRestartOrEndScreen(input);
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
        currentScreen = Screen.RestartOrEndScreen;
    }

    // Show an image of Otacon and Snake from MGS 1 snippet
    void ShowWinScreen()
    {
        isTimerRunning = false;
        timeRemaining = 0;

        var videoPlayer = GameObject.Find("FinaleVideo").GetComponent<VideoPlayer>();
        monitorScreen.SetActive(false);
        codecScreen.SetActive(true);
        videoPlayer.Play();

        currentScreen = Screen.RestartOrEndScreen;
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        codecScreen.SetActive(false);
        monitorScreen.SetActive(true);
        ShowRestartOrEndScreen();
    }

    void ShowRestartOrEndScreen(string input = null)
    {
        if(input == null || input.Length == 0)
        {
            Terminal.ClearScreen();
            Terminal.WriteLine("System: Do you wish to do it again?");
        } 
        else if (input.ToLower() == "yes")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TerminalScene");
        }
        else if (input.ToLower() == "no")
        {
            Application.Quit();
        } else
        {
            Terminal.WriteLine("System: Either 'yes' or 'no' only.");
        }

    }
    private static string GetRemainingMinutes()
    {
        return Mathf.FloorToInt(timeRemaining / 60).ToString();
    }

    private static string GetRemainingSeconds()
    {
        return Mathf.FloorToInt(timeRemaining % 60).ToString();
    }

    /**
     * Transforms index to an ordinal
     */
    private string GetOrdinal(int num)
    {
        string ordinal = (num + 1).ToString();

        switch ((num + 1) % 100)
        {
            case 11:
            case 12:
            case 13:
                ordinal += "th";
                break;
            default:
                switch ((num + 1) % 10)
                {
                    case 1:
                        ordinal += "st";
                        break;
                    case 2:
                        ordinal += "nd";
                        break;
                    case 3:
                        ordinal += "rd";
                        break;
                    default:
                        ordinal += "th";
                        break;
                }
                break;
        }

        return ordinal;
    }
}
