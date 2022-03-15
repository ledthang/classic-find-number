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
        AudioManager.Instance.LoadButton();
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
        StartCoroutine(DelayStart());
    }
    public void SingleplayTimeBlitzButton()
    {
        DataManager.Instance.playMode = PlayMode.SingleTimeBlitz;
        StartCoroutine(DelayStart());
    }

    public void MultiplayButton()
    {
        DataManager.Instance.playMode = PlayMode.Multiplay;
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        AdsManager.Instance.ShowInterstitialAd();
#endif
        yield return new WaitForSeconds(AudioManager.Instance.clickLength);
        SceneManager.LoadScene(1);
    }

    public void BackButton()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }
}
