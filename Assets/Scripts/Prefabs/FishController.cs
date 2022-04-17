using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    GameObject player;
    float destroyDistance = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("cat");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = this.player.transform.position;

        // 일정거리 아래 파괴
        if (transform.position.y < playerPos.y - destroyDistance)
            Destroy(gameObject);
    }
}
