using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * 50 + transform.up * 10, ForceMode.Impulse);
    }
}
