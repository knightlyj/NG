using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class GameWindow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    RectTransform rect;
    // Use this for initialization
    void Start()
    {
        rect = transform as RectTransform;
        Button btnClose = transform.FindChild("Close").GetComponent<Button>();
        btnClose.onClick.AddListener(this.OnCloseClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (drag)
        {
            rect.anchoredPosition = startDragPos + (Input.mousePosition - startDragMousPos);
        }
    }

    void OnDestroy()
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
        this.gameObject.SetActive(false);
    }

}
