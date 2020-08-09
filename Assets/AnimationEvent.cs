using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public CharacterBase Player;
    public void Punch()
    {
        Player.Punch();
    }
}
