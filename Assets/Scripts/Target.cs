using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private GameManager gameManager;
    private SoundManager soundManager;
    private float minSpeed = 12.0f;
    private float maxSpeed = 16.0f;
    private float maxTorque = 1.50f;
    private float xRange = 4.0f;
    private float ySpawnPos = -2.0f;
    
    public ParticleSystem explosionParticle;
    public ParticleSystem hitParticle;
    public int pointValue = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        
        transform.position = RandomSpawnPos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (!gameObject.CompareTag("Bad"))
        {
            gameManager.GameOver();
        }
    }
    
    private void OnMouseDown()
    {
        if (!gameManager.IsGameOver())
        {
            #if UNITY_ANDROID || UNITY_IOS
                Handheld.Vibrate();
            #endif
            
            if(gameObject.CompareTag("Bad"))
            {
                Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
                soundManager.PlayExplosionSound();
            } else if (gameObject.CompareTag("Good"))
            {
                soundManager.PlayHitSound();
            } else if (gameObject.CompareTag("Special"))
            {
                Instantiate(hitParticle, transform.position, hitParticle.transform.rotation);
                soundManager.PlaySpecialHitSound();
            }
            Destroy(gameObject);
            gameManager.UpdateScore(pointValue);
        }
    }
    
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }
    
    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange,xRange), ySpawnPos);
    }
    
    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

}

