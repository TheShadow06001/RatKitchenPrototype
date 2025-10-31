using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public static HealthDisplay instance;
    public int health;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform cameraRespawn;
    [SerializeField] private GameObject cameraMain;
    public GameObject gameOverMenu;
    public GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;    
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player");

        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(false);
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void TakeDamage()
    {
        health--;

        if (health > 0)
        {
            /*Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name); */
            
            player.transform.position = respawnPoint.transform.position;
            cameraMain.transform.position = cameraRespawn.transform.position;
        }
        else
        {
            HealthDisplay.instance.health = 0;
            Debug.Log("Game Over");
            if (gameOverMenu != null)
            {
                gameOverMenu.SetActive(true);
            }
        }
    }
}
