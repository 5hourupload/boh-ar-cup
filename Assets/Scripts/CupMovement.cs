using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using UnityEngine.UI;
using TMPro;



public class CupMovement : MonoBehaviour
{
    public GameObject sliderValue;

    bool mugInitialized = false;

    GameObject leftHandMiddleMiddle = null;
    GameObject leftHandWrist = null;

    bool pointerInitialized = false;
    GameObject rightHand = null;



    public int pulseWidth = 400; //0-400
    public int frequency = 100; //0-100

    //The strengths correspond to the channels in the arduino code, NOT the order of elements in the rightHand[] array. Ex. Element 0 == Channel 1 == 'a' 
    public int strength = 40; //0-255


    private string serialport = "COM19";

    bool colliding = false;

    SerialPort stream;


    bool leftHandOnline = false;


    // Start is called before the first frame update
    void Start()
    {

        try
        {
            stream = new SerialPort(serialport, 115200);
            string[] ports = SerialPort.GetPortNames();

            Debug.Log("# Ports: " + ports.Length);
            for (int i = 0; i < ports.Length; i++)
            {
                Debug.Log(ports[i]);
            }

            stream.ReadTimeout = 50;
            stream.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            stream.Open();
        }
        catch (Exception e)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

        float f = float.Parse(sliderValue.GetComponent<TextMeshPro>().text);
        frequency = (int) f;


        if (!leftHandOnline)
        {
            GameObject go = GameObject.Find("Left_HandLeft(Clone)");
            if (go != null)
            {
                leftHandMiddleMiddle = go.transform.Find("PinkyMiddleJoint Proxy Transform").gameObject;
                leftHandWrist = go.transform.Find("Wrist Proxy Transform").gameObject;
                leftHandOnline = true;
                Debug.Log("online!");
            }
        }
        else
        {
            if (leftHandMiddleMiddle == null)
            {
                leftHandOnline = false;
                Debug.Log("offline!");
            }
            else
            {
                Vector3 p1 = leftHandWrist.transform.position;
                Vector3 p2 = leftHandMiddleMiddle.transform.position;

                float s = 2f;
                transform.position = leftHandWrist.transform.position + new Vector3((p2.x-p1.x)*s, (p2.y - p1.y) * s, (p2.z - p1.z) * s);
                transform.rotation = leftHandWrist.transform.rotation * Quaternion.Euler(0,0,75);
            }
        }
        //if (!mugInitialized)
        //{
        //    leftHandPointer = GameObject.Find("PokePointer(Clone)");
        //    if (leftHandPointer != null)
        //    {
        //        mugInitialized = true;
        //        leftHandPointer.name = "LEFT!";
        //    }
        //    leftHandWrist = GameObject.Find("Wrist Proxy Transform");
        //    if (leftHandWrist != null)
        //    {
        //        mugInitialized = true;
        //        leftHandWrist.name = "LEFT WRIST!";
        //        Debug.Log("success");
        //    }
            

        //}
        //else
        //{
        //    transform.position = leftHandPointer.transform.position;
        //}


        if (!pointerInitialized && mugInitialized)
        {

            rightHand = GameObject.Find("PokePointer(Clone)");
            if (rightHand != null)
            {
                pointerInitialized = true;
                Debug.Log("All initialized");
                //rightHand.AddComponent(typeof(CapsuleCollider)); 
                
                rightHand.AddComponent<Rigidbody>();



                BoxCollider bc = rightHand.AddComponent<BoxCollider>();
                bc.size = new Vector3(.01f, .01f, .01f);


            }
        }




        String message1 = pulseWidth + "," + frequency + ",";
        String message2 = "0,0," + strength + ",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,";

        String message3 = "";

       if (colliding)
        {
            message3 = "0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
            message1 = message1 + "1,";

        }
        else
        {
            message3 = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
            message1 = message1 + "0,";


        }

        String wholeMessage = message1 + message2 + message3;
        WriteToSerial(wholeMessage);
        //Debug.Log(wholeMessage);

    }

    void OnCollisionEnter(Collision collision)
    {
        colliding = true;    
    }

    void OnCollisionExit(Collision collision)
    {
        colliding = false;    
    }
    public void WriteToSerial(string message)
    {
        if (!stream.IsOpen) return;
        stream.WriteLine(message);
        stream.BaseStream.Flush();

    }

    private static void DataReceivedHandler(
                    object sender,
                    SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string indata = sp.ReadExisting();
        Debug.Log("Data Received:");
        Debug.Log(indata);
    }

}
