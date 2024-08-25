using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHallCamera : MonoBehaviour
{
    public GameObject player;
    public float playerLimitY;
    public float limitY;
    void Start(){  
        player = GameObject.Find("Kaos");
    }
    void Update()
    {
        if (player.transform.position.y > playerLimitY)
        {
            gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        }
        else
        {
            gameObject.transform.position = new Vector3(player.transform.position.x, limitY, -10);
        }

    }
}

