using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPath : MonoBehaviour
{
    public CarPath NextPath;

    void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawSphere(transform.position, 5.0f);
    }
}
