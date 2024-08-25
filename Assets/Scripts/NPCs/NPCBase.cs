using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    public AudioClip goodSound;
    public AudioSource audioSource;
    public GameObject player;
    public GameObject desire;
    public GameObject reward;
    public GameObject e;
    public UnityEngine.Vector2 interactionPlace;
    public string desireCheck;
    public string rewardName;
    public List<string> playerItemList;
    public bool isTrading;
    public bool hasDesire;


    void Start()
    {
        player = GameObject.Find("Kaos");
        playerItemList = player.GetComponent<KaosBehaviour>().itemList;

    }

    void Update()
    {

        if (Vector2.Distance(player.transform.position, transform.position) < 0.5f && hasDesire)
        {
            if (desire)
            {
                desire.SetActive(true);
            }
            if (isTrading)
            {

                if (playerItemList.Contains(desireCheck))
                {
                    e.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        playerItemList.Add(rewardName);
                        interactionPlace = new UnityEngine.Vector2(player.transform.position.x, player.transform.position.y + .5f);
                        GameObject rewardPopup = Instantiate(reward, interactionPlace, Quaternion.identity);
                        StartCoroutine(DestroyReward(rewardPopup));
                        hasDesire = false;
                        
                    }

                }
            }
            else
            {
                e.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerItemList.Add(rewardName);
                    interactionPlace = new UnityEngine.Vector2(player.transform.position.x, player.transform.position.y + .5f);
                    GameObject rewardPopup = Instantiate(reward, interactionPlace, Quaternion.identity);
                    hasDesire = false;
                    StartCoroutine(DestroyReward(rewardPopup));
                }

            }

        }
        else
        {
            if (desire)
            {
                desire.SetActive(false);
            }
            e.SetActive(false);

        }
    }


    IEnumerator DestroyReward(GameObject rewardPopup)
    {
        print("Tenta destruir popup");
        yield return new WaitForSeconds(2);
        Destroy(rewardPopup);
    }
}
