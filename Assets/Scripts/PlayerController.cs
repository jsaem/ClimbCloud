using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    GameDirector m_gameDir;
    float jumpForce = 680.0f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        Application.targetFrameRate = 60;   //실행 프레임 속도 60프레임으로 고정 시키기.. 코드
        QualitySettings.vSyncCount = 0;     //모니터 주사율(플레임율)이 다른 컴퓨터일 경우 캐릭터 조작시 빠르게 움직일 수 있다.

        GameObject a_gdObj = GameObject.Find("GameDirector");
        if (a_gdObj != null)
            m_gameDir = a_gdObj.GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && this.rigid2D.velocity.y == 0)
        {
            this.animator.SetTrigger("JumpTrigger");
            this.rigid2D.AddForce(transform.up * this.jumpForce);
        }

        // 좌우이동
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) 
            key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) 
            key = -1;

        // 플레이어 속도
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        // 스피드 제한
        if (speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }

        // 움직이는 방향에 따라 이미지 반전
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        // 플레이어의 속도에 맞춰 애니메이션 속도를 바꾼다.
        if (this.rigid2D.velocity.y == 0)
        {
            this.animator.speed = speedx / 2.0f;
        } else
        {
            this.animator.speed = 1.0f;
        }
        

        // 플레이어가 화면 밖으로 나갔다면 처음부터
        if (transform.position.y < -10.0f)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    // 골 도착
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "flag")
            SceneManager.LoadScene("ClearScene");

        if (other.tag == "arrow")
        {
            //Debug.Log("고양이와 화살 충돌");
            m_gameDir.DecreaseHeart();
        }

        if (other.tag == "fish")
        {
            //Debug.Log("고양이와 물고기 충돌");
            m_gameDir.IncreaseHeart();
        }

        if (other.tag == "water")
        {
            //Debug.Log("고양이와 물 충돌");
            GameDirector.s_State = GameState.GameOver;
        }
    }
}
