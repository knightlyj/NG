using UnityEngine;
using System.Collections;

public class Handle : MonoBehaviour
{
    public virtual bool canManipulate { get { return true; } }
    public virtual void Manipulate(Player player)
    {

    }
}


