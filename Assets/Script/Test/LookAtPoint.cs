using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LookAtPoint : MonoBehaviour
{
    public Vector3 lookAtPoint = Vector3.zero;
    public int a;

    void Update()
    {
        transform.LookAt(lookAtPoint);
    }
}