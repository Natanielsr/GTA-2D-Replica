using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjects : MonoBehaviour
{
    public GameObject car;
    public float MaxDistanceToCar = 10;
    float distanceToCar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(car != null){//verifica carro proximo
            distanceToCar = Vector3.Distance(car.transform.position, transform.position);
            if(distanceToCar > MaxDistanceToCar)
                car = null;
            else
            {
               // Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
                Debug.DrawRay(
                    transform.position,
                    car.transform.position - transform.position,
                    Color.red);
            }
        }

    }

     void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(collider);
        if(collider.gameObject.tag == "car"){
            //Debug.Log("car");
            car = collider.gameObject;
        }
    }
}
