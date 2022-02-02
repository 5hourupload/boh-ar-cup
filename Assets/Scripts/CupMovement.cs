using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupMovement : MonoBehaviour
{

    bool mugInitialized = false;
    GameObject leftHand = null;

    bool pointerInitialized = false;
    GameObject rightHand = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        if (!mugInitialized)
        {
            leftHand = GameObject.Find("PokePointer(Clone)");
            if (leftHand != null)
            {
                mugInitialized = true;
                leftHand.name = "LEFTLEFTELEF";
            }

        }
        else
        {
            transform.position = leftHand.transform.position;
        }


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



       
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("yeerere");
    }
}
