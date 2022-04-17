using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    GameObject player;
    float waterSpeed = 1.0f; // 1초에 1m를 움직이게 한다는 속도의 의미
    float distanceDiff = 8.0f;

    private void Start()
    {
        this.player = GameObject.Find("cat");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameDirector.s_State != GameState.InGame)
            return;

        // player와 거리가 너무 먼 경우 보정
        if (transform.position.y < player.transform.position.y - distanceDiff)
            transform.position = new Vector3(0,
                    player.transform.position.y - distanceDiff, 0);

        // 일정속도로 위로
        if (GameDirector.s_State == GameState.InGame)
        {
            transform.Translate(new Vector3(0, Time.deltaTime * waterSpeed, 0));
        }

        // waterSpeed += 0.02f * Time.deltaTime;
        
    }
}
