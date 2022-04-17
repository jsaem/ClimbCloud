using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    float waterSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        waterSpeed += 0.02f * Time.deltaTime;
        if (GameDirector.s_State == GameState.InGame)
        {
            transform.Translate(0, Time.deltaTime * waterSpeed, 0);
        }
    }
}
