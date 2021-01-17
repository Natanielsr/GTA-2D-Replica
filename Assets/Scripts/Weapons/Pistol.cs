using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public int MaximumCockedBullets = 15;
    public int MaximumStoredBullets = 30;
    private int currentStoredBullets = 30;
    private int currentAmmo;

    public float FireCadence = 0.1f;
    private bool cocked; 

    public GameObject BulletPrefab;

    public SoundController soundController;

    void Start()
    {
        soundController = FindObjectOfType<SoundController>();
    }

    public override void StartWeapon()
    {
        cocked = true;
        currentAmmo = MaximumCockedBullets;
    }
    

    protected override void _interact()
    {
        if (currentAmmo > 0 && cocked)
        {
            soundController.playShot();
            var bf = Instantiate(BulletPrefab, transform.position, transform.rotation);
            currentAmmo--;
            cocked = false;
            StartCoroutine(cockedPistol());
        }
        else if(currentAmmo == 0)
        {
            reload();
        }

    }

    void reload()
    {
        var bulletsNeeded = MaximumCockedBullets - currentAmmo;
        if (bulletsNeeded < currentStoredBullets)
        {
            currentAmmo += bulletsNeeded;
            currentStoredBullets -= bulletsNeeded;
        }
        else
        {
            currentAmmo += currentStoredBullets;
            currentStoredBullets = 0;
        }



    }

    IEnumerator cockedPistol()
    {
        yield return new WaitForSeconds(FireCadence);
        cocked = true;
    }
}
