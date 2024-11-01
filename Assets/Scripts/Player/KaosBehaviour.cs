using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class KaosBehaviour : MonoBehaviour
{
    public AudioClip shootSound;
    public AudioClip chairSound;
    public AudioClip lockerSound;
    
    public AudioSource audioSource;
    public Rigidbody2D rb;
    public float speed;
    public float speedKickMultiplier;
    public float launchSpeed;
    public float arcHeight;
    public float timer = 60;
    public float timespent = 0;
    public UnityEngine.Vector2 direction;
    public Animator animator;
    public bool isAttacking;
    public bool isKicking;
    public bool canShoot = true;
    public bool isKickingLocker;
    public bool hasInstantiatedNerdola;
    public bool hasInstantiatedGeek;
    public GameObject marble;
    public GameObject nerdola;
    public GameObject geek;
    public GameObject ponteiroImagem;
    private Dictionary<GameObject, bool> kickedChairs = new Dictionary<GameObject, bool>();
    public List<string> itemList;
    public string currentRoom;
    public int score;
    public int lockersKicked;
    public int nerdolaInstantiateTime;
    public int geekInstantiateTime;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nerdolaInstantiateTime = Random.Range(1, 6);
        geekInstantiateTime = Random.Range(7, 11);
    }
    void Update()
    {
        timespent += Time.deltaTime;
        if (timespent >= 200)
        {
            PlayerPrefs.SetFloat("score", gameObject.GetComponent<KaosBehaviour>().score);
            PlayerPrefs.SetInt("time", (int)gameObject.GetComponent<KaosBehaviour>().timespent);
            PlayerPrefs.Save();
            SceneManager.LoadScene("FIM");
        }
        direction = new UnityEngine.Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (isAttacking)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("kick") && stateInfo.normalizedTime >= 1 || !stateInfo.IsName("kick") && !stateInfo.IsName("shoot"))
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
        if (Input.GetButtonDown("Fire1") && itemList.Contains("estilingue"))
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if(!stateInfo.IsName("kick") && !stateInfo.IsName("shoot"))
            Atirar();
        }
        ponteiroImagem.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, -360 * (timespent / timer));
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
            audioSource.PlayOneShot(shootSound);
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
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isKicking)
        {
            switch (collision.gameObject.tag)
            {
                case "Chair":
                    GameObject chair = collision.gameObject;
                    Animator collisionAnimator = chair.GetComponent<Animator>();
                    StartCoroutine(WairForKick(chair, collisionAnimator, "chairdead"));

                    break;
                case "Carteira":
                    GameObject carteira = collision.gameObject;
                    Animator carteiraAnimator = carteira.GetComponent<Animator>();
                    StartCoroutine(WairForKick(carteira, carteiraAnimator, "carteiradead"));

                    break;
                case "MesaProf":
                    GameObject mesaProf = collision.gameObject;
                    Animator mesaProfAnimator = mesaProf.GetComponent<Animator>();
                    StartCoroutine(WairForKick(mesaProf, mesaProfAnimator, "tabledead"));

                    break;
                case "Armario":
                    if (!isKickingLocker)
                    {
                        lockersKicked++;
                        isKickingLocker = true;
                    }
                    GameObject armario = collision.gameObject;
                    Animator armarioAnimator = armario.GetComponent<Animator>();
                    StartCoroutine(WairForKick(armario, armarioAnimator, "lockeropen"));
                    if (lockersKicked == nerdolaInstantiateTime && !hasInstantiatedNerdola)
                    {
                        GameObject nerdolaInstanciado = Instantiate(nerdola, new UnityEngine.Vector2(transform.position.x, transform.position.y - 0.1f), UnityEngine.Quaternion.identity);
                        hasInstantiatedNerdola = true;
                        Animator animNerdola = nerdolaInstanciado.GetComponent<Animator>();
                        animNerdola.CrossFade("nerdfall", 0);
                        StartCoroutine(NerdolaLevanta(animNerdola));
                    }
                    else if (lockersKicked == geekInstantiateTime && !hasInstantiatedGeek)
                    {
                        GameObject geekInstanciado = Instantiate(geek, new UnityEngine.Vector2(transform.position.x, transform.position.y - 0.1f), UnityEngine.Quaternion.identity);
                        hasInstantiatedGeek = true;
                        Animator animGeek = geekInstanciado.GetComponent<Animator>();
                        animGeek.CrossFade("geekfall", 0);
                        StartCoroutine(GeekLevanta(animGeek));
                    }

                    break;

            }
        }
    }
    IEnumerator WairForKick(GameObject chair, Animator collisionAnimator, string stateName)
    {
        yield return new WaitForSeconds(0.1f);
        if (!kickedChairs.ContainsKey(chair) || !kickedChairs[chair])
        {

            kickedChairs[chair] = true; // Mark this chair as kicked
            collisionAnimator.CrossFade(stateName, 0);
            if (stateName != "lockeropen")
            {
                score++;
                audioSource.PlayOneShot(chairSound);
                chair.GetComponent<Collider2D>().enabled = false;
            }
            else
            {
                isKickingLocker = false;
                AnimatorStateInfo checkAnimArmario = collisionAnimator.GetCurrentAnimatorStateInfo(0);
                if (!checkAnimArmario.IsName("lockeropen"))
                {
                    audioSource.PlayOneShot(lockerSound);
                    score++;
                }
            }

        }

    }
    IEnumerator NerdolaLevanta(Animator nerdolaAnim)
    {
        yield return new WaitForSeconds(0.65f);
        nerdolaAnim.CrossFade("nerdidle", 0);
    }
    IEnumerator GeekLevanta(Animator nerdolaAnim)
    {
        yield return new WaitForSeconds(0.65f);
        nerdolaAnim.CrossFade("geekidle", 0);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Prof")
        {
            PlayerPrefs.SetFloat("score", gameObject.GetComponent<KaosBehaviour>().score);
            PlayerPrefs.SetInt("time", (int)gameObject.GetComponent<KaosBehaviour>().timespent);
            PlayerPrefs.Save();
            SceneManager.LoadScene("FIM");

        }
    }

}
