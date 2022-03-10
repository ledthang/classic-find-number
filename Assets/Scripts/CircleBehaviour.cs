using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleBehaviour : MonoBehaviour
{
    [SerializeField] Image circleImg;
    [SerializeField] [Range(0, 1)] float progress = 0;
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
        this.transform.localScale *= Random.Range(0.8f, 1.1f) * DataManager.Instance.scaleRatio;
    }

    // Update is called once per frame
    void Update()
    {
        circleImg.fillAmount = progress;
        progress += Time.deltaTime;
        if (progress > 1)
        {
            Destroy(this);
        };
    }
}
