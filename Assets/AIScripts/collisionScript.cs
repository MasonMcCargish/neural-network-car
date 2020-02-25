using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public GameObject car;
    public bool collide = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 myPosition = car.transform.position;
        Vector3 rayForward = car.transform.forward;
        Vector3 rayRight = car.transform.right;
        //Vector3 rayLeft = -rayRight;
        float rayLengthSide = 2.0f; // These are the lengths of
        float rayLengthFront = 2.0f;     // the collision rays, please don't modify
        RaycastHit hitInfo;


        // Front facing beam
        if (Physics.Raycast(myPosition, rayForward, out hitInfo, rayLengthFront))
        {
            if (hitInfo.collider.gameObject.tag == "Wall") // Checks for any object with the Wall tag
            {
                collide = true;
            }
        }

        // Right facing beam
        if (Physics.Raycast(myPosition, rayRight, out hitInfo, rayLengthSide))
        {
            if (hitInfo.collider.gameObject.tag == "Wall")
            {
                collide = true;
            }
        }

        /*
        // Left facing beam
        if (Physics.Raycast(myPosition, rayLeft, out hitInfo, rayLengthSide))
        {
            if (hitInfo.collider.gameObject.tag == "Wall")
            {
                collide = true;
            }
        }
        */
    }
}
