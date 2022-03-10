using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private Button soundButton;
    private GameObject onImg;
    private GameObject offImg;

    private bool isMuted = false;

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
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        LoadButton();
    }

    public void LoadButton()
    {
        soundButton = GameObject.Find("Canvas/Sound Button").GetComponent<Button>();
        Debug.Log(soundButton == null ? "cant find sound button" : "finded sound button");
        onImg = GameObject.Find("Canvas/Sound Button/on");
        offImg = GameObject.Find("Canvas/Sound Button/off");
    }

    public void SoundButtonOnClick()
    {
        isMuted = !isMuted;
        onImg.SetActive(!isMuted);
        offImg.SetActive(isMuted);
        Debug.Log("Mute: " + isMuted);
    }
}
