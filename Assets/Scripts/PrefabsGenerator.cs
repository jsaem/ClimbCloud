using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsGenerator : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject fishPrefab;
    public GameObject warningPrefab;

    [HideInInspector] public float span = 1.0f;
    float delta = 0f;

    [HideInInspector] public float m_DownSpeedCtrl = -0.1f; //전체를 제어하는 낙하 속도

    // Update is called once per frame
    void Update()
    {
        m_DownSpeedCtrl -= (Time.deltaTime * 0.01f);
        if (m_DownSpeedCtrl < -0.3f)
            m_DownSpeedCtrl = -0.3f;

        this.span -= (Time.deltaTime * 0.03f); //DifficultyTime
        if (1.0f < this.span)  //0.2f 딜레이
            this.span = 1.0f;
        if (this.span < 0.1f)
            this.span = 0.1f;

        this.delta += Time.deltaTime;
        if (this.delta > this.span)
        {
            this.delta = 0;
            GameObject go = null;
            int prefabChance = Random.Range(1, 6);  //1 ~ 5 랜덤값 발생

            switch (prefabChance)
            {
                case 1: // 20% for fish
                    go = Instantiate(fishPrefab) as GameObject;
                    break;
                default: // 80% for arrow
                    go = Instantiate(arrowPrefab) as GameObject;
                    go.GetComponent<ArrowController>().m_DownSpeed = m_DownSpeedCtrl;
                    break;
            }
            int px = Random.Range(-2, 3);  //-2 ~ 2 까지 값
            go.transform.position = new Vector3(px, 9, 0);
        }
    }
}
