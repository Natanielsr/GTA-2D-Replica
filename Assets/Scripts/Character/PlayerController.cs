using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public class PlayerController : CharacterBase
{   


    //estado carro
    protected override void CarMode()
    {
        if (Input.GetButtonUp("Interact"))
        {
            exitCar();
        }
    }
    protected override void WalkingMode()
    {
        //entrar veiculo
        if (Input.GetButtonUp("Interact"))
        {
            enterCar();
        }

    }

    // Start is called before the first frame update
    protected override void _start()
    {
        if (animator == null)
            Debug.Log("defina o animator");
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
        animator.transform.localPosition = Vector3.zero;
        animator.transform.localRotation = new Quaternion(0f,0f,0f,0f);
        //Debug.Log(Input.GetAxis("CarAceleration"));
    }

    void LateUpdate()
    {
        //animacoes
        if (CharMode == CharacterMode.WALKING_MODE)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    //run
                    animator.SetFloat("RunZ", 0.9f);
                    audioSource.pitch = pitchRun;
                }
                else
                {
                    //walk
                    animator.SetFloat("RunZ", 0.6f);
                    audioSource.pitch = pitchWalk;
                }

                audioSource.enabled = true;
            }
            else
            {
                animator.SetFloat("RunZ", 0f);
                audioSource.enabled = false;
            }
            rotateChar();
        }
        else if(CharMode == CharacterMode.CAR_MODE)
        {
            audioSource.enabled = false;
            animator.SetFloat("RunZ", 0f);

        }
        //
    }

    
    
    void movement(){

        var currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = RunSpeed;
        }
        else
        {
            currentSpeed = speed;
        }

        rigidbody.velocity = new Vector3(
            Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime,
            rigidbody.velocity.y,
             Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime);
        
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

    public override float GetInput(string input)
    {
        return Input.GetAxis(input);
    }
}
