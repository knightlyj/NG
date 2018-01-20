using UnityEngine;
using System.Collections;

public class Switch : Handle
{
    [SerializeField]
    Sprite spriteOn = null;
    [SerializeField]
    Sprite spriteOff = null;

    SpriteRenderer sr = null;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
            if (mechanism != null)
                return this._canManipulate && this.mechanism.canTrigger;
            else
                return this._canManipulate;
        }
    }

    bool on = false;
    public Mechanism mechanism = null;
    public override void Manipulate(Player player)
    {
        on = !on;
        if (on)
        {
            if (spriteOn != null)
                sr.sprite = spriteOn;
        }
        else
        {
            if (spriteOff != null)
                sr.sprite = spriteOff;
        }

        if (mechanism != null)
        {
            mechanism.Trigger();
        }
    }
}