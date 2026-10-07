using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        foreach (Transform child in transform)
        {
            EnemyMovement movement = child.GetComponent<EnemyMovement>();
            if (movement != null) movement.GameRestart();

            GoombaStomp stomp = child.GetComponent<GoombaStomp>();
            if (stomp != null) stomp.ResetGoomba();
        }
    }
}
