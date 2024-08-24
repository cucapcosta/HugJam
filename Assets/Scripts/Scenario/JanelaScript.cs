using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JanelaScript : MonoBehaviour
{
    public Animator animator;
    public GameObject janelaQuebrando;
    void Start()
    {
        animator = GetComponent<Animator>();
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
            GameObject shatterInstance = Instantiate(janelaQuebrando, transform.position, Quaternion.identity);
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(collision.gameObject);
        }
    }
}
