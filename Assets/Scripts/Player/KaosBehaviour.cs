using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Animations;

public class KaosBehaviour : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float speedKickMultiplier;
    public float launchSpeed;
    public float arcHeight;
    public UnityEngine.Vector2 direction;
    public Animator animator;
    public bool isAttacking;
    public bool isKicking;
    public GameObject marble;
    private Dictionary<GameObject, bool> kickedChairs = new Dictionary<GameObject, bool>();


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        direction = new UnityEngine.Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (isAttacking)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("kick") && stateInfo.normalizedTime >= 1)
            {
                isAttacking = false;
                isKicking = false;
            }
            else if (stateInfo.IsName("shoot") && stateInfo.normalizedTime >= 1)
            {
                isAttacking = false;
            }
            rb.velocity = direction * speed / speedKickMultiplier;
        }
        else
        {
            Animacoes(direction);
            rb.velocity = direction * speed;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Chutar();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Atirar();
        }
    }
    void Animacoes(UnityEngine.Vector2 direction)
    {
        if (direction == UnityEngine.Vector2.zero)
        {
            animator.CrossFade("idle", 0);
        }
        else if (direction.x > 0)
        {
            animator.CrossFade("walk", 0);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (direction.x < 0)
        {
            animator.CrossFade("walk", 0);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (direction.y > 0)
        {
            animator.CrossFade("walkback", 0);
        }
        else
        {
            animator.CrossFade("walkfront", 0);
        }
    }

    void Atirar()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            UnityEngine.Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (worldMousePosition.x > transform.position.x)
            {
                animator.CrossFade("shoot", 0);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                animator.CrossFade("shoot", 0);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            StartCoroutine(WaitToShoot(0.2f, transform.position, worldMousePosition, arcHeight));
        }
    }
    void Chutar()
    {
        if (!isAttacking)
        {
            animator.CrossFade("kick", 0);
            isAttacking = true;
            isKicking = true;
        }
    }

    UnityEngine.Vector2 CalculateLaunchVelocity(UnityEngine.Vector2 startPosition, UnityEngine.Vector2 targetPosition, float arcHeight)
    {

        float gravity = Physics2D.gravity.y * -1;
        float displacementX = targetPosition.x - startPosition.x;
        float displacementY = Mathf.Abs(targetPosition.y - startPosition.y);
        print(-2 * arcHeight / gravity);
        float time = Mathf.Sqrt(-2 * arcHeight / gravity) + Mathf.Sqrt(2 * (displacementY - arcHeight) / gravity);
        UnityEngine.Vector2 velocity = new UnityEngine.Vector2(displacementX / time, (displacementY / time) + (0.5f * gravity * time));
        return velocity * launchSpeed;

    }
    IEnumerator WaitToShoot(float time, UnityEngine.Vector2 startPosition, UnityEngine.Vector2 targetPosition, float arcHeight)
    {
        yield return new WaitForSeconds(time);
        Rigidbody2D marbleInstance = Instantiate(marble, startPosition, UnityEngine.Quaternion.identity).GetComponent<Rigidbody2D>();
        UnityEngine.Vector2 launchVelocity = CalculateLaunchVelocity(startPosition, targetPosition, arcHeight);
        marbleInstance.velocity = launchVelocity;
        //DestroyMarble(marbleInstance.gameObject);
    }
    //IEnumerator DestroyMarble(GameObject marble, float time){
      //  yield return new WaitForSeconds(time);
        //Destroy(marble);
    //}
    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Carteira" && isKicking)
        {
            GameObject chair = collision.gameObject;
            Animator collisionAnimator = chair.GetComponent<Animator>();
            StartCoroutine(WairForChairKick(chair, collisionAnimator));
            // Check if this specific chair has been kicked before

        }
    }
    IEnumerator WairForChairKick(GameObject chair, Animator collisionAnimator)
    {
        yield return new WaitForSeconds(0.1f);
        if (!kickedChairs.ContainsKey(chair) || !kickedChairs[chair])
        {
            print("Acertou cadeira");
            collisionAnimator.CrossFade("carteirafall", 0);
            kickedChairs[chair] = true; // Mark this chair as kicked
        }
        if (collisionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && kickedChairs[chair])
        {
            print("Deixar cadeira derrubada");
            collisionAnimator.CrossFade("carteiradead", 0);
            chair.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
