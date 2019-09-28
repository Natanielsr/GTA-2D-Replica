using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSpawner : MonoBehaviour
{
    public GameObject prefabCitizen;
    public int numberOfCitizen = 10;
    GameObject[] pavs;

    public List<GameObject> citizens;
    // Start is called before the first frame update
    void Start()
    {
        pavs = GameObject.FindGameObjectsWithTag("pavment");

        for (int i = 0; i < numberOfCitizen; i++)
        {
            citizens.Add(spawn());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void removeCitizen(GameObject citizen) {
        citizens.Remove(citizen);
    }

    public void addCitizen()
    {
        citizens.Add(spawn());
    }

    GameObject spawn()
    {

        var rand = Random.Range(0, pavs.Length);

        var PositionToGo = pavs[rand].transform;

        return GameObject.Instantiate(prefabCitizen, PositionToGo.position, prefabCitizen.transform.rotation);
    }
}
