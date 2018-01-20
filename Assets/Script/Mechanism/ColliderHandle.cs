using UnityEngine;
using System.Collections;

public class ColliderHandle : Handle {
    Vector2 originlaPos;
    Vector2 holdPos;
    
    void Awake()
    {
        float moveDst = -0.3f;
        originlaPos = transform.position;
        holdPos = originlaPos + (Vector2)(transform.rotation * Vector2.right * moveDst);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    bool on = false;
    public override bool canManipulate { get { return true; } }
    public override void Manipulate(Player player)
    {

    }
}
