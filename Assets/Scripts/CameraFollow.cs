using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    PlayerController playerController;
    public float MinPosYCam = 40;
    public float MaxPosYCam = 60;
    float StartMaxPosYCam ;

    public bool movingCamera = false;

    Vector3 startMarker;
    Vector3 endMarker;

    public float speed = 1.0f;

    float startTime;
    float journeyLength;
    
    bool subindoACamera = false;
    

    // Start is called before the first frame update
    void Start()
    {
        StartMaxPosYCam = MaxPosYCam;
        startMarker = transform.position;
        endMarker.y = MinPosYCam;

        playerController = player.GetComponent<PlayerController>();

        StartMoveCamera();
    }

    private void Update()
    {
        
        if (playerController.CharMode == CharacterMode.WALKING_MODE)
        {
            startMarker.y = transform.position.y;
            endMarker.y = MinPosYCam;
            return;
        }


        if (playerController.GetCar().Speed > 25 && Input.GetAxis("Vertical") > 0)
        {
            if (!subindoACamera)
            { //verifica se nao estava apertando o botao no frame anterior

                startMarker.y = transform.position.y;
                endMarker.y = MaxPosYCam;
                speed = 4.0f;
                StartMoveCamera();

            }

            subindoACamera = true; //define como apertando para o proximo frame verificar
            
        }
        else
        {

            if (subindoACamera)
            {//verifica se estava apertando o botao no frame anterior
                startMarker.y = transform.position.y;
                endMarker.y = MinPosYCam;
                speed = 8.0f;
                StartMoveCamera();
            }

            subindoACamera = false;//define como nao apertando para o proximo frame verificar
        }
            

        
    }

    // Update is called once per frame
    void LateUpdate()
    {


        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        Vector3 posToGo = player.position;
        var y = Vector3.Lerp(startMarker, endMarker, fractionOfJourney).y;
        Debug.Log(y);
        posToGo.y = y.ToString() != "NaN" ? y : MinPosYCam;
        this.transform.position = posToGo;
    }

    void StartMoveCamera() {

        startTime = Time.time;

        journeyLength = Vector3.Distance(startMarker, endMarker);

    }
}
