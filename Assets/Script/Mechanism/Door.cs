using UnityEngine;
using System.Collections;

public class Door : Handle
{
    [SerializeField]
    Sprite spriteOn = null;
    [SerializeField]
    Sprite spriteOff = null;

    SpriteRenderer sr = null;
    void Awake()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    bool _canManipulate = true;
    public override bool canManipulate
    {
        get
        {
            return this._canManipulate;
        }
    }

    bool on = false;
    public override void Manipulate(Player player)
    {
        on = !on;
        if (on)
        {
            if (spriteOn != null)
                sr.sprite = spriteOn;

            sr.gameObject.layer = LayerMask.NameToLayer("Default");
            sr.material.shader = Shader.Find("Custom/SpriteNoOcc");
        }
        else
        {
            if (spriteOff != null)
                sr.sprite = spriteOff;

            sr.gameObject.layer = LayerMask.NameToLayer(TextResources.groundLayer);
            sr.material.shader = Shader.Find("Custom/SpriteWithOcc");
        }
    }
}
