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
                _currentNumner = DataManager.Instance.totalNumber;
            }
            else _currentNumner = value;
        }
    }
    [SerializeField] TextMeshProUGUI timeText;
    private float time;

    [SerializeField] GameObject canvas;
    [SerializeField] GameObject findCircleImg;

    private float startTime; //time blitz
    private float timeToAdd; //time blitz

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
    }

    private void Start()
    {
        AudioManager.Instance.LoadButton();
        currentNumber = 1;
        if (DataManager.Instance.playMode == PlayMode.SingleTimeBlitz)
        {
            time = 60 * DataManager.Instance.totalNumber / 99.0f;
            timeToAdd = 15 * DataManager.Instance.totalNumber / 99.0f;
        }
        else
        {
            time = 0;
        }
        Debug.Log("Mode: " + DataManager.Instance.playMode);
        Debug.Log("Total Number: " + DataManager.Instance.totalNumber);
        Debug.Log("Scale: " + DataManager.Instance.scaleRatio);
    }

    private void Update()
    {
        currentNumberText.text = currentNumber.ToString();
        DisplayTime(time);
        if (DataManager.Instance.playMode == PlayMode.SingleTimeBlitz)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time += Time.deltaTime;
        }
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
        timeText.text = min + ":" + (seconds < 10 ? "0" : "") + seconds;
    }

    public void ReplayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void FindButton()
    {

        GameObject currentNumObj = GameObject.Find(currentNumber.ToString());
        if (currentNumObj != null)
        {
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
        SceneManager.LoadScene(0);
    }

    public void SoundButton()
    {
        AudioManager.Instance.SoundButtonOnClick();
    }
}
