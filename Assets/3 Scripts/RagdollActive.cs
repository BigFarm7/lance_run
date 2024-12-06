using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RagdollActive : MonoBehaviour
{
    public Transform givePower;
    Transform Cam_Offset, mask3D, offset;
    Vector3 currentTrans, Cam_cur_offset;
    Rigidbody rb;
    private void Start()
    {
        Cam_Offset = GameObject.Find("Camera Offset").transform;
        mask3D = GameObject.FindGameObjectWithTag("Mask").transform;
        currentTrans = transform.position;
        Cam_cur_offset = Cam_Offset.position;

        rb = givePower.GetComponent<Rigidbody>();
        givePower.GetComponent<Rigidbody>().AddForce(transform.forward * rb.velocity.magnitude * 1000 + transform.up * rb.velocity.magnitude * 1000, ForceMode.Impulse);
    }

    private void Update()
    {
        Cam_Offset.transform.position = (transform.position - currentTrans) + Cam_cur_offset;
        mask3D.transform.position = transform.position;
    }
}
