using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    GameObject player;
    GameDirector m_gameDir;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("player");

        GameObject a_gdObj = GameObject.Find("GameDirector");
        if (a_gdObj != null)
            m_gameDir = a_gdObj.GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        // 프레임마다 등속으로 낙하시킨다.
        //transform.position(0, m_DownSpeed, 0);

        // 화면 밖으로 나오면 오브젝트를 소멸시킨다.
        if (transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        }
    }
}
