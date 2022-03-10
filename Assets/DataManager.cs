using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayMode
{
    SingleNormal,
    SingleTimeBlitz,
    Multiplay
}
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private int _totalNumber;
    public int totalNumber
    {
        set
        {
            _totalNumber = value;
            SaveTotalNumber();
        }
        get
        {
            LoadTotalNumber();
            return _totalNumber;
        }
    }

    public float scaleRatio;

    public PlayMode playMode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);

        if (!PlayerPrefs.HasKey("totalNumber"))
        {
            totalNumber = 99;
        }
        else
        {
            LoadTotalNumber();
        }
    }

    void LoadTotalNumber()
    {
        _totalNumber = PlayerPrefs.GetInt("totalNumber");
        float rate = (float)((float)_totalNumber * (float)_totalNumber / 99.0f);
        scaleRatio = 1 / rate + 1;
    }
    void SaveTotalNumber()
    {
        PlayerPrefs.SetInt("totalNumber", _totalNumber);
    }
}
