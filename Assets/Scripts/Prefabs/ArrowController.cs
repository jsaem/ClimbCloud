using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowController : MonoBehaviour
{
    GameObject player;

    [Header("----------Arrow----------")]
    [HideInInspector] public float m_DownSpeed = -5f;
    float destroyDistance = 10.0f;

    [Header("----------Warning----------")]
    public Image warningImg;
    float waitTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (this.player == null)
            this.player = GameObject.Find("cat");
    }

    // Update is called once per frame
    void Update()
    {
        // 프레임마다 등속으로 낙하시킨다.
        transform.Translate(0, m_DownSpeed * Time.deltaTime, 0);

        // 일정거리 아래 detroy
        if (transform.position.y < this.player.transform.position.y - destroyDistance)
            Destroy(gameObject);

        if (GameDirector.s_State == GameState.InGame)
        {
            if (0 < waitTime)
            {
                waitTime -= Time.deltaTime;
                warningDirect();
                return;
            }

            if (warningImg.enabled)
                warningImg.enabled = false;

            transform.Translate(0, m_DownSpeed * Time.deltaTime, 0);
            if (transform.position.y < player.transform.position.y - 10.0f)
                Destroy(gameObject);
        } //if (GameDirector.s_State == GameState.InGame)
        else
        {
            Destroy(gameObject);
        }


    }

    public void Set(int a_Pos)
    {
        // (* 1.1f): warning 이미지 중앙을 맞추기 위해서...
        this.player = GameObject.Find("cat");
        
        transform.position = new Vector3(a_Pos * 1.1f,
            this.player.transform.position.y + 10f, 0);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        warningImg.transform.position = new Vector3(screenPos.x, 
                warningImg.transform.position.y, warningImg.transform.position.z);
        // --> x 좌표만 따라가되, y,z 좌표는 그대로
    }


    // -- warning sign
    float alpha = 0.0f; // 투명도 변화 속도
    void warningDirect() // 깜박임 투명도 변화 연출 함수
    {
        if (warningImg.color.a >= 1.0f)
            alpha = -6.0f;
        else if (warningImg.color.a <= 0.0f)
            alpha = 6.0f;

        warningImg.color = new Color(1.0f, 1.0f, 1.0f,
                warningImg.color.a + alpha * Time.deltaTime);
    }
}
