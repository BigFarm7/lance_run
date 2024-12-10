using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RagdollActive : MonoBehaviour
{
    public Transform givePower;
    Transform Cam_Offset, mask3D, offset;
    Vector3 currentTrans, Cam_cur_offset;
    Vector3 dir;
    Rigidbody rb;

    private void OnEnable()
    {
        Cam_Offset = GameObject.Find("Camera Offset").transform;
        mask3D = GameObject.FindGameObjectWithTag("Mask").transform;
        currentTrans = transform.position;
        Cam_cur_offset = Cam_Offset.position;
        dir = GameObject.Find("XR Origin").GetComponent<CarManager>().direction;


        rb = givePower.GetComponent<Rigidbody>();
        givePower.GetComponent<Rigidbody>().AddForce(dir * 1000 + transform.up * 1500, ForceMode.Impulse);
    }

    private void Update()
    {
        Cam_Offset.transform.position = (transform.position - currentTrans) + Cam_cur_offset;
        mask3D.transform.position = transform.position;
    }
}
