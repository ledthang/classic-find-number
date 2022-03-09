using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberBehaviour : MonoBehaviour
{
    private Button button;
    private TextMeshPro text;
    Color singleClickColor = Color.red;
    Color doubleClickColor = Color.green;

    private void Start()
    {
        button = this.GetComponent<Button>();
        text = this.GetComponent<TextMeshPro>();
        text.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        text.fontSize = Random.Range(6.0f, 8.0f);
    }

    public void ButtonOnClick()
    {
        if (PlayerController.Instance.isDouleClick)
        {
            Debug.Log("button double clicked");
            text.color = doubleClickColor;
            button.interactable = false;
        }
        else
        {
            StartCoroutine(CheckDoubleClick());
        }
    }

    IEnumerator CheckDoubleClick()
    {
        yield return new WaitForSeconds(ConstStorage.deltaTime);
        if (!PlayerController.Instance.isDouleClick)
        {
            text.color = singleClickColor;
            Debug.Log("button single clicked");
            button.interactable = false;
        }
    }
}


