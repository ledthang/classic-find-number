using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private PlayerInput input;
    InputAction click;
    InputAction doubleclick;
    InputAction clickPosition;

    public bool isDouleClick;

    [SerializeField] GameObject canvas;
    [SerializeField] GameObject circleImg1;
    [SerializeField] GameObject circleImg2;

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

        input = new PlayerInput();
        click = input.Player.Click;
        click.Enable();
        click.performed += (ctx) => HandleClick(ctx);
        //click.performed += (ctx) => HandleSingleClick(ctx);
        doubleclick = input.Player.DoubleClick;
        doubleclick.Enable();
        //doubleclick.performed += (ctx) => HandleDoubleClick(ctx);
        clickPosition = input.Player.Clickposition;
        clickPosition.Enable();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void HandleClick(InputAction.CallbackContext ctx)
    {
        Ray mousePos2D = Camera.main.ScreenPointToRay(clickPosition.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.GetRayIntersection(mousePos2D);
        if (hit.transform != null)
        {
            if (hit.transform.name == GameManager.Instance.currentNumber.ToString())
            {
                GameManager.Instance.currentNumber++;
                GameObject circle = Instantiate(circleImg1, canvas.transform);
                circle.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position);
            }
        }
    }

    void HandleSingleClick(InputAction.CallbackContext ctx)
    {
        isDouleClick = false;
        StartCoroutine(CheckDoubleClick());
    }
    IEnumerator CheckDoubleClick()
    {
        yield return new WaitForSeconds(ConstStorage.deltaTime);
        if (!isDouleClick)
        {
            Ray mousePos2D = Camera.main.ScreenPointToRay(clickPosition.ReadValue<Vector2>());
            RaycastHit2D hit = Physics2D.GetRayIntersection(mousePos2D);
            if (hit.transform != null)
            {
                if (hit.transform.name == GameManager.Instance.currentNumber.ToString())
                {
                    GameManager.Instance.currentNumber++;
                    GameObject circle = Instantiate(circleImg1, canvas.transform);
                    circle.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position);
                }
            }
        }
    }

    void HandleDoubleClick(InputAction.CallbackContext ctx)
    {
        Debug.Log("Double click at " + clickPosition.ReadValue<Vector2>());
        isDouleClick = true;

        Ray mousePos2D = Camera.main.ScreenPointToRay(clickPosition.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.GetRayIntersection(mousePos2D);

        if (hit.transform != null)
        {
            if (hit.transform.name == GameManager.Instance.currentNumber.ToString())
            {
                GameManager.Instance.currentNumber++;
                GameObject circle = Instantiate(circleImg2, canvas.transform);
                circle.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position);
            }
        }

    }

}
