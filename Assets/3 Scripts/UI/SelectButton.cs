using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
    public GameObject[] images;

    public void Selected()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log(transform.GetChild(0));
        AudioManager.instance.PlaySfx(2);
    }
}
