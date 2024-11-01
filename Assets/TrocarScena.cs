using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocarScena : MonoBehaviour
{
    float time;
    void Update(){
        time += Time.deltaTime;
        if(time >= 5){
            SceneManager.LoadScene("KaosAgenteDoCaos");
        }
    }
}
