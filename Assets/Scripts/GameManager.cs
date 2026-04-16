using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    private SoundManager soundManager;
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject titleScreen;
    public ParticleSystem tapParticle;
    public ParticleSystem wowParticle;
    
    private int score;
    private int milestoneInterval = 200;
    private float spawnRate = 1.5f;
    private bool gameOver = false;
    
    void Start()
    {
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }

    void Update()
    {
        // Only show tap feedback during active gameplay
        if (Input.GetMouseButtonDown(0) && !gameOver && !titleScreen.activeSelf)
        {
            ShowTapFeedback(Input.mousePosition);
        }
    }
    
    void ShowTapFeedback(Vector3 screenPosition)
    {
        // Convert screen position to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, 10f)
        );

        // Play particle at tap location
        Instantiate(tapParticle, worldPosition, tapParticle.transform.rotation);
    }

    IEnumerator SpawnTarget()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        int previousScore = score;
        score += scoreToAdd;
        scoreText.text = "SCORE: " + score;
    
        // Check if we've crossed a milestone
        int previousMilestone = previousScore / milestoneInterval;
        int currentMilestone = score / milestoneInterval;
    
        if (currentMilestone > previousMilestone && score >= milestoneInterval)
        {
            PlayWowEffect();
        }
    }
    
    public void PlayWowEffect()
    {
        soundManager.PlayWowSound();
        Vector3 pos = new Vector3(0, 5, 0);
    
        if (score >= milestoneInterval && !gameOver)
        {
            int currentMilestone = score / milestoneInterval;
            Debug.Log("Player reached milestone " + currentMilestone + " (Score: " + score + ") Congratulations!");
            Instantiate(wowParticle, pos, Quaternion.identity);
        }
    }
    
    public void GameOver()
    {
        gameOver = true;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficultyLevel)
    {
        score = 0;
        UpdateScore(0);
        spawnRate /= difficultyLevel;
        StartCoroutine(SpawnTarget());
        titleScreen.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
    }
}