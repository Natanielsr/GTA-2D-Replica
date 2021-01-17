using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public string Type;
    public Sprite Image;
    public void Interact()
    {
        _interact();
    }

    protected virtual void _interact() { }

    public virtual void StartWeapon() { }
}
