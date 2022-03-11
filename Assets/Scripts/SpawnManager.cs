using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject numberButton;
    private float minHeight;
    private float maxHeight;
    private float minWidth;
    private float maxWidth;
    private float radius;
    private float loopBound = 10000;
    void Start()
    {
        minHeight = 1.25f * Screen.height switch
        {
            var n when n > 720 => 90,
            var n when n > 400 => 50,
            _ => 32
        };
        maxHeight = Screen.height * 0.95f;
        minWidth = Screen.width * 0.1f;
        maxWidth = Screen.width * 0.95f;
        radius = 0.5f * DataManager.Instance.scaleRatio;

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
            randomPos = new Vector2(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
            hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(randomPos), radius, Vector2.zero);
        } while (hit.collider != null && riskManager < loopBound);

        return Camera.main.ScreenToWorldPoint(randomPos);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
