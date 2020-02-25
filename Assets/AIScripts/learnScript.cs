using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class learnScript : MonoBehaviour
{
    public int speciminNum;

    string learnFile = @"C:\Users\Home\3DTopDownRacer\Assets\AIScripts\LearnedData.txt";
    public string tempLearn;

    public GameObject CollisionDetect;
    public GameObject RayCastScript;

    private Vector3 originalPos;

    public bool eventDetected = false;

    public bool didFail = false;

    private float successClock = .3f;

    public float timer = 0;
    private float lastTime = 0;
    private float lastKeyAction = 0;
    public float speed;
    public float distance = 0f;
    public float maxDistance = 0f;
    public Vector3 lastPosition;

    public bool rfrontBeam = false;
    public bool lfrontBeam = false;
    public bool rightBeam = false;
    public bool leftBeam = false;
    private bool[] beamArray = { false, false, false, false };
    private bool[] lastBeamArray = { false, false, false, false };

    // array represents [straight, turnR, turnL]
    private float[] successCounter = { 0f, 0f, 0f };
    private float[] failureCounter = { 0f, 0f, 0f };
    private float[] dataSize = { 0f, 0f, 0f };
    private float[] successRate = { 0f, 0f, 0f };
    private float[] failureRate = { 0f, 0f, 0f };
    private float[] netSuccess = { 0f, 0f, 0f };

    private enum action
    {
        none,
        straight,
        turnR,
        turnL,
    }
    action currentAction = action.straight;
    action lastAction = action.none;
    public int actionInt = 0;

    // Start is called before the first frame update
    void Start()
    {
        tempLearn = @"C:\Users\Home\3DTopDownRacer\Assets\AIScripts\tempLearn" + speciminNum + ".txt";

        originalPos = gameObject.transform.position;

        // grabs the max distance
        string[] learnLines = System.IO.File.ReadAllLines(tempLearn);
        maxDistance = float.Parse(learnLines[0].Split(' ')[0]);

        // clears the file
        System.IO.StreamWriter strm = System.IO.File.CreateText(tempLearn);
        strm.Flush();
        strm.Close();

        // puts max distance back in the file
        using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(tempLearn, true))
        {
            file.WriteLine(maxDistance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //
        // Some basic info for the AI to know to help learn
        //
        if(timer < .5)
            distance = 0;
        timer += Time.deltaTime;
        speed = gameObject.GetComponent<CarController>().CurrentSpeed;
        distance += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        
        // End Basic info


        //begin learning
        
        // checks if collision with wall
        if (CollisionDetect.GetComponent<collisionScript>().collide && didFail == false)
        {
            // log failure
            if (currentAction != action.none)
            {
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(tempLearn, true))
                {
                    if (timer - lastKeyAction > successClock)
                    {
                        file.WriteLine(beamArray[0] + " " + beamArray[1] + " " + beamArray[2] + " " + beamArray[3] + " " + stringAction(currentAction) + " False");
                        Debug.Log(stringAction(lastAction) + " Failure Logged");             
                    }
                }
            }
            didFail = true;
            RayCastScript.GetComponent<RayCastScript>().beamChange = false;
        }
        // checking for a learnable event


        // if the beamArray state changes
        else if (RayCastScript.GetComponent<RayCastScript>().beamChange && didFail == false)
        {
            //
            // events stored as followed:
            // Beam States: 
            // frnt: rt:   lft:  act:  result:
            // false false false turnR false

            // if beamArray state already exists in text document:
            // if majority of beamArray that state resulted in death (success = false):
            // try different action
            // log result
            // else if majority of state resulted in success:
            // try same action
            // log result
            // else if doesn't exist in text document:
            // try random action


            // log success
            if (currentAction != action.none)
            {
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(tempLearn, true))
                {
                    if (timer - lastKeyAction > successClock)
                    {
                        file.WriteLine(beamArray[0] + " " + beamArray[1] + " " + beamArray[2] + " " + beamArray[3] + " " + stringAction(currentAction) + " True");
                        Debug.Log(stringAction(currentAction) + " Success Logged");
                    }
                }
            }

            lastAction = currentAction;
            lastBeamArray = beamArray;

            // Grab which rays are triggered
            rfrontBeam = RayCastScript.GetComponent<RayCastScript>().rfrontHit;
            lfrontBeam = RayCastScript.GetComponent<RayCastScript>().lfrontHit;
            rightBeam = RayCastScript.GetComponent<RayCastScript>().rightHit;
            leftBeam = RayCastScript.GetComponent<RayCastScript>().leftHit;
            beamArray[0] = rfrontBeam;
            beamArray[1] = lfrontBeam;
            beamArray[2] = rightBeam;
            beamArray[3] = leftBeam;

            successCounter[0] = 0f;
            successCounter[1] = 0f;
            successCounter[2] = 0f;

            failureCounter[0] = 0f;
            failureCounter[1] = 0f;
            failureCounter[2] = 0f;
            string[] eventString = System.IO.File.ReadAllLines(learnFile);
            foreach (string line in eventString)
            {
                string[] eventData = line.Split(' ');
                // Debug.Log(eventData[0] + " " + eventData[1] + " " + eventData[2] + " " + eventData[3] + " " + eventData[4]);

                // if beamArray state already exists in text document:
                if ((beamArray[0] == boolString(eventData[0])) && (beamArray[1] == boolString(eventData[1])) && (beamArray[2] == boolString(eventData[2])) && (beamArray[3] == boolString(eventData[3])))
                {
                    // Add counters for successes and failure
                    if (eventData[5] == stringBool(true))
                    {
                        successCounter[decisionIncrement(actionify(eventData[4]))] += 1f;
                    }
                    else
                    {
                        failureCounter[decisionIncrement(actionify(eventData[4]))] += 1f;
                    }
                }
            }

            for (int i = 0; i <= 2; ++i)
            {
                dataSize[i] = successCounter[i] + failureCounter[i];
            }

            // get ratio of each straight, turnR, turnL for success and failure, then pick the best from each:
            for (int i = 0; i <= 2; ++i)
            {
                if (dataSize[i] != 0)
                {
                    //successRate[i] = (float)successCounter[i] / dataSize[i];
                    //failureRate[i] = (float)failureCounter[i] / dataSize[i];
                    //if (failureRate[i] != 0)
                    //    netSuccess[i] = (float)successRate[i] / failureRate[i];
                    //else
                    //    netSuccess[i] = 1f;

                    if (failureCounter[i] != 0)
                        netSuccess[i] = (float)successCounter[i] / failureCounter[i];
                    else
                        netSuccess[i] = successCounter[i];
                }
                else
                    netSuccess[i] = 1f;
            }
            // pick most successful choice choice

            // if they are all equally successful
            
            if (netSuccess[0] == netSuccess[1] && netSuccess[0] == netSuccess[2])
            {
                currentAction = randomAction(0,2);
            }
            // if stright has the highest success
            else if (netSuccess[0] > netSuccess[1] && netSuccess[0] > netSuccess[2])
            {
                currentAction = action.straight;
            }
            // if turnR has the highest success
            else if (netSuccess[1] > netSuccess[0] && netSuccess[1] > netSuccess[2])
            {
                currentAction = action.turnR;
            }
            // if turnL has the highest success
            else if (netSuccess[2] > netSuccess[1] && netSuccess[2] > netSuccess[0])
            {
                currentAction = action.turnL;
            }
            else if (netSuccess[0] == netSuccess[1])
            {
                currentAction = randomAction(0, 1);
            }
            else if (netSuccess[0] == netSuccess[2])
            {
                do
                {
                    currentAction = randomAction(0, 2);
                } while (currentAction != action.turnR);
            }
            else if (netSuccess[1] == netSuccess[2])
            {
                currentAction = randomAction(1, 2);
            }
            actionInt = decisionIncrement(currentAction);
            RayCastScript.GetComponent<RayCastScript>().beamChange = false;

            // counts the time since decision was made.
            lastKeyAction = timer;

            Debug.Log(beamArray[0] + "," + beamArray[1] + "," + beamArray[2] + "," + beamArray[3] + " :: Straight: " + (float)netSuccess[0] + " | Right: " + (float)netSuccess[1] + " | Left: " + (float)netSuccess[2] + " :: " + stringAction(currentAction));
        }

    }

    action randomAction(int a, int b)
    {
        int r = Random.Range(a, b + 1);
        if (r == 0)
            return action.straight;
        else if (r == 1)
            return action.turnR;
        else if (r == 2)
            return action.turnL;
        else
            return action.none;
    }

    int decisionIncrement(action a)
    {
        if (a == action.straight)
            return 0;
        else if (a == action.turnR)
            return 1;
        else if (a == action.turnL)
            return 2;
        else
            return -1;
    }

    string stringAction(action a)
    {
        if (a == action.straight)
            return "straight";
        else if (a == action.turnR)
            return "turnR";
        else if (a == action.turnL)
            return "turnL";
        else
            return "none";
    }

    action actionify(string a)
    {
        if (a == "straight")
            return action.straight;
        else if (a == "turnR")
            return action.turnR;
        else if (a == "turnL")
            return action.turnL;
        else
            return action.none;
    }

    string stringBool(bool b)
    {
        if (b)
            return "True";
        else
            return "False";
    }

    bool boolString(string s)
    {
        if (s == "True")
            return true;
        else
            return false;
    }
}
