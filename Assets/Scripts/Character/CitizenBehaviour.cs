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
    CarIA carIa;
   
    public float PosYCalibrate;

    public float TimeToDestroy = 60;

    public float TimeToThink = 3.0f;
    private float timeToThink;

    public GameObject Suit;
    public GameObject SuitRagdoll;

    public float RunZ;

    public FieldOfView fieldOfView;
    Vector3 viewStartPosition;

   

    public bool VagabundoBandidoPilantra = false;

    bool walkRandom { get; set; }


    public Transform CarGuide;


    private void Awake()
    {
        animator.SetBool("Grounded", true);
    }
    // Start is called before the first frame update
    protected override void _start()
    {
        viewStartPosition = fieldOfView.gameObject.transform.position;
        navMesh = GetComponent<NavMeshAgent>();
        carIa = GetComponent<CarIA>();
        randPosition();

        

        timeToThink = RandThink();
        navMesh.speed = RandSpeed();
        

        //Get the Renderer component from the new cube
        var renderer = Suit.GetComponent<Renderer>();
        var rRag = SuitRagdoll.GetComponent<Renderer>();
        //Call SetColor using the shader property name "_Color" and setting the color to red
        var r = Random.Range(0.0f, 1.0f);
        var g = Random.Range(0.0f, 1.0f);
        var b = Random.Range(0.0f, 1.0f);
        var color = new Color(r, g, b);

       // renderer.material.shader = Shader.Find("Standard (Specular Setup)");
        renderer.material.SetColor("_SpecColor", color);
        rRag.material = renderer.material;
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
       
        if (CharState == CharacterState.DEAD)
        {
            TimeToDestroy -= Time.deltaTime;
            if (TimeToDestroy < 0)
            {
                Destroy(this.gameObject);
            }
        }

        

        RunZ = animator.GetFloat("RunZ");
    }

    protected override void WalkingMode()
    {
        base.WalkingMode();

        if (walkRandom && CharState == CharacterState.ALIVE)
        {
            walkRandomPosition();
        }
    }

    public void ReceiveViewObj(GameObject g) {
        //Debug.Log(g.tag);
        switch (CharState)
        {
            case CharacterState.ALIVE:
                switch (CharMode)
                {
                    case CharacterMode.NONE:
                        break;
                    case CharacterMode.WALKING_MODE:
                        if (g.tag == "car" && VagabundoBandidoPilantra)
                        {
                            var carScript = g.GetComponent<WheelVehicle>();
                            if (carScript.GetCarOwner() != null)
                            {
                                if (!walkRandom)
                                {
                                    navMesh.speed = RandSpeed();
                                    navMesh.SetDestination(RandomPositionToGo.position);
                                }
                                walkRandom = true;
                            }
                            else
                            {
                                //carro esta vazio
                                navMesh.speed = 10f;
                                navMesh.SetDestination(g.transform.position);
                                if (detectObjects.carNearby != null)
                                {
                                    //ia entra no carro
                                    Vector3 newPosition = fieldOfView.gameObject.transform.position;
                                    newPosition.y -= 3;
                                    var rot = fieldOfView.gameObject.transform.rotation;
                                    rot.y += 0.2f;

                                    fieldOfView.gameObject.transform.position = newPosition;
                                    fieldOfView.gameObject.transform.rotation = rot;
                                    navMesh.enabled = false;
                                    enterCar();
                                    car = carScript;
                                }

                                walkRandom = false;
                            }
                        }
                        else {
                            walkRandom = true;
                        }

                        break;
                    case CharacterMode.CAR_MODE:
                       // Debug.Log(g.tag);
                        if (g.tag == "carGuide")
                        {
                         //   print(g);
                            CarGuide = g.transform;
                        }

                        break;
                    default:
                        break;
                }
                break;
            case CharacterState.DEAD:
                break;
            default:
                break;
        }

        
    }

    float RandSpeed() {
        return Random.Range(2.0f, 5.0f);
    }

    float RandThink() {
        return Random.Range(1.0f, TimeToThink);
    }

    void walkRandomPosition() {
        
        var d = Vector3.Distance(RandomPositionToGo.position, transform.position);
       // Debug.Log(d);
        // Debug.Log(d);
        if (d < 3)
        {
            timeToThink -= Time.deltaTime;
            if (timeToThink < 0)
            {
                timeToThink = RandThink();
                navMesh.speed = RandSpeed();
                randPosition();
            }
        }
    }

    protected override void _lateUpdate()
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

    public override float GetInput(string input)
    {
        return carIa.GetInput(input);
    }
    
     
}
