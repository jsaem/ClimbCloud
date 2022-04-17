using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 480x720 Resolution

public enum GameState
{
    GameStart,
    InGame,
    GameOver
}

public class GameDirector : MonoBehaviour
{
    public static GameState s_State = GameState.GameStart;
    GameObject player;
    public static int level = 0; // 게임 난이도

    [Header("---------Height---------")]
    public Text heightTxt;
    public Text HighestHeightText;
    float height = 0.0f;
    public static float currentHeight = 0.0f; // 현재최고높이
    public static float highestHeight = 0.0f; // 최고기록높이

    [Header("---------CountDown---------")]
    public GameObject GameStartPanel;
    public Text CountDownText;
    float countDown = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        s_State = GameState.GameStart;
        level = 0;

        // -- height
        Load();
        this.player = GameObject.Find("cat");
        currentHeight = 0.0f;
        // -- height

        // -- countdown
        GameStartPanel.SetActive(true);
        countDown = 4.0f;
        // -- countdown
    }

    // Update is called once per frame
    void Update()
    {
        switch (s_State)
        {
            case GameState.GameStart:
                Time.timeScale = 1.0f;

                // -- countdown
                countDown -= Time.deltaTime;

                if (0 <= countDown)
                    CountDownText.text = ((int)countDown).ToString();
                else if (-1.0f <= countDown)
                    CountDownText.text = "Start!";
                else
                {
                    s_State = GameState.InGame;
                }
                // -- countdown
                break;

            case GameState.InGame:
                GameStartPanel.SetActive(false);
                Time.timeScale = 1.0f;
                break;

            case GameState.GameOver:
                Time.timeScale = 0.0f;
                SceneManager.LoadScene("GameOverScene");
                break;
        }

        level = (int)(this.player.transform.position.y / 15);

        // -- 높이값 기록
        this.height = this.player.transform.position.y;

        if (this.height < 0)
            this.height = 0.0f;

        if (currentHeight < height)
            currentHeight = height;

        if (highestHeight < currentHeight)
        {
            highestHeight = currentHeight;
            Save();
        }

        heightTxt.text = string.Format("높이: {0:N}", currentHeight);

        if (HighestHeightText != null)
            HighestHeightText.text = string.Format("최고 기록: {0:N}", highestHeight);
    }

    public static void Save()
    {
        PlayerPrefs.SetFloat("HighScore", highestHeight);
    }

    public static void Load()
    {
        highestHeight = PlayerPrefs.GetFloat("HighScore");
    }
}