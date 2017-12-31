using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class UIDialogOpt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler  {
    Text textComp;
    Color normalColor;
    Color coverColor;
    void Awake()
    {
        normalColor = Helper.HexToColor(0xFFFF63);
        coverColor = Helper.HexToColor(0xB87333);
        textComp = GetComponent<Text>();
        textComp.fontSize = GameSetting.textFontSize;
    }

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    int index;
    public delegate void OnOptionChosen(int idx);
    public OnOptionChosen OptionChosenEvent;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        SetCover(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        SetCover(false);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if(OptionChosenEvent != null)
        {
            OptionChosenEvent(this.index);
        }
    }

    public void SetOption(string opt, OnOptionChosen onChosen, int idx)
    {
        if (opt == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            this.index = idx;
            textComp.text = idx + ". " + opt;
            OptionChosenEvent = onChosen;
            gameObject.SetActive(true);
        }
    }

    public void SetCover(bool chosen)
    {
        if (chosen)
            textComp.color = coverColor;
        else
            textComp.color = normalColor;
    }
}
