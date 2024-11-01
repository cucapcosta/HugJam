using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeacherController : MonoBehaviour
{
    public Animator animator;
    public float speed;
    public GameObject player;
    public string currentRoom;
    public Rigidbody2D rb;
    public bool isChase;
    public bool isTP;
    public List<TPINFO> tpPlaceList = new List<TPINFO>();
    [SerializeField]public Dictionary<string, UnityEngine.Vector2> tpPlaces;

    void Start()
    {
        player = GameObject.Find("Kaos");
        tpPlaces = tpPlaceList.ToDictionary(x => x.room, x => x.position);
        animator = GetComponent<Animator>();
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
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            } else {
                if (stateInfo.IsName("teacheridle"))
                {
                    animator.CrossFade("teachershock", 0); // animação antes de chase
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
    
}
[Serializable]
public class TPINFO {
    public string room;
    public UnityEngine.Vector2 position;
}