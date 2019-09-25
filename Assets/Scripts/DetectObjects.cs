using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjects : MonoBehaviour
{
    public GameObject carNearly;
    public float MaxDistanceToCar = 10;
    float distanceToCar;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public GameObject detectCarNearly() {
        var cars = DetectObjectsNearly("car");
        //  Debug.Log(carNearly);
        if (cars.Length > 0)
        {//verifica carro proximo
            float moreNearly = MaxDistanceToCar;
            foreach (var car in cars)
            {
                distanceToCar = Vector3.Distance(car.transform.position, transform.position);
                if (distanceToCar < moreNearly)
                {
                    carNearly = car;
                    moreNearly = distanceToCar;
                }
            }

            Debug.DrawRay(
                      transform.position,
                      carNearly.transform.position - transform.position,
                      Color.red);

        }
        else
        {
            carNearly = null;
        }


        return carNearly;
    }

    GameObject DetectObject(string tag) {
        var objs = DetectObjectsNearly(tag);
        foreach (var obj in objs) {
            if (obj.tag == tag) {
                return obj;
            }
        }

        return null;
    }

    GameObject[] DetectObjectsNearly(string tag)
    {
        var hitColliders = Physics.OverlapSphere(transform.position, MaxDistanceToCar/2);
        var objects = new List<GameObject>();
        if (hitColliders.Length > 0)
        {
            var i = 0;
            foreach (var col in hitColliders)
            {
                var go = col.gameObject;
                if(go.tag == tag)
                    objects.Add(go);
                i++;
            }
        }

        return objects.ToArray();
    }

    void OnDrawGizmosSelected()
    {
        
        // Draw a yellow sphere at the transform's position
        Color newColor = new Color(1f, 1f, 1f, 0.5f);

        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, MaxDistanceToCar/2);
    }
}
