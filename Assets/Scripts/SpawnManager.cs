using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject numberButton;
    private float minHeight;
    float loopBound = 10000;
    void Start()
    {
        minHeight = Screen.height switch
        {
            var n when n > 720 => 90,
            var n when n > 400 => 50,
            _ => 32
        };

        //loopBound  = 10000 * (1 / ((float)DataManager.Instance.totalNumber / 99.0f) * ((float)DataManager.Instance.totalNumber / 99.0f));
       
        //Debug.Log("Screen height " + Screen.height);
        for (int i = 1; i <= DataManager.Instance.totalNumber; i++)
        {
            Vector2 randomPos = GerenateRandomPosition();
            GameObject go = Instantiate(numberButton, randomPos, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)));
            go.transform.localScale *= DataManager.Instance.scaleRatio;
            go.name = i.ToString();
            go.transform.GetComponent<TextMeshPro>().text = i.ToString();
        }
    }

    Vector2 GerenateRandomPosition()
    {
        Vector2 randomPos;
        RaycastHit2D hit;
        int riskManager = 0;        
        do
        {
            riskManager++;
            randomPos = new Vector2(Random.Range(Screen.width * 0.11f, Screen.width * 0.95f), Random.Range(minHeight, Screen.height * 0.95f));
            hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(randomPos), 0.5f, Vector2.zero);
        } while (hit.collider != null && riskManager < loopBound);

        return Camera.main.ScreenToWorldPoint(randomPos);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
