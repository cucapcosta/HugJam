using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHallCamera : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        if (player.transform.position.y > -5.089999)
        {
            gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        }
        else
        {
            gameObject.transform.position = new Vector3(player.transform.position.x, -5.089999f, -10);
        }

    }
}

