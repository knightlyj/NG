using UnityEngine;
using System.Collections;

public class Mechanism : MonoBehaviour
{
    public virtual void Trigger()
    {

    }
    public virtual bool canTrigger { get { return true; } }
}


