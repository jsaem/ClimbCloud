using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 480x720 Resolution

public enum GameState
{
    GameStart = 0,
    InGame = 1,
    GameOver = 2
}

public class GameDirector : MonoBehaviour
{
    static public GameState s_State;

    [Header("---------Heart---------")]
    [HideInInspector] public static int heartAmount = 6;
    GameObject heart1Obj;
    GameObject heart2Obj;
    GameObject heart3Obj;
    Image heart1Img = null;
    Image heart2Img = null;
    Image heart3Img = null;

    [Header("---------CountDown---------")]
    int timer = 0;
    public GameObject GameStartPanel;
    public Text CountDownText;

    // Start is called before the first frame update
    void Start()
    {
        s_State = GameState.GameStart;
        timer = 0;

        this.heart1Obj = GameObject.Find("heart1");
        if (heart1Obj != null)
            heart1Img = this.heart1Obj.GetComponent<Image>();
        this.heart2Obj = GameObject.Find("heart2");
        if (heart2Obj != null)
            heart2Img = this.heart2Obj.GetComponent<Image>();
        this.heart3Obj = GameObject.Find("heart3");
        if (heart3Obj != null)
            heart3Img = this.heart3Obj.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (s_State)
        {
            case GameState.GameStart:
                Time.timeScale = 0.0f;
                if (GameStartPanel != null)
                    GameStartPanel.SetActive(true);
                
                if (timer <= 180)
                {
                    timer++;

                    if (timer> 60) // 3->2
                    {
                        CountDownText.text = "2";
                    }
                    if (timer > 120) // 2->1
                    {
                        CountDownText.text = "1";
                    }
                    if (timer >= 180)// 1->0
                    {
                        CountDownText.text = "0";
                        s_State = GameState.InGame;
                    }
                }
                break;
            case GameState.InGame:
                timer = 0;
                GameStartPanel.SetActive(false);
                Time.timeScale = 1.0f;
                //this.hpGage.SetActive(true);
                break;
            case GameState.GameOver:
                Time.timeScale = 0.0f;
                SceneManager.LoadScene("GameOverScene");
                break;
        }
    }

    public void DecreaseHeart()
    {

        if (heart1Img == null || heart2Img == null || heart3Img == null)
            return;

        heartAmount -= 2;
        Debug.Log("Decreased heart");

        switch (heartAmount)
        {
            case 5:
                heart3Img.fillAmount -= 0.5f;
                break;
            case 4:
                heart3Obj.SetActive(false);
                break;
            case 3:
                heart2Img.fillAmount -= 0.5f;
                break;
            case 2:
                heart2Obj.SetActive(false);
                break;
            case 1:
                heart1Img.fillAmount -= 0.5f;
                break;
            default:
                heart1Obj.SetActive(false);
                GameDirector.s_State = GameState.GameOver;
                break;
        }
    }

    public void IncreaseHeart()
    {
        if (heart1Img == null || heart2Img == null || heart3Img == null)
            return;

        heartAmount += 1;

        switch (heartAmount)
        {
            case 5:
                heart3Img.fillAmount -= 0.5f;
                break;
            case 4:
                heart3Obj.SetActive(false);
                break;
            case 3:
                heart2Img.fillAmount -= 0.5f;
                break;
            case 2:
                heart2Obj.SetActive(false);
                break;
            case 1:
                heart1Img.fillAmount -= 0.5f;
                break;
            default:
                heart1Obj.SetActive(false);
                GameDirector.s_State = GameState.GameOver;
                break;
        }
    }
}