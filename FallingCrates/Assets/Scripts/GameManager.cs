using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject cratePrefab;
    [SerializeField]
    private int maxCrateToSpawn = 3;

    [SerializeField]
    private TMPro.TextMeshProUGUI scoreText;
    [SerializeField]
    private Image backgroundMenu;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject mainVCam;
    [SerializeField]
    private GameObject zoomVCam;

    [SerializeField]
    private GameObject gameOverMenu;

    private int highScore;
    private int score;
    private float timer;
    private Coroutine cratesCoroutine;

    private bool gameOver;

    private static GameManager instance;
    private const string HighScorePreferenceKey = "HighScore";
    public static GameManager Instance => instance;
    public int HighScore => highScore;

    void Start()
    {
        instance = this;

        highScore = PlayerPrefs.GetInt(HighScorePreferenceKey);
    }

    private void OnEnable()
    {      
        player.SetActive(true);

        zoomVCam.SetActive(false);
        mainVCam.SetActive(true);

        gameOver = false;
        scoreText.text = "0";
        score = 0;
        timer = 0;

        cratesCoroutine = StartCoroutine(SpawnCrates());
    }


    private void Update()
    {
        // Pause and resume the game with Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Resume();
            }
            if (Time.timeScale == 1)
            {
                Pause();
            }
        }

        if (gameOver)
            return;

        timer += Time.deltaTime;

        // Add points to the score
        if (timer >= 1f)
        {
            score++;
            scoreText.text = score.ToString();

            timer = 0;
        }
    }

    // Method used by LeanTween to change the time scale
    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = 0.02F * value;
    }
    
    // Coroutine to randonly spawn crates
    private IEnumerator SpawnCrates()
    {
        var crateToSpawn = Random.Range(1, maxCrateToSpawn);

        for (int i = 0; i < crateToSpawn; i++)
        {
            var x = Random.Range(-7, 7);
            var drag = Random.Range(0f, 2f);

            var crate = Instantiate(cratePrefab, new Vector3(x, 11, 0), Quaternion.identity);
            crate.GetComponent<Rigidbody>().drag = drag;
        }

        var timeToWait = Random.Range(0.5f, 1.0f);
        yield return new WaitForSeconds(timeToWait);

        yield return SpawnCrates();
    }

    public void GameOver()
    {
        StopCoroutine(cratesCoroutine);
        gameOver = true;

        // Prevent player to get stuck when he pauses and dies after the transition to pause menu
        if (Time.timeScale < 1)
        {
            Resume();
        }

        // Check the score and save it with PlayerPrefs
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScorePreferenceKey, highScore);
        }

        mainVCam.SetActive(false);
        zoomVCam.SetActive(true);

        gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    // Enable the GameManager object when pressed the Play button
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    private void Pause()
    {
        // Pausing the game with LeanTween             
        LeanTween.value(1, 0, 0.75f)
            .setOnUpdate(SetTimeScale)
            .setIgnoreTimeScale(true);


        // Pausing the game with Coroutine
        //StartCoroutine(ScaleTime(1, 0, 0.5f));

        backgroundMenu.gameObject.SetActive(true);
    }

    private void Resume()
    {
        // Pausing the game with LeanTween
        LeanTween.value(0, 1, 0.75f)
            .setOnUpdate(SetTimeScale)
            .setIgnoreTimeScale(true);

        // Pausing the game with Coroutine
        //StartCoroutine(ScaleTime(0, 1, 0.5f));

        backgroundMenu.gameObject.SetActive(false);
    }

    // Coroutine to smoothly change the time scale over a specified duration
    /*
    IEnumerator ScaleTime(float start, float end, float duration)
    {
        float lastTime = Time.realtimeSinceStartup;
        float timer = 0.0f;

        while (timer < duration)
        {
            Time.timeScale = Mathf.Lerp(start, end, timer / duration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            timer += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        Time.timeScale = end;
        Time.fixedDeltaTime = 0.02F * end;
    }
    */
}
