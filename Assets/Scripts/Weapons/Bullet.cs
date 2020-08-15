using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float bulletSpeed = 10;
    private Rigidbody bulletRigid;
    public float timeToDestroy = 5f;

    void Start()
    {
        bulletRigid = GetComponent<Rigidbody>();
        StartCoroutine(DestroyBullet());
        bulletRigid.AddForce(transform.forward * bulletSpeed);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
