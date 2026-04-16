using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject titleScreen;
    public ParticleSystem tapParticle;  // ADD THIS
    
    private int score;
    private float spawnRate = 1.5f;
    private bool gameOver = false;
    
    void Start()
    {

    }

    void Update()
    {
        // Only show tap feedback during active gameplay
        if (Input.GetMouseButtonDown(0) && !gameOver && !titleScreen.activeSelf)
        {
            ShowTapFeedback(Input.mousePosition);
        }
    }

    // ADD THIS METHOD
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
        score += scoreToAdd;
        scoreText.text = "SCORE: " + score;
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