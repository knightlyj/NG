using UnityEngine;
using System.Collections;

public class UIBase : MonoBehaviour {

	// Use this for initialization
	virtual public void Start () {
	
	}

    // Update is called once per frame
    virtual public void Update () {
	
	}

    virtual public bool HandleInput(KeyCode key, Vector2 pos)
    {
        return false;
    }
}
