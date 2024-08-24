using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Layering : MonoBehaviour
{
    public GameObject player;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (player.transform.position.y > gameObject.transform.position.y)
        {
            spriteRenderer.sortingOrder = 3;
        }
        else if (player.transform.position.y < gameObject.transform.position.y)
        {
            spriteRenderer.sortingOrder = 1;
        }
        
    }
}
