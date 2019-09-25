using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    NavMeshSurface navMesh;
    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        //navMesh.BuildNavMesh();
    }
}
