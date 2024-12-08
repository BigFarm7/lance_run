using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class RecreateRoad : MonoBehaviour
{
    public GameObject[] curveRoad;

    public GameObject firstRoad;
    public GameObject secondRoad;
    public GameObject thirdRoad;
    public CarManager carManager;
    public Vector3 angle;

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Road")
        {
            if (thirdRoad != null)
            {
                Destroy(thirdRoad);
                thirdRoad = secondRoad;
                secondRoad = firstRoad;
                firstRoad = other.gameObject;

            }
            else if (secondRoad != null)
            {
                thirdRoad = secondRoad;
                secondRoad = firstRoad;
                firstRoad = other.gameObject;
            }
            else if(firstRoad != null)
            {
                secondRoad = firstRoad;
                firstRoad = other.gameObject;
            }

            angle = carManager.forwardVec;

            int rand = Random.Range(0, curveRoad.Length);
            firstRoad = Instantiate(curveRoad[rand], other.transform.position + angle * 1.5f, Quaternion.LookRotation(angle));
            
            
            

        }
    }

}
