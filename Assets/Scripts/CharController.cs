using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharState{
    WIDLE = 1,
    WALKING = 2,
    RUNNING = 3
}
public class CharController : MonoBehaviour
{
    Animator animator;
    public Animation animation; 
    public float speed = 2;
    Rigidbody r;
    public float raySize = 10;
    bool widle;

    public bool encostandoNoChao = false;

    public CharState charState;



    // Start is called before the first frame update
    void Start()
    {
        charState = CharState.WIDLE;
        r = this.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        r.velocity = new Vector3(
            Input.GetAxis("Horizontal") * speed * Time.deltaTime,
            0,
             Input.GetAxis("Vertical") * speed * Time.deltaTime);

        
        if(r.velocity != Vector3.zero )
        {
            widle = false;
            animation.Play("walk 1");
            rotateChar();
        }
        else
        {
            widle = true;
            
            animation.CrossFade("widle");
        }
        animator.SetBool("widle", widle);
        

        raycast();

        if(encostandoNoChao)
            r.useGravity = false;
        else
            r.useGravity = true;
    }

    void rotateChar(){
        if(r.velocity.x > 0 && r.velocity.z > 0){
            transform.eulerAngles = new Vector3(0, 45, 0);
        }
        else if(r.velocity.x < 0 && r.velocity.z < 0){
            transform.eulerAngles = new Vector3(0, -135, 0);
        }
         else if(r.velocity.x < 0 && r.velocity.z > 0){
            transform.eulerAngles = new Vector3(0, -45, 0);
        }
         else if(r.velocity.x > 0 && r.velocity.z < 0){
            transform.eulerAngles = new Vector3(0, 135, 0);
        }
        else if(r.velocity.x < 0){
            transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if(r.velocity.x > 0){
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if(r.velocity.z > 0){
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if(r.velocity.z < 0){
            transform.eulerAngles = new Vector3(0, 180, 0);
        }



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
