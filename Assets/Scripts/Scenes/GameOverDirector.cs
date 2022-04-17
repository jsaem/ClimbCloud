using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverDirector : MonoBehaviour
{
    public Button m_Reset_Btn;
    public Text highestRcdText;
    public Text currentRcdText;

    void Start()
    {
        if (GameDirector.highestHeight < GameDirector.currentHeight)
        {
            GameDirector.highestHeight = GameDirector.currentHeight;
            GameDirector.Save();
        }

        if (highestRcdText != null)
        {
            string str = string.Format("최고 기록 : {0:N}", GameDirector.highestHeight);
            highestRcdText.text = str;
        }
        if (currentRcdText != null)
        {
            string str = string.Format("이번 기록 : {0:N}", GameDirector.currentHeight);
            currentRcdText.text = str;
        }

        if (m_Reset_Btn != null)
        {
            m_Reset_Btn.onClick.AddListener(() => {
                SceneManager.LoadScene("GameScene");
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameDirector.s_State = GameState.GameStart;
            SceneManager.LoadScene("GameScene");
        }
            
        if (Input.GetKeyDown(KeyCode.M)) // 저장 값을 모두 초기화
        { 
            PlayerPrefs.DeleteAll();
            GameDirector.s_State = GameState.GameStart;
            SceneManager.LoadScene("GameScene");
        }
    }
}
