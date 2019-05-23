using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f,1f)] private float gameSpeed = 1f;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int scoreAddition = 50;
    [SerializeField] private int currentScore = 0;

    void Awake()
    {
        int gameStatusCount = FindObjectsOfType<GameSession>().Length;
        if (gameStatusCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
	// Update is called once per frame
	void Update ()
	{
	    Time.timeScale = gameSpeed;
	}

    public void AdditionToTheScore()
    {
        currentScore += scoreAddition;
        scoreText.text = currentScore.ToString();
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
