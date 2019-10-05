using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VehicleBehaviour;

public class CitizenBehaviour : CharacterBase
{
    public GameObject Graphics;
    public GameObject Ragdoll;
    
    public Transform RandomPositionToGo;

    NavMeshAgent navMesh;
   
    public float PosYCalibrate;

    public float TimeToDestroy = 60;

    private float TimeToThink;

    public GameObject Suit;

    public float RunZ;

    public FieldOfView fieldOfView;

    GameObject car;

    private void Awake()
    {
        animator.SetBool("Grounded", true);
    }
    // Start is called before the first frame update
    protected override void _start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        randPosition();

        

        TimeToThink = Random.Range(0, 10);
        navMesh.speed = Random.Range(2.0f, 5.0f);
        

        //Get the Renderer component from the new cube
        var renderer = Suit.GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        var r = Random.Range(0.0f, 1.0f);
        var g = Random.Range(0.0f, 1.0f);
        var b = Random.Range(0.0f, 1.0f);
        var color = new Color(r, g, b);
       
        renderer.material.SetColor("_Color", color);
    }

    void randPosition() {

        var pavs = GameObject.FindGameObjectsWithTag("pavment");

        var rand = Random.Range(0, pavs.Length);

        RandomPositionToGo = pavs[rand].transform;
        
        navMesh.SetDestination(RandomPositionToGo.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    protected override void _update()
    {
        if (CharState == CharacterState.ALIVE)
        {
            if (fieldOfView.FindVisibleTargetByTag("Player") != null)
            {
                Debug.Log("> ..." + this.gameObject.name + " view the player ...");
                
            }
            else {
              //  Debug.Log("...");
            }

            if (CharMode.Equals(CharacterMode.WALKING_MODE)){
                car = fieldOfView.FindVisibleTargetByTag("car");

                if (car != null)
                {
                    // viu o carro
                    var carScript = car.GetComponent<WheelVehicle>();
                    if (carScript.GetCarOwner() == null)
                    {
                        Debug.Log(carScript.GetCarOwner());
                        //carro nao tem dono
                        navMesh.speed = 10f;
                        navMesh.SetDestination(car.transform.position);
                        if (detectObjects.carNearby != null)
                        {
                            enterCar();
                        }
                    }
                    else
                    {
                        Debug.Log(carScript.GetCarOwner());
                        navMesh.speed = 5f;
                        //carro tem dono
                        // car = fieldOfView.FindVisibleTargetByTag("car");
                        // navMesh.isStopped = true;
                        
                        walkRandomPosition();
                    }

                }
                else {
                   
                   
                    walkRandomPosition();
                }
                
            }

            Debug.ClearDeveloperConsole();

            
        }
        else if (CharState == CharacterState.DEAD)
        {
            TimeToDestroy -= Time.deltaTime;
            if (TimeToDestroy < 0)
            {
                Destroy(this.gameObject);
            }
        }

        RunZ = animator.GetFloat("RunZ");
    }

    void walkRandomPosition() {
        navMesh.SetDestination(RandomPositionToGo.position);
        var d = Vector3.Distance(RandomPositionToGo.position, transform.position);
        
        // Debug.Log(d);
        if (d < 3)
        {
            TimeToThink -= Time.deltaTime;
            if (TimeToThink < 0)
            {
                TimeToThink = Random.Range(1.0f, 3.0f);
                navMesh.speed = Random.Range(2.0f, 5.0f);
                randPosition();
            }
        }
    }

    void LateUpdate()
    {
        if (CharState == CharacterState.ALIVE)
        {
            var animationVelocity = Vector3.Magnitude(navMesh.velocity) * 0.1f;
          //  Debug.Log(animationVelocity);
            animator.SetFloat("RunZ", animationVelocity);
            var posZero = Vector3.zero;
            posZero.y = PosYCalibrate;
            Graphics.transform.localPosition = posZero;
            Graphics.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }



    protected override void base_die() {

        Graphics.SetActive(false);
        Ragdoll.SetActive(true);
        navMesh.enabled = false;

        var cs = FindObjectOfType<CitizenSpawner>();
        cs.removeCitizen(this.gameObject);


        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Color newColor = new Color(1f, 1f, 1f, 1f);

        Gizmos.color = newColor;
        if(RandomPositionToGo != null)
            Gizmos.DrawSphere(RandomPositionToGo.position, 1);
    }

}
