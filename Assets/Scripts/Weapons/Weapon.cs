using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public void Interact()
    {
        _interact();
    }

    protected virtual void _interact() { }

    public virtual void StartWeapon() { }
}
