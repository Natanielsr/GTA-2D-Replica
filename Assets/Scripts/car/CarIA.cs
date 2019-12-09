
using System;
using UnityEngine;
using VehicleBehaviour;

public class CarIA : MonoBehaviour
{

    private Transform carTransform;
    Transform transformToGo;
    CitizenBehaviour citizen;
    WheelVehicle wheelVehicle;

    private float MaxSteerAngle = 50;


    [Range(-1.0f, 1.0f)]
    public float Vertical;

    [Range(-1.0f, 1.0f)]
    public float Horizontal;

    bool start = true;
    private void Start()
    {
        
        citizen = GetComponent<CitizenBehaviour>();
    }
    public float ApplySteer() {
        

        if (transformToGo == null)
        {
            Horizontal = 0.0f;
            return Horizontal;
        }

        carTransform = this.transform;


        Vector3 relativeVector = carTransform.InverseTransformPoint(transformToGo.position);
        Vector3 result = relativeVector / relativeVector.magnitude;

        float newSteer = (relativeVector.x / relativeVector.magnitude) * MaxSteerAngle;

        Horizontal = convertAngleToInput(newSteer);

        return Horizontal;

    }

    private void FixedUpdate()
    {
        if (citizen.CharMode == CharacterMode.CAR_MODE)
        {
            
            GetFirstPathPoint();
            
            float steer = ApplySteer();
            Drive(steer);

            changePointTransform();
        }
    }
    private void Drive(float steer){
        if (transformToGo != null)
        {
            var positiveNumber = System.Math.Abs(steer);

            var v = (1.0f - positiveNumber);

            Vertical = v > 0.5f ? 0.5f : v;

            print(citizen.car.Speed);

        }
        else {
            Vertical = 0.0f;
        }
    }

    private void GetFirstPathPoint() {
        if (start)
        {
            if (citizen.CarGuide != null)
            {
                transformToGo = citizen.CarGuide;
                start = false;
            }
        }
    }


    private void changePointTransform() {
        if (transformToGo == null)
            return;

        var distance = Vector3.Distance(this.transform.position, transformToGo.position);
       // print(distance);
        if (distance < 7)
        {
            
            var path = transformToGo.GetComponent<CarPath>();
            if (path.NextPath != null)
            {
                transformToGo = path.NextPath.transform;
                //print(transformToGo);
            }
            else
            {
                transformToGo = null;
                throw new NullReferenceException("Path not find");

            }
        }
    }

    private float convertAngleToInput(float angle) {
        return angle / MaxSteerAngle;
    }


    public float GetInput(string input)
    {
        if (input == InputNames.throttleInput)
        {
            return Vertical;
        }
        else if (input == InputNames.turnInput)
        {
            return Horizontal;
        }

        return 0;
    }
}
