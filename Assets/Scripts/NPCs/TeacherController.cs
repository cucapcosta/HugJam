using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeacherController : MonoBehaviour
{
    public AudioClip mugSound;
    public AudioClip hitSound;
    public AudioClip hit2Sound;
    public AudioClip hit3Sound;
    public AudioSource audioSource;
    public Animator animator;
    public float speed;
    public float maxScore = 90f;
    public float minSpeed = 3f;
    public float maxSpeed = 9f;
    public GameObject player;
    public string currentRoom;
    public Rigidbody2D rb;
    public bool isChase;
    public bool isTP;
    public List<TPINFO> tpPlaceList = new List<TPINFO>();
    [SerializeField] public Dictionary<string, UnityEngine.Vector2> tpPlaces;

    private float acceleration = 1.0f;
    private float targetAcceleration;
    private float maxAcceleration = 1.5f;
    private Vector2 targetDirection;

    void Start()
    {
        player = GameObject.Find("Kaos");
        tpPlaces = tpPlaceList.ToDictionary(x => x.room, x => x.position);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.drag = 2f;  // Linear drag for steady movement
        rb.angularDrag = 5f; // Higher angular drag for smoother turns
    }

    void Update(){
        if(player.transform.position.x > transform.position.x){
         GetComponent<SpriteRenderer>().flipX = false;   
        }else{
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (player.GetComponent<KaosBehaviour>().score == 0) {return;}
        float teleportTime = 0.2f * (39/player.GetComponent<KaosBehaviour>().score);
        if(player.GetComponent<KaosBehaviour>().score >=15){
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("teacheridle"))
            {
                isChase = true;
                animator.CrossFade("teacherwalk", 0);
            }
        }
        if(teleportTime < 3f){
            teleportTime = 3f;
        }
        print(teleportTime);
        if(player.GetComponent<KaosBehaviour>().currentRoom == currentRoom){
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (isChase)
            {
                // Adjust speed based on score, with faster pursuit at higher scores
                speed = Mathf.Clamp(minSpeed + (maxSpeed - minSpeed) * (player.GetComponent<KaosBehaviour>().score / maxScore), minSpeed, maxSpeed);

                // Calculate target direction and smoothly adjust acceleration for a relentless chase
                targetDirection = (player.transform.position - transform.position).normalized;
                targetAcceleration = Mathf.Lerp(acceleration, maxAcceleration, Time.deltaTime * 0.5f); // Smooth transition for acceleration

                rb.AddForce(targetDirection * speed * targetAcceleration * Time.deltaTime, ForceMode2D.Force);

                // Update acceleration for damping effect
                acceleration = Mathf.Lerp(acceleration, targetAcceleration, Time.deltaTime * 0.5f);

                // If close to player, maintain high speed for consistent chase
                if (Vector2.Distance(transform.position, player.transform.position) < 0.5f)
                {
                    acceleration = 1.2f; // Slight increase to prevent slowing too much
                }
            } else {
                if (stateInfo.IsName("teacheridle"))
                {
                    animator.CrossFade("teachershock", 0); // animação antes de chase
                    audioSource.PlayOneShot(mugSound);
                }
                if (stateInfo.IsName("teachershock") && stateInfo.normalizedTime >= 1) // apenas comeca o chase apos tocar animação
                {
                    isChase = true;  
                    animator.CrossFade("teacherwalk", 0);
                }
            }
        } else{
            if(!isTP && isChase){
                StartCoroutine(Teleport(teleportTime));
            }
        }
    }
    IEnumerator Teleport(float time){
        print("Teleporting");
        isTP = true;
        yield return new WaitForSeconds(time);

        if(player.GetComponent<KaosBehaviour>().currentRoom != "mainHall"){
            transform.position = tpPlaces[player.GetComponent<KaosBehaviour>().currentRoom];
            currentRoom = player.GetComponent<KaosBehaviour>().currentRoom;
        }else{
            switch (currentRoom){
                case "room1":
                    transform.position = tpPlaces["mainHall1"];
                    currentRoom = "mainHall";
                    break;
                case "room2":
                    transform.position = tpPlaces["mainHall2"];
                    currentRoom = "mainHall";
                    break;
                case "room3":
                    transform.position = tpPlaces["mainHall3"];
                    currentRoom = "mainHall";
                    break;
                case "roomprof":
                    transform.position = tpPlaces["mainHall4"];
                    currentRoom = "mainHall";
                    break;
            }
        }
        
        isTP = false;
    }
    public void stunTeacher()
    {StartCoroutine(StunTeacherCoroutine());}
    private IEnumerator StunTeacherCoroutine()
    {
        animator.CrossFade("teacherhit", 0);
        int randomIndex = UnityEngine.Random.Range(0, 3);
        switch (randomIndex)
        {
            case 0:
                audioSource.PlayOneShot(hitSound);
                break;
            case 1:
                audioSource.PlayOneShot(hit2Sound);
                break;
            case 2:
                audioSource.PlayOneShot(hit3Sound);
                break;
        }
        yield return new WaitForSeconds(0.5f);
        isChase = true;
        animator.CrossFade("teacherwalk", 0);
    }
}
[Serializable]
public class TPINFO
{
    public string room;
    public UnityEngine.Vector2 position;
}