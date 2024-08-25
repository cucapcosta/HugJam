using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JanelaScript : MonoBehaviour
{
    public Animator animator;
    public GameObject janelaQuebrando;
    Animator shatterAnimator;
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Kaos");
        animator = GetComponent<Animator>();
    }
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tiro"))
        {
            int randValue = Random.Range(0, 2);
            print("Random value: " + randValue);
            if (randValue == 0)
            {
                animator.CrossFade("shattered1", 0);
            }
            else
            {
                animator.CrossFade("shattered2", 0);
            }
            player.GetComponent<KaosBehaviour>().score += 1;
            GameObject shatterInstance = Instantiate(janelaQuebrando, transform.position, Quaternion.identity);
            shatterAnimator = shatterInstance.GetComponent<Animator>();
            shatterAnimator.CrossFade("shatter_Clip", 0);
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(collision.gameObject);
        }
    }
}
