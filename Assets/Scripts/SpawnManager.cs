using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject numberButton;
    private float minHeight;

    void Start()
    {
        minHeight = Screen.height switch
        {
            var n when n > 720 => 90,
            var n when n > 400 => 50,
            _ => 32
        };

        Debug.Log("Screen height " + Screen.height);
        for (int i = 1; i < 100; i++)
        {
            Vector2 randomPos = GerenateRandomPosition();
            GameObject go = Instantiate(numberButton, randomPos, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)));
            go.name = i.ToString();
            go.transform.GetComponent<TextMeshPro>().text = i.ToString();
        }
    }

    Vector2 GerenateRandomPosition()
    {
        Vector2 randomPos;
        RaycastHit2D hit;
        do
        {
            randomPos = new Vector2(Random.Range(Screen.width * 0.11f, Screen.width * 0.95f), Random.Range(minHeight, Screen.height * 0.95f));
            hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(randomPos), 0.5f, Vector2.zero);
        } while (hit.collider != null);

        return Camera.main.ScreenToWorldPoint(randomPos);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
