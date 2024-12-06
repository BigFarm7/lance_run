using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bounceForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        // 바닥과 충돌했을 때 반응
        if (collision.gameObject.CompareTag("Ragdoll"))
        {
            
            // 충돌 지점의 법선 벡터를 이용해 반동 벡터를 계산
            Vector3 bounceDirection = collision.contacts[0].normal;

            // 반동 벡터에 힘을 추가
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("aa");
                rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
            }

            // 각 본의 Rigidbody에도 개별적으로 반동을 추가할 수 있음
            foreach (Rigidbody childRb in GetComponentsInChildren<Rigidbody>())
            {
                if (childRb != null && childRb != rb)
                {
                    childRb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
                }
            }
        }
    }
}
