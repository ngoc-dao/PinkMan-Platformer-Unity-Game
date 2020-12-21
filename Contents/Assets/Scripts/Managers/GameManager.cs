using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance = null;
    public GameObject playerSpawnPoint;
    public CameraManager cameraManager;
    public HitPoints hitPoints;
    Coroutine enemyAttackDelayCoroutine = null;
    [HideInInspector] public PlayerController playerScript;
    public GameObject playerPrefab;
    public GameObject appearPrefab;
    GameObject player;

    public float enemyAttackDelay; // 0.5
    public float respawnDelay; // 2


    private void Awake()
    {
        // This is here to prevent more than one instance of type RPGGameManager to be made,
        // other than sharedInstance of this class
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }

    void Start()
    {
        SetupScene();
        playerScript = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (hitPoints.playerIsHit && enemyAttackDelayCoroutine == null && playerScript != null)
        {
            hitPoints.playerIsHit = false;

            // Makes player turn into different color for brief time when attacked
            StartCoroutine(playerScript.FlickerCharacter(0.2f));

            // delays keyboard inputs when player is attacked
            playerScript.delayCoroutine = StartCoroutine(playerScript.Delay(0.2f));

            // if cannot decrement health, meaning no more lives, respawn player
            if (playerScript.healthBar.Decrement(1) == false || hitPoints.instantDeath)
            {
                // turns all hearts to grey
                if (hitPoints.instantDeath)
                {
                    playerScript.healthBar.InstantDeath();
                }

                hitPoints.value = 0;
                StartCoroutine(Respawn());
            }

            // gives some time before enemy can attack again.
            enemyAttackDelayCoroutine = StartCoroutine(Delay(enemyAttackDelay));
        }
        else
        {
            hitPoints.playerIsHit = false;
        }
    }

    void SetupScene()
    {
        AppearAnimationThenSpawn();
    }

    public void SpawnPlayer()
    {
        if (playerSpawnPoint != null)
        {
            player = Instantiate(playerPrefab, playerSpawnPoint.transform.position, Quaternion.identity);
            hitPoints.value = playerScript.startingHitPoints;
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }

    IEnumerator Respawn()
    {
        playerScript.KillCharacter();
        yield return new WaitForSeconds(respawnDelay);
        hitPoints.value = playerScript.startingHitPoints;
        AppearAnimationThenSpawn();
    }

    public IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        enemyAttackDelayCoroutine = null;
    }

    public void AppearAnimationThenSpawn()
    {
        Instantiate(appearPrefab, playerSpawnPoint.transform.position, Quaternion.identity);
        if (player == null)
        {
            cameraManager.virtualCamera.Follow = playerSpawnPoint.transform;
        }
        Invoke("SpawnPlayer", appearPrefab.GetComponent<Appear>().appear.length);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
