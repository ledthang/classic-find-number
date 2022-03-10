using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    [Header("Slider")]
    [SerializeField] TextMeshProUGUI singleNumberText;
    [SerializeField] Slider singleNumberSlider;
    
    [SerializeField] TextMeshProUGUI multiNumberText;
    [SerializeField] Slider multiNumberSlider;

    

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

    private void OnEnable()
    {

    }

    private void Update()
    {
        UpdateSliderValue();
    }

    public void SliderOnValueChanged(Slider slider)
    {
        DataManager.Instance.totalNumber = (int)slider.value;
        UpdateText();
    }

    private void UpdateText()
    {
        singleNumberText.text = multiNumberText.text = DataManager.Instance.totalNumber.ToString();
    }

    private void UpdateSliderValue()
    {
        singleNumberSlider.value = multiNumberSlider.value = DataManager.Instance.totalNumber;
    }

    public void SingleplayNormalButton()
    {
        DataManager.Instance.playMode = PlayMode.SingleNormal;
        SceneManager.LoadScene(1);
    }
    public void SingleplayTimeBlitzButton()
    {
        DataManager.Instance.playMode = PlayMode.SingleTimeBlitz;
        SceneManager.LoadScene(1);
    }

    public void MultiplayButton()
    {
        DataManager.Instance.playMode = PlayMode.Multiplay;
        SceneManager.LoadScene(1);
    }
}
