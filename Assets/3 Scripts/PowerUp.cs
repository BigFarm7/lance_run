using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.transform.GetComponent<PlayerController>().forwardVec * 50 - transform.up * 50, ForceMode.Impulse);
    }
}
