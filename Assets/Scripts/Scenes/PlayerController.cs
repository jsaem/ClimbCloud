using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    float jumpForce = 680.0f;
    //float walkForce = 30.0f;
    //float maxWalkSpeed = 2.0f;
    float walkSpeed = 3.0f;

    [Header("----------Heart----------")]
    float hp;
    public Image[] hpImage;

    // --> player에 두개의 collider가 있을 경우
    // GameObject m_OverlapBlock = null; // 보상이나 화살 두세번 연속 충돌 방지 변수

    bool isCloudColl = true;

    // Start is called before the first frame update
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        Application.targetFrameRate = 60;   //실행 프레임 속도 60프레임으로 고정 시키기.. 코드
        QualitySettings.vSyncCount = 0;     //모니터 주사율(플레임율)이 다른 컴퓨터일 경우 캐릭터 조작시 빠르게 움직일 수 있다.

        ResetPlayer();
    }

    void ResetPlayer()
    {
        hp = 3.0f;
        CheckHPImg();
        transform.position = new Vector3(0, -1.0f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameDirector.s_State != GameState.InGame)
        {
            this.animator.speed = 0.0f;
            return;
        }

        // 점프
        isCloudColl = AABBIntersection();
        if (Input.GetKeyDown(KeyCode.Space) &&
            this.rigid2D.velocity.y <= 0.4f &&
            isCloudColl == true) // && 구름의 위를 밟고 있으면
        {
            this.animator.SetTrigger("JumpTrigger");
            this.rigid2D.AddForce(transform.up * this.jumpForce);
        }

        

        // 좌우이동
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;

        // 플레이어 속도
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        /*
        // 스피드 제한
        if (speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }
        */

        //캐릭터 이동
        this.rigid2D.velocity = new Vector2(key * this.walkSpeed,
                                            this.rigid2D.velocity.y);

        // 움직이는 방향에 따라 이미지 반전
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        // 플레이어의 속도에 맞춰 애니메이션 속도를 바꾼다.
        if (this.rigid2D.velocity.y == 0)
            this.animator.speed = speedx / 2.0f;
        else
            this.animator.speed = 1.0f;
        

        // 플레이어가 화면 밖으로 나갔다면 처음부터
        if (transform.position.y < -10.0f)
            GameDirector.s_State = GameState.GameStart;

        // 화면밖으로 못나가게 하기
        Vector3 pos = transform.position;
        if (pos.x < -2.5f) pos.x = -2.5f;
        if (pos.x > 2.5f) pos.x = 2.5f;
        transform.position = pos;

    } // void Update()

    // 골 도착
    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.gameObject.name);
        //--> 어떤 오브젝트와 충돌했는지 확인

        if (other.gameObject.name.Contains("flag"))
            SceneManager.LoadScene("ClearScene");

        else if (other.gameObject.name.Contains("WaterBox"))
            GameDirector.s_State = GameState.GameOver;

        else if (other.gameObject.name.Contains("fishPrefab"))
        {
            hp += 0.5f;
            if (3.0f < hp)
                hp = 3.0f;
            CheckHPImg();
            Destroy(other.gameObject);

        } else if (other.gameObject.name.Contains("arrowPrefab"))
        {
            Damage();
            /* // --> player에 두개의 collider가 있을 경우
            if (m_OverlapBlock != other.gameObject)
            {
                Damage();
                m_OverlapBlock = other.gameObject;
            }
            */
            Destroy(other.gameObject);

        }
    }

    public void Damage()
    {
        hp -= 1.0f;
        CheckHPImg();

        if (hp <= 0.0f)
            GameDirector.s_State = GameState.GameOver;
    }

    void CheckHPImg()
    {
        float a_CacHp = 0.0f;

        for (int i = 0; i < hpImage.Length; i++)
        {
            a_CacHp = hp - (float)i;
            if (a_CacHp < 0.0f)
                a_CacHp = 0.0f;

            if (0.45f < a_CacHp && a_CacHp < 0.55f) // offset 주기 - 하트 정중앙이 잘리게끔
                a_CacHp = 0.445f;

            hpImage[i].fillAmount = a_CacHp;
        }
    } //void CheckHPImg()

    bool AABBIntersection()
    {
        float a_CcSize = this.gameObject.GetComponent<CircleCollider2D>().radius;
        Vector2 a_OffSet = this.gameObject.GetComponent<CircleCollider2D>().offset;

        a_CcSize = a_CcSize * this.transform.localScale.y; // 충돌박스의 크기

        Vector2 a_CcMinPos;
        a_CcMinPos.x = this.transform.position.x - a_CcSize + a_OffSet.x;
        a_CcMinPos.y = this.transform.position.y - a_CcSize + a_OffSet.y;

        Vector2 a_CcMaxPos;
        a_CcMaxPos.x = this.transform.position.x + a_CcSize + a_OffSet.x;
        a_CcMaxPos.y = this.transform.position.y + a_CcSize + a_OffSet.y;

        GameObject[] a_CloudList;
        a_CloudList = GameObject.FindGameObjectsWithTag("CloudObj");
        Vector2 a_BxColSize = Vector2.zero;
        Vector2 a_BxOffSet = Vector2.zero;
        Vector2 a_BxMinPos = Vector2.zero;
        Vector2 a_BxMaxPos = Vector2.zero;

        foreach (GameObject a_CdObj in a_CloudList)
        {
            a_BxColSize = a_CdObj.GetComponent<BoxCollider2D>().size;
            a_BxColSize.x = a_BxColSize.x * 1.05f;
            a_BxColSize.y = a_BxColSize.y * 0.5f;
            a_BxOffSet = a_CdObj.GetComponent<BoxCollider2D>().offset;
            a_BxOffSet.y = a_BxOffSet.y + 0.15f;

            a_BxColSize.x = a_BxColSize.x * a_CdObj.transform.localScale.x;
            a_BxColSize.y = a_BxColSize.y * a_CdObj.transform.localScale.y; // 충돌박스의 크기

            a_BxMinPos.x = a_CdObj.transform.position.x - (a_BxColSize.x / 2.0f) + a_BxOffSet.x;
            a_BxMinPos.y = a_CdObj.transform.position.y - (a_BxColSize.y / 2.0f) + a_BxOffSet.y;

            a_BxMaxPos.x = a_CdObj.transform.position.x + (a_BxColSize.x / 2.0f) + a_BxOffSet.x;
            a_BxMaxPos.y = a_CdObj.transform.position.y + (a_BxColSize.y / 2.0f) + a_BxOffSet.y;

            if (a_CcMaxPos.x < a_BxMinPos.x || a_CcMinPos.x > a_BxMaxPos.x)
                continue;

            if (a_CcMaxPos.y < a_BxMinPos.y || a_CcMinPos.y > a_BxMaxPos.y)
                continue;

            return true; // 하나라도 충돌이 되면
        }

        return false;
    }
}
