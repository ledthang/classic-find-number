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

    private int _isMuted;
    private bool isMuted
    {
        set
        {
            _isMuted = value ? 1 : 0;
            SaveSoundState();
        }
        get
        {
            LoadSoundState();
            return _isMuted == 1 ? true : false;
        }
    }

    private float volume
    {
        get
        {
            return isMuted ? 0 : 1;
        }
    }

    [SerializeField] AudioClip butttonClickClip;
    [SerializeField] AudioClip drawClip;
    [SerializeField] AudioClip findClip;
    [SerializeField] AudioClip loseClip;
    [SerializeField] AudioClip noHighscoreClip;
    [SerializeField] AudioClip highscoreClip;
    public float clickLength;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        DontDestroyOnLoad(this);

        if (!PlayerPrefs.HasKey("IsSoundMuted"))
        {
            isMuted = false;
        }
        else
        {
            LoadSoundState();
        }

        clickLength = butttonClickClip.length;
    }

    public void LoadButton()
    {
        soundButton = GameObject.Find("Canvas/Sound Button").GetComponent<Button>();
        //Debug.Log(soundButton == null ? "cant find sound button" : "finded sound button");
        onImg = GameObject.Find("Canvas/Sound Button/on");
        offImg = GameObject.Find("Canvas/Sound Button/off");
        UpdateSoundState();
    }

    private void LoadSoundState()
    {
        _isMuted = PlayerPrefs.GetInt("IsSoundMuted");
    }
    private void SaveSoundState()
    {
        PlayerPrefs.SetInt("IsSoundMuted", _isMuted);
    }

    private void UpdateSoundState()
    {
        onImg.SetActive(!isMuted);
        offImg.SetActive(isMuted);
        //Debug.Log("Mute: " + isMuted);
    }
    public void SoundButtonOnClick()
    {
        isMuted = !isMuted;
        UpdateSoundState();
    }

    public void DrawCircleSFX()
    {
        AudioSource.PlayClipAtPoint(drawClip, transform.position, volume);
    }
    public void ClickButtonSFX()
    {
        AudioSource.PlayClipAtPoint(butttonClickClip, transform.position, volume);
    }
    public void FindButtonSFX()
    {
        AudioSource.PlayClipAtPoint(findClip, transform.position, volume);
    }
    public void LoseSFX()
    {
        AudioSource.PlayClipAtPoint(loseClip, transform.position, volume);
    }
    public void NoHighscoreSFX()
    {
        AudioSource.PlayClipAtPoint(noHighscoreClip, transform.position, volume);
    }
    public void HighscoreSFX()
    {
        AudioSource.PlayClipAtPoint(highscoreClip, transform.position, volume);
    }
}
