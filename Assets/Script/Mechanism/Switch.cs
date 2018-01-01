using UnityEngine;
using System.Collections;

public class Switch : Handle
{
    [SerializeField]
    Sprite spriteOn = null;
    [SerializeField]
    Sprite spriteOff = null;

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
        if (mechanism != null)
        {
            mechanism.Trigger();
        }
    }
}