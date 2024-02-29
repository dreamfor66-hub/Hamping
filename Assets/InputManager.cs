using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputState
{
    //AFK = 0,
    Idle = 1,
    Hold = 2,
}

public class InputManager : MonoBehaviour
{

    Vector2 touchStartPoint;
    Vector2 touchEndPoint;
    Vector2 touchCurrentPoint;

    public bool isTouch;
    public bool isDrag;

    //������
    bool isOneTap;
    float TapTimer = 0.0f;
    float doubleTapTime = 0.3f;

    public Canvas canvas;

    public HamBehaviour ham;

    public GameObject timeStopPanel;
    public GameObject dragArrow;
    GameObject TimeStopPanel;
    GameObject DragArrow;

    public InputState inputState = InputState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        inputState = InputState.Idle;
    }

    private void Update()
    {
        switch (ham.hamState)
        {
            case HamState.Bound:
            case HamState.Idle:
                if (Input.GetMouseButtonDown(0)) // ���� ����
                {
                    inputState = InputState.Hold;
                    touchStartPoint = Input.mousePosition;
                    isTouch = true;
                    TimeStopPanel = Instantiate(timeStopPanel,canvas.transform);
                }

                if (isTouch && Input.GetMouseButton(0)) // ������ �ִ� ����
                {
                    touchCurrentPoint = Input.mousePosition;
                    if ((touchCurrentPoint - touchStartPoint).magnitude > 200f)
                    {
                        isDrag = true;
                        if (DragArrow == null)
                            DragArrow = Instantiate(dragArrow, Camera.main.WorldToScreenPoint(ham.transform.position), Quaternion.Euler(touchCurrentPoint - touchStartPoint) ,canvas.transform);
                    }
                    if (DragArrow != null)
                    {
                        DragArrow.transform.position = (Camera.main.WorldToScreenPoint(ham.transform.position));
                        DragArrow.transform.rotation = Quaternion.Euler(0, 0, 180 + Vector2.SignedAngle(Vector2.up, touchCurrentPoint - touchStartPoint));
                        DragArrow.transform.localScale = new Vector3(1, Mathf.Clamp((touchCurrentPoint - touchStartPoint).magnitude/1000f, 0f, 1.5f), 1);
                    }
                }
                //Debug.Log((touchCurrentPoint - touchStartPoint).magnitude);

                if (isDrag && Input.GetMouseButtonUp(0)) // ���� �̻� ������ �ڿ� �� ��, �߻� ���� ����
                {
                    touchEndPoint = Input.mousePosition;
                    
                    var dir = touchStartPoint - touchEndPoint;
                    ham.Shoot(dir);

                    Destroy(DragArrow);
                    DragArrow = null;
                }

                if (Input.GetMouseButtonUp(0)) // �Ÿ� ���� ���� �� �� ����
                {
                    isTouch = false;
                    isDrag = false;
                    inputState = InputState.Idle;
                    Debug.Log(touchStartPoint + " => " + touchEndPoint);

                    Destroy(TimeStopPanel);
                    TimeStopPanel = null;
                }

                // ������
                if (Input.GetMouseButtonDown(0))
                {
                    if (!isOneTap)
                    {
                        isOneTap = true;
                        TapTimer = doubleTapTime;
                    }
                    else if (isOneTap && (TapTimer > 0))
                    {
                        isOneTap = false;
                        TapTimer = 0f;
                        //������ ������ �� �� ����
                        ham.TriggerStomp();
                    }
                }

                break;
            case HamState.Jump:

                if (Input.GetMouseButtonDown(0))
                {
                    if (!isOneTap)
                    {
                        isOneTap = true;
                        TapTimer = doubleTapTime;
                    }
                    else if (isOneTap && (TapTimer > 0))
                    {
                        isOneTap = false;
                        TapTimer = 0f;
                        //������ ������ �� �� ����
                        ham.TriggerStomp();
                    }
                }


                break;
            case HamState.Stomp:
                touchEndPoint = Input.mousePosition;

                Destroy(DragArrow);
                DragArrow = null;
                isTouch = false;
                isDrag = false;
                inputState = InputState.Idle;
                Debug.Log(touchStartPoint + " => " + touchEndPoint);

                Destroy(TimeStopPanel);
                TimeStopPanel = null;
                break;
        }
        switch (inputState)
        {
            case InputState.Hold:
                Time.timeScale = 0.05f;
                break;
            case InputState.Idle:
                Time.timeScale = 1f;
                break;

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (ham.hamState)
        {
            case HamState.Bound:
            case HamState.Idle:
            case HamState.Jump:
            case HamState.Stomp:
                if (TapTimer < 0)
                {
                    isOneTap = false;
                }
                if (isOneTap)
                    TapTimer -= Time.deltaTime;
                break;
        }


        
    }
}
