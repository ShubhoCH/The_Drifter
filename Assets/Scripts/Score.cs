using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    float Points = 0f;
    public Text scoreText;
    void Update()
    {
        scoreText.text = Points.ToString();
    }
    public void ScoreUpdater()
    {
        Points += 10f;
    }
    public float returnScore()
    {
        return Points;
    }
}
