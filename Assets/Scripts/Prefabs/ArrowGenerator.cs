using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    public GameObject arrowPrefab;
    float spawn = 2.0f;
    float delta = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameDirector.s_State == GameState.InGame)
        {
            this.delta += Time.deltaTime;

            if (this.delta > this.spawn)
            {
                this.delta = 0f;
                GameObject arrow = Instantiate(arrowPrefab) as GameObject;

                int dropPos = Random.Range(-2, 3);  //-2 ~ 2 까지 값
                arrow.GetComponent<ArrowController>().Set(dropPos);

                spawn = 2.0f - (GameDirector.level * 0.2f); // 난이도 조정 - 스폰 주기 빠르게
                if (spawn < 0.5f)
                    spawn = 0.5f;
            }
        } // if (GameDirector.s_State == GameState.InGame)
        else
        {
            this.delta = 0f;
            spawn = 2.0f;
        }
    } //void Update()
}
