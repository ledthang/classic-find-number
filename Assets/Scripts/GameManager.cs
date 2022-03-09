using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

class ConstStorage
{
    public const float deltaTime = 0.25f;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] TextMeshProUGUI currentNumberText;
    public int currentNumber;
    [SerializeField] TextMeshProUGUI timeText;
    public int time;

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
    }

    private void Update()
    {
        currentNumberText.text = currentNumber.ToString();
    }
}
