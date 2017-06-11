using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftWnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button btnCraft = transform.FindChild("Craft").GetComponent<Button>();
        btnCraft.onClick.AddListener(this.OnCraftClick);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        Button btnCraft = transform.FindChild("Craft").GetComponent<Button>();
        btnCraft.onClick.RemoveAllListeners();
    }

    void OnCraftClick()
    {
        Debug.Log("craft");
    }
}
