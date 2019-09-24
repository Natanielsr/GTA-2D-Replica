using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterState {
    ALIVE,
    DEAD
}
public class CitizenBehaviour : MonoBehaviour
{
    public GameObject graphics;
    public GameObject ragdoll;

    public CharacterState charState;

    public Transform positionToGo;

    NavMeshAgent navMesh;
    

    
    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        charState = CharacterState.ALIVE;

        if (charState == CharacterState.ALIVE)
            navMesh.SetDestination(positionToGo.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        graphics.transform.localPosition = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void Die() {

        charState = CharacterState.DEAD;
        graphics.SetActive(false);
        ragdoll.SetActive(true);
        navMesh.Stop();

    }

}
