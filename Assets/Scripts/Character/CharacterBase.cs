using System.Collections;
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

    public GameObject Model;

    public float speed = 2;

    public float RunSpeed = 4;
    

    public bool Grounded = false;
    public float footPosY = 0.86f;
    public float footRadius = 1f;
    private Vector3 posFoot;

    public WheelVehicle car;

    public float pitchWalk = 1.15f;
    public float pitchRun = 2.30f;

    //punch
    public LayerMask PunchObjects;

    public SoundController soundController;

    protected List<GameObject> Weapons;
    protected int weaponSelected = 0;

    public GameObject PunchPrefab;


    // Start is called before the first frame update
    void Start()
    {
        Weapons = new List<GameObject>();
        Weapons.Add(PunchPrefab);

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

    public Weapon ChangeWeapon(int numbWeapon)
    {
        if (numbWeapon >= 0 && numbWeapon < Weapons.Count)
        {
            weaponSelected = numbWeapon;
            if (Weapons[weaponSelected].GetComponent<Weapon>().Type == "Pistol")
            {
                animator.SetBool("Gun", true);
                Debug.Log(animator.GetBool("Gun"));
            }
            else
            {
               // Debug.Log("sem gun");
                animator.SetBool("Gun", false);
                Debug.Log(animator.GetBool("Gun"));
            }
            return Weapons[weaponSelected].GetComponent<Weapon>();
        }
        return null;
    }

    void _walkingMode()
    {

        //do
        detectObjects.detectCarNearly();

        posFoot = transform.position;
        posFoot.y += footPosY;

        Grounded = isGrounded();

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
        switch (CharMode)
        {
            case CharacterMode.WALKING_MODE:

                

                animator.SetBool("Grounded", Grounded);

                break;
            case CharacterMode.CAR_MODE:

                break;
        }

        _lateUpdate();
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
                Model.SetActive(false);

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

        Model.SetActive(true);

        collider.isTrigger = false;
        rigidbody.useGravity = true;
        // detectObjects.carNearby.GetComponent<WheelVehicle>().IsPlayer = false;

        var car = detectObjects.carNearby.GetComponent<WheelVehicle>();
        if (car.GetCarOwner().gameObject.tag == "Player")
        {
            car.IsPlayer = false;
        }
        car.ResetCarOwner();
        car = null;
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
    protected virtual void _lateUpdate() { }
    public abstract float GetInput(string input);

    public WheelVehicle GetCar()
    {
        var c = detectObjects.carNearby.GetComponent<WheelVehicle>();

        return c;
    }

    public void Punch()
    {
        Vector3 dir = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        Vector3 start = new Vector3(
            transform.position.x, transform.position.y+4f, transform.position.z);

        float distance = 3f;

        Debug.DrawRay(start, dir * distance, Color.green);

        if (Physics.Raycast(start, dir, out hit, distance, PunchObjects))
        {
            if (hit.transform.tag == "citizen")
            {
                soundController.playRndAudioClip();
                hit.transform.GetComponent<CharacterBase>().Die();
            }
        }
        else
        {
        }
    }

}
