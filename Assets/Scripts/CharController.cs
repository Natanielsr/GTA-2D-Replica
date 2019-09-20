using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    public float speed = 2;
    Rigidbody r;
    public float raySize = 10;

    public bool encostandoNoChao = false;

    // Start is called before the first frame update
    void Start()
    {
        r = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        r.velocity = new Vector3(
            Input.GetAxis("Horizontal") * speed * Time.deltaTime,
            0,
             Input.GetAxis("Vertical") * speed * Time.deltaTime);

             raycast();

             if(encostandoNoChao)
                r.useGravity = false;
             else
                r.useGravity = true;
    }

    void raycast(){
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position,
         transform.TransformDirection(Vector3.down),
          out hit, raySize))
        {
            Debug.DrawRay(transform.position,
             transform.TransformDirection(Vector3.down) * hit.distance,
              Color.red);
            //Debug.Log("Did Hit");
            encostandoNoChao = true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * raySize, Color.green);
            //Debug.Log("Did not Hit");
            encostandoNoChao = false;
        }
    }
}
