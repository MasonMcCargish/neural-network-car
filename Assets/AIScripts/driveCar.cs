using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class driveCar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CarUserControl>().accel = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(GetComponent<learnScript>().actionInt == 0)
        {
            GetComponent<CarUserControl>().steering = 0;
        }
        else if (GetComponent<learnScript>().actionInt == 1)
        {
            GetComponent<CarUserControl>().steering = 0.6f;
        }
        else if (GetComponent<learnScript>().actionInt == 2)
        {
            GetComponent<CarUserControl>().steering = -0.6f;
        }

    }
}
