using UnityEngine.UI;
using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class UIDialog : MonoBehaviour {
    [SerializeField]
    RectTransform[] trOptions = null;

    const float textToSide = 30;
    const float textInterval = 15;

    const float textWidth = 500;
    const float optionHeight = 30;
    const float dialogWidth = textWidth + textToSide * 2;

    Text txtContent = null;
    void Awake()
    {
        Init();
    }
    
    // Use this for initialization
    void Start () {

    }
    
	// Update is called once per frame
	void Update () {

    }

    void Init()
    {
        RectTransform rectContent = transform.FindChild("Content") as RectTransform;
        rectContent.anchoredPosition = new Vector2(0, -textToSide);
        txtContent = rectContent.GetComponent<Text>();
        txtContent.fontSize = GameSetting.textFontSize;
    }
    
    void Chosen(int idx)
    {
        Debug.Log("choose " + idx);
    }

    public void ShowDialog(string content, string[] options, UIDialogOpt.OnOptionChosen optionChosen)
    {
        this.gameObject.SetActive(true);
        txtContent.text = content;
        float height = 0;
        height = txtContent.preferredHeight;

        //最多3条选项
        height += textToSide + textInterval;
        for(int i = 0; i < 3; i++)
        {
            UIDialogOpt dialogOpt = trOptions[i].GetComponent<UIDialogOpt>();
            if (i < options.Length)
            {
                dialogOpt.SetOption(options[i], optionChosen, i);
                dialogOpt.SetCover(false);
                trOptions[i].anchoredPosition = new Vector2(0, -height);
                height += textInterval / 2 + optionHeight;
            }
            else
            {
                dialogOpt.SetOption(null, null, i + 1);
            }
        }

        height += textToSide / 2;
        RectTransform rect = transform as RectTransform;
        rect.offsetMin = rect.anchoredPosition - new Vector2(dialogWidth / 2, height / 2);
        rect.offsetMax = rect.offsetMin + new Vector2(dialogWidth, height);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
