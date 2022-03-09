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
            if (value > 99)
            {
                _currentNumner = 99;
            }
            else _currentNumner = value;
        }
    }
    [SerializeField] TextMeshProUGUI timeText;
    private float time;

    [SerializeField] GameObject canvas;
    [SerializeField] GameObject findCircleImg;

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
        currentNumber = 1;
        time = 0;
    }

    private void Update()
    {
        currentNumberText.text = currentNumber.ToString();
        DisplayTime(time);
        time += Time.deltaTime;
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
            currentNumber++;
            GameObject circle = Instantiate(findCircleImg, canvas.transform);
            circle.transform.position = Camera.main.WorldToScreenPoint(currentNumObj.transform.position);
        }
    }
}
