using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EndController : MonoBehaviour
{
    float time;
    float scorevalue;
    int timeUsed;
    public GameObject[] scoreLetter;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    void Start()
    {
        scorevalue = PlayerPrefs.GetFloat("score");
        timeUsed = PlayerPrefs.GetInt("time");
        scoreText.text = scorevalue.ToString();
        timeText.text = timeUsed.ToString();
        if(scorevalue > 100){
            scoreLetter[0].SetActive(true);
        }else if(110>scorevalue && scorevalue>=80){
            scoreLetter[1].SetActive(true);
        }else if(80>scorevalue && scorevalue>=50){
            scoreLetter[2].SetActive(true);
        }else if(50>scorevalue && scorevalue>=20){
            scoreLetter[3].SetActive(true);
        }else{
            scoreLetter[4].SetActive(true);
        }
    }
    void Update(){
        time += Time.deltaTime;
        if(time > 5){
            SceneManager.LoadScene("Inicio");
        }
    }
}
