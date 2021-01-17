using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour
{
    public Image gunImageUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setGunImage(Sprite image)
    {
        gunImageUI.sprite = image;
    }
}
