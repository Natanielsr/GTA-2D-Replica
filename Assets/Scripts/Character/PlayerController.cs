using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public class PlayerController : CharacterBase
{
    public float JumpSpeed = 2;
    public Transform WeaponPostion;

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

        ChangingWeapon();

    }

    void ChangingWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            ChangeWeapon(weaponSelected + 1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") <0f) // backwards
        {
            ChangeWeapon(weaponSelected - 1);
        }
    }

    // Start is called before the first frame update
    protected override void _start()
    {
        if (animator == null)
            Debug.LogError("defina o animator");
        else
        {
            Weapons[0].GetComponent<Punch>().SetAnimator(animator);
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //update on physics update
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

    protected override void _lateUpdate()
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

            if (Input.GetButton("Fire1") && Grounded)
            {
                if(Weapons.Count > 0)
                    this.Weapons[weaponSelected].GetComponent<Weapon>().Interact();
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

        var jump = rigidbody.velocity.y;
        if (Input.GetKey(KeyCode.Space) && Grounded)
        {
            jump = JumpSpeed * Time.deltaTime;
        }

        rigidbody.velocity = new Vector3(
            Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime,
            jump,
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "weapon")
        {
            var weaponPrefab = other.GetComponent<PickableItem>().ItemPrefab.GetComponent<Weapon>();
            var weaponObject = Instantiate(weaponPrefab.gameObject,
                WeaponPostion.transform.position, WeaponPostion.transform.rotation,  WeaponPostion);
            
            Weapons.Add(weaponObject);
            weaponObject.GetComponent<Weapon>().StartWeapon();
            Destroy(other.gameObject);
        }
    }
}
