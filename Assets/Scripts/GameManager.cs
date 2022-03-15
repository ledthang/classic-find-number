using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class ConstStorage
{
    public const float deltaTime = 0.25f;

}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] TextMeshProUGUI currentNumberText;

    public bool isAllNumberFinded = false;

    private int _currentNumner;
    public int currentNumber
    {
        get
        {
            return _currentNumner;
        }
        set
        {
            if (value > DataManager.Instance.totalNumber)
            {
                isAllNumberFinded = true;
                _currentNumner = DataManager.Instance.totalNumber;
            }
            else _currentNumner = value;
        }
    }
    [SerializeField] TextMeshProUGUI timeText;
    private float time;

    private int score;
    private int _highScore;
    private int highScore
    {
        set
        {
            _highScore = value;
            SaveHighScore();
        }
        get
        {
            LoadHighScore();
            return _highScore;
        }
    }
    private string highScoreKey;

    [SerializeField] GameObject canvas;
    [SerializeField] GameObject findCircleImg;

    private float startTime; //time blitz
    private float timeToAdd; //time blitz

    private bool isGameOverSoundPlayed;
    private bool isGameOverLayerShown;
    [Header("Gameover-lose")]
    [SerializeField] GameObject gameOverLoseLayer;
    [Header("Gameover-highscore")]
    [SerializeField] GameObject gameOverHighscoreLayer;
    [SerializeField] TextMeshProUGUI gameOverHighscoreText;
    [Header("Gameover-nohighscore")]
    [SerializeField] GameObject gameOverNoHighscoreLayer;
    [SerializeField] TextMeshProUGUI gameOverNoHighscoreText;
    [SerializeField] TextMeshProUGUI gameOverYourScoreText;
    [Header("Gameover-Multiplayer Score")]
    [SerializeField] GameObject gameOverMultiLayer;
    [SerializeField] TextMeshProUGUI gameOverP1ScoreText;
    [SerializeField] TextMeshProUGUI gameOverP2ScoreText;

    [Header("Button")]
    [SerializeField] public Button findButton;
    [SerializeField] GameObject backButtonState2;
    [SerializeField] GameObject replayButtonState2;
    [SerializeField] GameObject nextLevelButton;

    int findLeft;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

        highScoreKey = DataManager.Instance.playMode + "" + DataManager.Instance.totalNumber + "highscore";
        Debug.Log(highScoreKey);
        if (!PlayerPrefs.HasKey(highScoreKey))
        {
            highScore = -1;
        }
        else
        {
            LoadHighScore();
        }

        isGameOverLayerShown = false;
        isGameOverSoundPlayed = false;

    }
    
    private void LoadHighScore()
    {
        _highScore = PlayerPrefs.GetInt(highScoreKey);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(highScoreKey, _highScore);
    }

    private void Start()
    {
        AudioManager.Instance.LoadButton();
        currentNumber = 1;
        findLeft = DataManager.Instance.totalNumber / 30;
        if (DataManager.Instance.playMode == PlayMode.SingleTimeBlitz)
        {
            time = 30 * DataManager.Instance.totalNumber / 99.0f;
            timeToAdd = 10 * DataManager.Instance.totalNumber / 99.0f;
        }
        else
        {
            time = 0;
        }
        Debug.Log("Mode: " + DataManager.Instance.playMode);
        Debug.Log("Total Number: " + DataManager.Instance.totalNumber);
        Debug.Log("Scale: " + DataManager.Instance.scaleRatio);
#if UNITY_ANDROID || UNITY_IPHONE
        AdsManager.Instance.ShowBannerAd();
#endif
    }

    private void Update()
    {
        currentNumberText.text = currentNumber.ToString();
        DisplayTime(time);

        if (IsGameOver())
        {
            if (!isGameOverLayerShown)
            {
                isGameOverLayerShown = true;
                HandleGameOver();
                Debug.Log("GAME " + highScoreKey + " OVER");
                Debug.Log("High score: " + highScore);
            }
        }
        else if (DataManager.Instance.playMode == PlayMode.SingleTimeBlitz)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time += Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        AdsManager.Instance.HideBannerAd();
#endif
    }

    public void AddTime()
    {
        if (DataManager.Instance.playMode == PlayMode.SingleTimeBlitz)
        {
            time += timeToAdd;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float min = Mathf.FloorToInt(timeToDisplay / 60);
        timeText.text = timeToDisplay > 0 ? min + ":" + (seconds < 10 ? "0" : "") + seconds : "0:00";
        if ((DataManager.Instance.playMode == PlayMode.SingleTimeBlitz) && timeToDisplay < 3)
        {
            timeText.color = Color.red;
        }
        else
        {
            timeText.color = Color.white;
        }
    }

    private bool IsGameOver()
    {
        if (DataManager.Instance.playMode == PlayMode.SingleTimeBlitz)
        {
            if (time < 0)
            {
                return true;
            }
        }
        return isAllNumberFinded;
    }

    private void HandleGameOver()
    {
        score = (int)time;
        Debug.Log("game score: " + score);
        DestroyAllNumber();

        if (highScore <= 0)
        {
            highScore = score;
        }

        if (DataManager.Instance.playMode == PlayMode.SingleTimeBlitz)
        {
            if (currentNumber != DataManager.Instance.totalNumber)
            {
                HandleLose();
            }
            else
            {
                if (score > highScore)
                {
                    HandleHighscore();
                }
                else
                {
                    HandleNoHighscore();
                }
            }
        }
        else
        {
            if (score < highScore)
            {
                HandleHighscore();
            }
            else
            {
                HandleNoHighscore();
            }
            if (DataManager.Instance.playMode == PlayMode.Multiplay)
            {
                HandleMultiplayerScore();
            }
        }
    }

    void HandleLose()
    {
        gameOverLoseLayer.SetActive(true);
        if (!isGameOverSoundPlayed)
        {
            isGameOverSoundPlayed = true;
            AudioManager.Instance.LoseSFX();
        }
    }

    void HandleNoHighscore()
    {
        gameOverNoHighscoreLayer.SetActive(true);
        gameOverNoHighscoreText.text = "Highscore: " + highScore;
        gameOverYourScoreText.text = "Your score: " + score;
        if (DataManager.Instance.totalNumber != 99)
            ShowNextLevelButton();

        if (!isGameOverSoundPlayed)
        {
            isGameOverSoundPlayed = true;
            AudioManager.Instance.NoHighscoreSFX();
        }
    }
    void HandleHighscore()
    {
        highScore = score;
        gameOverHighscoreLayer.SetActive(true);
        gameOverHighscoreText.text = "New highscore: " + highScore;
        if (DataManager.Instance.totalNumber != 99)
            ShowNextLevelButton();

        if (!isGameOverSoundPlayed)
        {
            isGameOverSoundPlayed = true;
            AudioManager.Instance.HighscoreSFX();
        }
    }

    void HandleMultiplayerScore()
    {
        gameOverMultiLayer.SetActive(true);
        gameOverP1ScoreText.text = PlayerController.Instance.P1Score.ToString();
        gameOverP2ScoreText.text = PlayerController.Instance.P2Score.ToString();
    }

    void DestroyAllNumber()
    {
        StartCoroutine(DelayDestroy());
        StopAllCoroutines();
        GameObject[] numbers = GameObject.FindGameObjectsWithTag("number");
        for (int i = 0; i < numbers.Length; i++)
        {
            Destroy(numbers[i].gameObject);
        }
        foreach (Transform c in canvas.GetComponentsInChildren<Transform>())
        {
            if (c.transform.CompareTag("circle"))
            {
                Destroy(c.gameObject);
            }
        }
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(2);
    }

    public void ReplayButton()
    {
        StartCoroutine(TapAgainToReplay());
    }
    IEnumerator TapAgainToReplay()
    {
        replayButtonState2.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        replayButtonState2.SetActive(false);
    }

    public void ReplayButtonState2()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        AdsManager.Instance.ShowInterstitialAd();
#endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void FindButtonShowAd()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        AdsManager.Instance.ShowFindRewardAd();
#else
        FindButton();
        findLeft--;
        Debug.Log(findLeft);
        if (findLeft < 1)
        {
            findButton.interactable = false;
        }
#endif
    }

    public void FindButton()
    {
        GameObject currentNumObj = GameObject.Find(currentNumber.ToString());
        if (currentNumObj != null)
        {
            AudioManager.Instance.FindButtonSFX();
            StartCoroutine(FindFX(currentNumObj));
            currentNumber++;
            GameObject circle = Instantiate(findCircleImg, canvas.transform);
            circle.transform.position = Camera.main.WorldToScreenPoint(currentNumObj.transform.position);
        }
    }

    IEnumerator FindFX(GameObject obj)
    {
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            obj.transform.localScale *= 1.01f;
        }
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            obj.transform.localScale /= 1.01f;
        }
    }

    public void BackButton()
    {
        StartCoroutine(TapAgainToBack());
    }

    IEnumerator TapAgainToBack()
    {
        backButtonState2.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        backButtonState2.SetActive(false);
    }

    public void BackButtonState2()
    {
        StopAllCoroutines();
        StartCoroutine(DelayBack());
    }

    IEnumerator DelayBack()
    {
        yield return new WaitForSeconds(AudioManager.Instance.clickLength);
        SceneManager.LoadScene(0);
    }

    private void ShowNextLevelButton()
    {
        nextLevelButton.SetActive(true);
    }

    public void NextLevel()
    {
        DataManager.Instance.totalNumber++;
        ReplayButtonState2();
    }

    public void SoundButton()
    {
        AudioManager.Instance.SoundButtonOnClick();
    }

    public void ClickButtonSFX()
    {
        AudioManager.Instance.ClickButtonSFX();
    }
}
