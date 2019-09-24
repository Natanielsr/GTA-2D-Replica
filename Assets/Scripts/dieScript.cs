using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieScript : MonoBehaviour
{
    public GameObject graphics;
    public GameObject ragdoll;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void Die() {
        graphics.active = false;
        ragdoll.active = true;
    }

}
