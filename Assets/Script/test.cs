using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class test : MonoBehaviour, IPointerClickHandler {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ValueChangeCheck()
    {
        Debug.Log("Value Changed");
    }
        //
        // Summary:
        //     ///
        //     ///
        //
        // Parameters:
        //   eventData:
        //     Current event data.
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("start eidt");
    }
}
