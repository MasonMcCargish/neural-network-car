using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class driver : MonoBehaviour
{
    //Vector3 startpoint = new Vector3(0.85f, -4.66f, 0.8f);
    Vector3 startpoint = new Vector3(-131.0f, -4.66f, 0.8f);


    string learnFile = @"C:\Users\Home\3DTopDownRacer\Assets\AIScripts\LearnedData.txt";

    public GameObject car;

    public int failureCounter = 0;
    public bool[] failureArray = new bool[generationSize] {false, false, false, false, false};

    const int generationSize = 5;
    public GameObject[] carArray = new GameObject [generationSize];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < generationSize; ++i)
        {
            carArray[i] = Instantiate(car, startpoint, Quaternion.identity);
            carArray[i].GetComponent<learnScript>().speciminNum = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < generationSize; ++i)
        {
            if (carArray[i].GetComponent<learnScript>().didFail == true)
            {
                carArray[i].SetActive(false);
                failureArray[i] = true;
            }
        }

        failureCounter = 0;
        foreach (bool f in failureArray)
        {
            if (f == true)
                ++failureCounter;
        }

        if (failureCounter == generationSize)
        {
            // figure out the best of the generation by distance
            int bestOne = 0;
            float bestDist = 0;
            for (int i = 0; i < generationSize; ++i)
            {
                if (carArray[i].GetComponent<learnScript>().distance > bestDist)
                {
                    bestDist = carArray[i].GetComponent<learnScript>().distance;
                    bestOne = i;
                }
            }

            // writes data from the best one in the gen to the learnedData file
            string[] toLearn = System.IO.File.ReadAllLines(carArray[bestOne].GetComponent<learnScript>().tempLearn);
            bool notFirst = false;
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(learnFile, true))
            {
                foreach (string line in toLearn)
                {
                    if (notFirst)
                    {
                        file.WriteLine(line);
                    }
                    else
                        notFirst = true;
                }

            }

            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
