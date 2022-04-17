using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudWaveController : MonoBehaviour
{
    public GameObject[] Cloud;
    public GameObject fishPrefab;
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

        // 일정거리 아래 detroy
        if (transform.position.y < playerPos.y - destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    public void SetHideCloud(int a_Count) // a_Count 몇개를 보이지 않게 할지 개수 정하기
    {
        List<int> active = new List<int>();
        for (int i = 0; i < Cloud.Length; i++)
            active.Add(i);

        for (int i = 0; i < a_Count; i++)
        {
            int j = Random.Range(0, active.Count);
            Cloud[active[j]].SetActive(false);

            active.RemoveAt(j);
        }

        active.Clear();

        // -- 물고기 스폰시키기 --
        int range = 20 - GameDirector.level; // 20분의 1 확률로 구름 위에 아이템 생성
        if (range < 10) // 난이도에 따라서 확률은 점점 늘어나도록...
            range = 10; // 10분의 1 확률까지

        SpriteRenderer[] a_CloudObj = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < a_CloudObj.Length; i++)
        {
            if (Random.Range(0, range) == 0)
                SpawnFish(a_CloudObj[i].transform.position);
        }//for (int i = 0; i < a_CloudObj.Length; i++)
        // -- 물고기 스폰시키기 --
    }

    void SpawnFish(Vector3 a_Pos)
    {
        GameObject go = Instantiate(fishPrefab);
        go.SetActive(true);
        go.transform.position = a_Pos + Vector3.up * 0.8f;
    }
}
