using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartMenuScript : MonoBehaviour
{   
    float playerScore;
    public Score score;
    public Text yourScoreText;
    public Text highScoreText;
    void Start()
    {
        playerScore = score.returnScore();
        if(playerScore > PlayerPrefs.GetFloat("HighScore",0))
        {
            PlayerPrefs.SetFloat("HighScore",playerScore);
            highScoreText.text = playerScore.ToString();
        }
        else{
            highScoreText.text = PlayerPrefs.GetFloat("HighScore",0).ToString();
        }
        yourScoreText.text = playerScore.ToString();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
}
