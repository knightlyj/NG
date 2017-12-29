using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class GameWindow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    RectTransform rect;
    public void Awake()
    {
        rect = transform as RectTransform;
        Button btnClose = transform.FindChild("Close").GetComponent<Button>();
        btnClose.onClick.AddListener(this.OnCloseClicked);
    }

    // Use this for initialization
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (drag)
        {
            rect.anchoredPosition = startDragPos + (Input.mousePosition - startDragMousPos);
        }
    }

    public void OnDestroy()
    {
        Button btnClose = transform.FindChild("Close").GetComponent<Button>();
        btnClose.onClick.RemoveAllListeners();
    }

    bool drag = false;
    Vector3 startDragPos;
    Vector3 startDragMousPos;
    public void OnPointerDown(PointerEventData eventData)
    {
        drag = true;
        startDragPos = rect.anchoredPosition;
        startDragMousPos = Input.mousePosition;
        rect.SetAsLastSibling();
        //Debug.Log("down " + gameObject.name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        drag = false;
        //Debug.Log("up " + gameObject.name);
    }

    void OnCloseClicked()
    {
        HideWnd();
    }

    public void ShowWnd()
    {
        this.gameObject.SetActive(true);
        rect.SetAsLastSibling();
    }

    public void HideWnd()
    {
        this.gameObject.SetActive(false);
    }

    public void SwitchShowAndHide()
    {
        if (this.gameObject.activeInHierarchy == false)
            ShowWnd();
        else if (this.gameObject.activeInHierarchy == true)
            HideWnd();
    }
}
