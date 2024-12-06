using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskObject : MonoBehaviour
{
    public GameObject[] ObjMasked;
    void Start()
    {

        StartCoroutine(Masking());
       
    }


    IEnumerator Masking()
    {
        yield return new WaitForSeconds(0.2f);

        if (transform.tag == "Bigcol")
        {
            transform.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = 3005;
            for (int i = 0; i < ObjMasked.Length; i++)
            {
                ObjMasked[i].GetComponent<MeshRenderer>().sharedMaterial.renderQueue = 3007;
            }

        }
        else
        {
            for (int i = 0; i < ObjMasked.Length; i++)
            {
                ObjMasked[i].GetComponent<MeshRenderer>().sharedMaterial.renderQueue = 3002;
            }
        }
    }
}

