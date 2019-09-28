using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CitizenBehaviour : CharacterBase
{
    public GameObject Graphics;
    public GameObject Ragdoll;
    
    public Transform PositionToGo;

    NavMeshAgent navMesh;
   
    public float PosYCalibrate;

    public float TimeToDestroy = 60;

    private float TimeToThink;

    public GameObject Suit;

    // Start is called before the first frame update
    protected override void _start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        randPosition();

        animator.SetBool("Grounded", true);

        TimeToThink = Random.Range(0, 10);
        navMesh.speed = Random.Range(4, 5);
        

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

        PositionToGo = pavs[rand].transform;

        if (CharState == CharacterState.ALIVE)
        {
            navMesh.SetDestination(PositionToGo.position);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    protected override void _update()
    {
        if (CharState == CharacterState.ALIVE)
        {
            var d = Vector3.Distance(PositionToGo.position, transform.position);
            // Debug.Log(d);
            if (d < 3)
            {
                TimeToThink -= Time.deltaTime;
                if (TimeToThink < 0)
                {
                    TimeToThink = Random.Range(0, 10);
                    navMesh.speed = Random.Range(4, 5);
                    randPosition();
                }
            }
        }
        else if (CharState == CharacterState.DEAD)
        {
            TimeToDestroy -= Time.deltaTime;
            if (TimeToDestroy < 0)
            {
                Destroy(this.gameObject);
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
        if(PositionToGo != null)
            Gizmos.DrawSphere(PositionToGo.position, 1);
    }

}
