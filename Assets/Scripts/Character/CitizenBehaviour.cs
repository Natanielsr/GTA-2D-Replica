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


    // Start is called before the first frame update
    protected override void _start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    protected override void _update()
    {
        if (CharState == CharacterState.ALIVE)
        {
            navMesh.SetDestination(PositionToGo.position);
        }
    }

    void LateUpdate()
    {
        if (CharState == CharacterState.ALIVE)
        {
            var animationVelocity = Vector3.Magnitude(Vector3.Normalize(navMesh.velocity)) / 2;
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
        
    }

}
