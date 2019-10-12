﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public enum CharacterState
{
    ALIVE,
    DEAD
}
public enum CharacterMode
{
    NONE = 0,
    WALKING_MODE = 4,
    CAR_MODE = 5,
}
public abstract class CharacterBase : MonoBehaviour
{
    public CharacterState CharState;
    public CharacterMode CharMode;

    protected Collider collider;
    public Animator animator;
    protected Rigidbody rigidbody;
    protected AudioSource audioSource;

    protected DetectObjects detectObjects;

    public float speed = 2;
    

    public bool grounded = false;
    public float footPosY = 0.86f;
    public float footRadius = 1f;
    private Vector3 posFoot;

    [Range(-1.0f, 1.0f)]
    public float Vertical;

    [Range(-1.0f, 1.0f)]
    public float Horizontal;


    // Start is called before the first frame update
    void Start()
    {
        
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        detectObjects = GetComponent<DetectObjects>();
        if (animator == null)
            Debug.LogError("Defina o animator do personagem");

        CharState = CharacterState.ALIVE;
        CharMode = CharacterMode.WALKING_MODE;

        _start();
    }

    private void FixedUpdate()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        switch (CharMode)
        {
            case CharacterMode.WALKING_MODE:
                _walkingMode();
                break;
            case CharacterMode.CAR_MODE:
                _carMode();
                break;
        }

        _update();
    }

    void _walkingMode()
    {

        //do
        detectObjects.detectCarNearly();

        posFoot = transform.position;
        posFoot.y += footPosY;

        grounded = isGrounded();

        WalkingMode();
    }

    void _carMode()
    {
        //do

        transform.position = detectObjects.carNearby.transform.position;
        transform.rotation = detectObjects.carNearby.transform.rotation;

        CarMode();
    }

    private void LateUpdate()
    {
        
    }


    //entra no carro
    protected void enterCar()
    {
        
        if (detectObjects.carNearby != null )
        {//verifica carro proximo
            var car = detectObjects.carNearby.GetComponent<WheelVehicle>();
            if (car.GetCarOwner() == null)
            {
                //entra no carro
                CharMode = CharacterMode.CAR_MODE;
                rigidbody.velocity = Vector3.zero;
                rigidbody.useGravity = false;
                collider.isTrigger = true;

                var positionToGo = detectObjects.carNearby.transform.position;
                transform.position = positionToGo;
                transform.rotation = detectObjects.carNearby.transform.rotation;

                //


                car.SetCarOwner(this);
                if (car.GetCarOwner().gameObject.tag == "Player") {
                    car.IsPlayer = true;
                }
            }
        }
    }

    protected void exitCar() {
        //sai do carro
        CharMode = CharacterMode.WALKING_MODE;

        var positionToGo = transform.position;
        positionToGo.y = positionToGo.y + 5;
        transform.position = positionToGo;


        collider.isTrigger = false;
        rigidbody.useGravity = true;
        // detectObjects.carNearby.GetComponent<WheelVehicle>().IsPlayer = false;

        var car = detectObjects.carNearby.GetComponent<WheelVehicle>();
        if (car.GetCarOwner().gameObject.tag == "Player")
        {
            car.IsPlayer = false;
        }
        car.ResetCarOwner();
    }

    public void Die() {
        CharState = CharacterState.DEAD;
        collider.isTrigger = true;
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;

        base_die();
    }
    

    public bool isGrounded()
    {

        var hitColliders = Physics.OverlapSphere(posFoot, footRadius);
        if (hitColliders.Length > 0)
        {
            foreach (var col in hitColliders)
            {
                if (col.gameObject.tag != "Player")
                {
                    //Debug.Log(hitColliders);
                    return true;
                }
            }

        }

        return false;


    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Color newColor = new Color(0.3f, 0.4f, 0.6f, 0.75f);

        Gizmos.color = newColor;
        Gizmos.DrawSphere(posFoot, footRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }


    protected abstract void base_die();
    protected virtual void WalkingMode() { }
    protected virtual void CarMode() { }
    protected virtual void _start() { }
    protected virtual void _update() { }
    public abstract float GetInput(string input);

    public WheelVehicle GetCar()
    {
        var c = detectObjects.carNearby.GetComponent<WheelVehicle>();

        return c;
    }

}
