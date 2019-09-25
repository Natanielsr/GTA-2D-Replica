using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : CharacterBase
{
    public Animation animation;

    

    //estado carro
    protected override void CarMode()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            exitCar();
        }
    }
    protected override void WalkingMode()
    {
        //entrar veiculo
        if (Input.GetKeyUp(KeyCode.E))
        {
            enterCar();
        }

    }

    // Start is called before the first frame update
    protected override void _start()
    {
        if (animation == null)
            Debug.Log("defina o animation");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(CharMode == CharacterMode.WALKING_MODE)
        {
            movement();
        }

    }

    protected override void _update()
    {
        

    }

    void LateUpdate()
    {
        //animacoes
        if (CharMode == CharacterMode.WALKING_MODE)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                animation.Play("walk 1");
                audioSource.enabled = true;

            }
            else
            {
                animation.CrossFade("widle");
                audioSource.enabled = false;
            }
            rotateChar();
        }
        else if(CharMode == CharacterMode.CAR_MODE)
        {
            audioSource.enabled = false;
            animation.CrossFade("widle");

        }
        //
    }

    
    
    void movement(){
        rigidbody.velocity = new Vector3(
            Input.GetAxis("Horizontal") * speed * Time.deltaTime,
            rigidbody.velocity.y,
             Input.GetAxis("Vertical") * speed * Time.deltaTime);
        
    }
    

    void rotateChar(){
        if(Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") > 0){
            transform.eulerAngles = new Vector3(0, 45, 0);
        }
        else if(Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") < 0){
            transform.eulerAngles = new Vector3(0, -135, 0);
        }
         else if(Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") > 0){
            transform.eulerAngles = new Vector3(0, -45, 0);
        }
         else if(Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") < 0){
            transform.eulerAngles = new Vector3(0, 135, 0);
        }
        else if(Input.GetAxis("Horizontal") < 0){
            transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if(Input.GetAxis("Horizontal") > 0){
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if(Input.GetAxis("Vertical") > 0){
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if(Input.GetAxis("Vertical") < 0){
            transform.eulerAngles = new Vector3(0, 180, 0);
        }



    }

    protected override void base_die()
    {
        throw new System.NotImplementedException();
    }
}
