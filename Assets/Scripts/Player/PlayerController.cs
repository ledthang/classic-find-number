using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private PlayerInput input;
    InputAction singleclick;
    InputAction doubleclick;
    InputAction clickPosition;

    public bool isDouleClick;

    [SerializeField] GameObject canvas;
    [SerializeField] GameObject circleImg1;
    [SerializeField] GameObject circleImg2;

    enum ClickType
    {
        SingleClick,
        DoubleClick
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }

    }

    private void OnEnable()
    {
        input = new PlayerInput();
        if (DataManager.Instance.playMode == PlayMode.Multiplay)
        {
            singleclick = input.Player.Click;
            singleclick.Enable();
            singleclick.performed += (ctx) => HandleSingleClick(ctx);
            doubleclick = input.Player.DoubleClick;
            doubleclick.Enable();
            doubleclick.performed += (ctx) => HandleDoubleClick(ctx);
        } else
        {
            singleclick = input.Player.Click;
            singleclick.Enable();
            singleclick.performed += (ctx) => HandleClick(ctx);
        }
        clickPosition = input.Player.Clickposition;
        clickPosition.Enable();
    }

    void Update()
    {

    }
    void HandleClick(InputAction.CallbackContext ctx)
    {
        OnTouchBehaviour(ClickType.SingleClick);
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
            OnTouchBehaviour(ClickType.SingleClick);
        }
    }

    void HandleDoubleClick(InputAction.CallbackContext ctx)
    {
        Debug.Log("Double click at " + clickPosition.ReadValue<Vector2>());
        isDouleClick = true;

        OnTouchBehaviour(ClickType.DoubleClick);
    }

    void OnTouchBehaviour(ClickType clickType)
    {
        Ray mousePos2D = Camera.main.ScreenPointToRay(clickPosition.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.GetRayIntersection(mousePos2D);

        if (hit.transform != null)
        {
            if (hit.transform.name == GameManager.Instance.currentNumber.ToString())
            {
                GameManager.Instance.AddTime();
                GameManager.Instance.currentNumber++;
                GameObject circle = Instantiate(clickType == ClickType.SingleClick ? circleImg1 : circleImg2, canvas.transform);
                circle.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position);
            }
        }
    }

    public void OnDisable()
    {
        StopAllCoroutines();
        singleclick.performed -= (ctx) => HandleSingleClick(ctx);
        singleclick.Disable();
        if (DataManager.Instance.playMode == PlayMode.Multiplay)
        {
            doubleclick.performed -= (ctx) => HandleDoubleClick(ctx);
            doubleclick.Disable();
        }
    }

}
