using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool beamChange = false;

    public GameObject sideCorer;
    public GameObject sideCorel;
    public GameObject frontCorer;
    public GameObject frontCorel;
    Color beamColorFr = Color.red;
    Color beamColorFl = Color.red;
    Color beamColorR = Color.red;
    Color beamColorL = Color.red;
    public bool rfrontHit = false;
    public bool lfrontHit = false;
    public bool rightHit = false;
    public bool leftHit = false;

    private bool rfrontHitP = false;
    private bool lfrontHitP = false;
    private bool rightHitP = false;
    private bool leftHitP = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 frPosition = frontCorer.transform.position;
        Vector3 flPosition = frontCorel.transform.position;
        Vector3 sPositionr = sideCorer.transform.position;
        Vector3 sPositionl = sideCorel.transform.position;
        Vector3 frrayForward = frontCorer.transform.forward;
        Vector3 flrayForward = frontCorel.transform.forward;
        Vector3 rayRight = sideCorer.transform.right;
        Vector3 rayLeft = -sideCorel.transform.right;
        int rayLengthFront = 15;
        int rayLengthSide = 7;
        RaycastHit hitInfo;

        rfrontHitP = rfrontHit;
        lfrontHitP = lfrontHit;
        rightHitP = rightHit;
        leftHitP = leftHit;

        //
        // Front facing beam
        //
        
        Vector3 forwardr = frontCorer.transform.TransformDirection(Vector3.forward) * rayLengthFront; //these two lines draw the visible line
        Debug.DrawRay(frPosition, forwardr, beamColorFr); //cont

        if (Physics.Raycast(frPosition, frrayForward, out hitInfo, rayLengthFront)) //this is testing the actual ray collider
        {
            if (hitInfo.collider.gameObject.tag == "Wall") //Checkingt if the collider hits a wall
            {
                beamColorFr = Color.green; //chages the color to green when it hits a wall for the fun visual effect
                rfrontHit = true;  // This is the output seen in the inspector
            }    
        }
        else
        {
            beamColorFr = Color.red; // these two lines return the beam to normal
            rfrontHit = false;
        }
        
        Vector3 forwardl = frontCorel.transform.TransformDirection(Vector3.forward) * rayLengthFront; //these two lines draw the visible line
        Debug.DrawRay(flPosition, forwardl, beamColorFl); //cont

        if (Physics.Raycast(flPosition, flrayForward, out hitInfo, rayLengthFront)) //this is testing the actual ray collider
        {
            if (hitInfo.collider.gameObject.tag == "Wall") //Checkingt if the collider hits a wall
            {
                beamColorFl = Color.green; //chages the color to green when it hits a wall for the fun visual effect
                lfrontHit = true;  // This is the output seen in the inspector
            }
        }
        else
        {
            beamColorFl = Color.red; // these two lines return the beam to normal
            lfrontHit = false;
        }
        

        //
        // Right facing beam
        //
        Vector3 right = sideCorer.transform.TransformDirection(Vector3.right) * rayLengthSide;
        Debug.DrawRay(sPositionr, right, beamColorR);

        if (Physics.Raycast(sPositionr, rayRight, out hitInfo, rayLengthSide))
        {
            if (hitInfo.collider.gameObject.tag == "Wall")
            {
                beamColorR = Color.green;
                rightHit = true;
            }
        }
        else
        {
            beamColorR = Color.red;
            rightHit = false;
        }

        //
        // Left facing beam
        //
        Vector3 left = sideCorel.transform.TransformDirection(Vector3.left) * rayLengthSide;
        Debug.DrawRay(sPositionl, left, beamColorL);

        if (Physics.Raycast(sPositionl, rayLeft, out hitInfo, rayLengthSide))
        {
            if (hitInfo.collider.gameObject.tag == "Wall")
            {
                beamColorL = Color.green;
                leftHit = true;
            }
        }
        else
        {
            beamColorL = Color.red;
            leftHit = false;
        }

        if (rfrontHit != rfrontHitP || lfrontHit != lfrontHitP || rightHit != rightHitP || leftHit != leftHitP)
            beamChange = true;
    }
}
