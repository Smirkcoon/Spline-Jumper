using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using TMPro;
public class GameManager : MonoBehaviour
{
    public enum GameType
    {
        Levels,
        Loop,
        Change
    }

    public static GameManager inst;

    public GameType NowGameType { get; private set; }

    [SerializeField] private Toggle[] selectGameType;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private Text endGameScoreText;
    [SerializeField] private Text endGameBestScoreText;
    [SerializeField] private Text levelText;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject winText;
    [SerializeField] private Pause pause;
    [SerializeField] private TMP_InputField countSplinesText;

    private int scoreInt;
    private int levelInt = 1;
    private void Start()
    {
        Application.targetFrameRate = 60;
        inst = this;
        bestScoreText.text = "Best Score : " + PlayerPrefs.GetInt("BestScore", 0);
        levelInt = PlayerPrefs.GetInt("LevelInt", 1);
        levelText.text = "Level : " + PlayerPrefs.GetInt("LevelInt", 1);
        Observer.EndGame += EndGame;
        Observer.AddScore += AddScore;
        Observer.StartGame += StartGame;
        countSplinesText.onValueChanged.AddListener(CheckTextInputField);
        
    }

    private void StartGame()
    {
        for (int i = 0; i < selectGameType.Length; i++)
        {
            if (selectGameType[i].isOn)
            {
                AudioManager.inst.Play1ShotMenu();
                SwitchGameType((GameType)i);
                Debug.Log("gameType " + NowGameType);
            }
        }
    }
    private void SwitchGameType(GameType gameType)
	{
        NowGameType = gameType;
        switch (gameType)
        {
            case GameType.Levels:
                Observer.SetCountSplines(levelInt+1);
                break;
            case GameType.Loop:
                Observer.SetCountSplines(0);
                break;
            case GameType.Change:
                CheckTextInputField(countSplinesText.text);
                break;
            default:
                break;  
        }
    }

/// <summary>
/// action on Restart button
/// </summary>
public void RestartSceneButton()
    {
        AudioManager.inst.Play1ShotMenu();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    private void AddScore(int score, Vector3 playerPos)
    {
        AudioManager.inst.Play1ShotGetCoin();
        scoreInt += score;
        scoreText.text = scoreInt.ToString();
    }
    private void EndGame(bool isWin)
    {
        loseText.SetActive(!isWin);
        winText.SetActive(isWin);
        endGamePanel.SetActive(true);
        pause.btnPause.gameObject.SetActive(false);
        if(isWin)
            AudioManager.inst.Play1ShotWinGame();
        else
            AudioManager.inst.Play1ShotLoseGame();

        if (isWin) levelInt++;
        scoreText.gameObject.SetActive(false);
        int i = 0;
        DOTween.To(() => i, x => i = x, scoreInt, 2f)
            .OnUpdate(() => endGameScoreText.text = i.ToString());

        if (scoreInt > PlayerPrefs.GetInt("BestScore", 0))
        {
            endGameBestScoreText.text = scoreInt.ToString();
        }
        else
            endGameBestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        SaveData();
    }
    
    public void SaveData()
    {
        if (scoreInt > PlayerPrefs.GetInt("BestScore", 0))
            PlayerPrefs.SetInt("BestScore", scoreInt);//Best Score
        PlayerPrefs.SetInt("IsMute", AudioManager.IsMute ? 1 : 0);//MuteOn or MuteOff
        if (levelInt > PlayerPrefs.GetInt("LevelInt", 1))
            PlayerPrefs.SetInt("LevelInt", levelInt);//MuteOn or MuteOff
        PlayerPrefs.Save();
    }
    private void CheckTextInputField(string text)
    {
        if (selectGameType[2].isOn)
        {
            if (text.Length > 0)
            {
                int i = int.Parse(text);
                if (i > 0)
                    Observer.SetCountSplines(i);
                else
                {
                    Observer.SetCountSplines(-i);
                    countSplinesText.text = (-i).ToString();
                }
            }
            else
            {
                countSplinesText.text = 0.ToString();
                Observer.SetCountSplines(0);
            }
        }
    }
}
