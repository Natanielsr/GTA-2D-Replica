using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public enum PlayerState{
    NONE = 0,
    PLAYER_MODE = 4,
    CAR_MODE = 5,
}
public class PlayerController : MonoBehaviour
{
    Animator animator;
    public Animation animation;
    Rigidbody r;
    Collider collider;
    AudioSource audioSource;

    public float speed = 2;
    
    public float raySize = 10;
    bool widle;

    public bool grounded = false;
    public float footPosY = 1f;
    public float footRadius = 1f;
    private Vector3 posFoot; 

    public PlayerState playerState;

    public DetectObjects detectObjects;

    bool walking;


    // Start is called before the first frame update
    void Start()
    {
       // charState = CharState.NONE;
        r = this.GetComponent<Rigidbody>();
        collider = this.GetComponent<Collider>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerState = PlayerState.PLAYER_MODE;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerState == PlayerState.PLAYER_MODE)
        {
            //movimentacao
            movement();
        }

    }
    void Update()
    {
        switch(playerState){
            case PlayerState.PLAYER_MODE:
                playerMode();
                break;
            case PlayerState.CAR_MODE:
                carMode();
                break;
        }


        posFoot = transform.position;
        posFoot.y += footPosY;

    }

    void LateUpdate()
    {

        //animacoes
        steticChar();
        //
    }

    //estado carro
    void carMode(){
        transform.position = detectObjects.car.transform.position;
        if (Input.GetKeyUp(KeyCode.E))
        {
            //rouba o carro
            // transform.position = detectObjects.car.transform.position;
            playerState = PlayerState.PLAYER_MODE;
            
            var positionToGo = transform.position;
            positionToGo.y = positionToGo.y + 5;
            transform.position = positionToGo;


            collider.isTrigger = false;
            r.useGravity = true;
            detectObjects.car.GetComponent<WheelVehicle>().IsPlayer = false;
            
        }
    }
    void playerMode(){
        
        verificaEncostandoNoChao();
        
        //entrar veiculo
        if (Input.GetKeyUp(KeyCode.E))
        {
            enterCar();
        }
        
    }
    
    void movement(){
        r.velocity = new Vector3(
            Input.GetAxis("Horizontal") * speed * Time.deltaTime,
            r.velocity.y,
             Input.GetAxis("Vertical") * speed * Time.deltaTime);
        
    }
    
    void steticChar(){
        if (playerState == PlayerState.PLAYER_MODE)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                widle = false;
                animation.Play("walk 1");
                audioSource.enabled = true;

            }
            else
            {
                widle = true;
                animator.SetBool("widle", widle);
                animation.CrossFade("widle");
                audioSource.enabled = false;
            }
            rotateChar();
        }
        else {
            audioSource.enabled = false;
        }

        
    }

    void verificaEncostandoNoChao(){
        grounded = isGrounded();

       // if(grounded)
      //      r.useGravity = false;
      //  else
        //    r.useGravity = true;
    }

    //entra no carro
    void enterCar(){
         if(detectObjects.car != null){//verifica carro proximo
            
                //rouba o carro
                playerState = PlayerState.CAR_MODE;
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

    bool isGrounded(){

        var hitColliders = Physics.OverlapSphere(posFoot, footRadius);
        if(hitColliders.Length > 0)
        {
            foreach (var col in hitColliders) {
                if (col.gameObject.tag != "Player") {
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


}
