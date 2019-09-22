using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public enum CharState{
    WIDLE = 1,
    WALKING = 2,
    RUNNING = 3,
    PLAYER_MODE = 4,
    CAR_MODE = 5,
}
public class CharController : MonoBehaviour
{
    Animator animator;
    public Animation animation; 
    public float speed = 2;
    Rigidbody r;
    Collider collider;
    public float raySize = 10;
    bool widle;

    public bool encostandoNoChao = false;

    public CharState charState;

    public DetectObjects detectObjects;



    // Start is called before the first frame update
    void Start()
    {
        charState = CharState.PLAYER_MODE;
        r = this.GetComponent<Rigidbody>();
        collider = this.GetComponent<Collider>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(charState){
            case CharState.PLAYER_MODE:
                playerMode();
                break;
            case CharState.CAR_MODE:
                carMode();
                break;
        }
        
    }

    //estado carro
    void carMode(){
        transform.position = detectObjects.car.transform.position;
        if (Input.GetKeyUp(KeyCode.E))
        {
            //rouba o carro
            // transform.position = detectObjects.car.transform.position;
            charState = CharState.PLAYER_MODE;
            
            var positionToGo = transform.position;
            positionToGo.y = positionToGo.y + 5;
            transform.position = positionToGo;

            collider.isTrigger = false;
            detectObjects.car.GetComponent<WheelVehicle>().IsPlayer = false;
            
        }
    }
    void playerMode(){
        //movimentacao
        r.velocity = new Vector3(
            Input.GetAxis("Horizontal") * speed * Time.deltaTime,
            0,
             Input.GetAxis("Vertical") * speed * Time.deltaTime);
        verificaEncostandoNoChao();

        
        //animacoes
        if(r.velocity != Vector3.zero )
        {
            widle = false;
            animation.Play("walk 1");
            rotateChar();
        }
        else
        {
            widle = true;
            animator.SetBool("widle", widle);
            animation.CrossFade("widle");
        }
        //
        
        //entrar veiculo
        if (Input.GetKeyUp(KeyCode.E))
        {
            enterCar();
        }
        

        
    }

    void verificaEncostandoNoChao(){
        encostandoNoChao = raycast();

        if(encostandoNoChao)
            r.useGravity = false;
        else
            r.useGravity = true;
    }

    //entra no carro
    void enterCar(){
         if(detectObjects.car != null){//verifica carro proximo
            
                //rouba o carro
                charState = CharState.CAR_MODE;
                r.velocity = Vector3.zero;
                r.useGravity = false;
                collider.isTrigger = true;
                //r.detectionCollisions = false;
                animation.Play("widle");

                var positionToGo = detectObjects.car.transform.position;
                transform.position = positionToGo;

                detectObjects.car.GetComponent<WheelVehicle>().IsPlayer = true;
        }
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

    bool raycast(){
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;
        
        var _encostandoNoChao = false;
        
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
            _encostandoNoChao = true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * raySize, Color.green);
            //Debug.Log("Did not Hit");
            _encostandoNoChao = false;
        }
        
        return _encostandoNoChao;
        
        
    }

   
}
