using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }

        // Negative steering turns left, positive turns right. (-1 to 1)
        public float steering;
        // Negative acceleration is reverse, positive is forward (-1 to 1)
        public float accel;
        // Don't worry about this
        public float handbrake;
        private void FixedUpdate()
        {
            m_Car.Move(steering, accel, accel, handbrake);
        }
    }
}
