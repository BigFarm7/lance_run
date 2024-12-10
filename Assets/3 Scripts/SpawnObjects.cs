using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public static int i = 0;
    public GameObject CcalCon;
    public GameObject JumpDae;
    public GameObject[] Lines;
    public Transform[] RockPoints;
    public GameObject[] RockPrefs;
    public int level;
    void Start()
    {
        i++;

        if (i > 4)
            level = 2;
        if (i > 8)
            level = 3;
        if (i > 12)
            level = 5;


        if (transform.gameObject.tag == "Ill")
        {

        }
        else
        {
            Lines = new GameObject[3];

            for (int i = 0; i < 3; i++)
            {
                Lines[i] = transform.GetChild(i + 1).gameObject;
            }

            for (int i = 0; i < level; i++)
            {
                int randNum1 = Random.Range(0, Lines.Length);

                for (int j = 0; j < Lines[randNum1].transform.childCount; j++)
                {
                    int randNum2 = Random.Range(0, Lines[randNum1].transform.childCount);

                    if (Lines[randNum1].transform.GetChild(randNum2).childCount > 0)
                        break;
                    else Instantiate(CcalCon, Lines[randNum1].transform.GetChild(randNum2).position, Lines[randNum1].transform.GetChild(randNum2).rotation, Lines[randNum1].transform.GetChild(randNum2));
                }

            }
        }
        if (transform.tag == "Basic")
        {


            Material material = GetComponent<MeshRenderer>().sharedMaterial;

            Material instanceMat = new Material(material);

            transform.GetComponent<MeshRenderer>().sharedMaterial = instanceMat;

            Color currentColor = instanceMat.color;

            currentColor.a = 0f;

            DOTween.To(() => currentColor.a, x =>
            {
                currentColor.a = x;
                instanceMat.color = currentColor;
            }, 1f, 1f);


            StartCoroutine(MakeRock());
        }


        for (int i = 0; i < 5; i++)
        {
            int j = Random.Range(0, 5);
            Instantiate(RockPrefs[j], RockPoints[i].position, Quaternion.identity, transform);
        }
    }
    IEnumerator MakeRock()

    {



        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < 5; i++)
        {
            int j = Random.Range(0, 5);
            GameObject newObj = Instantiate(RockPrefs[j], RockPoints[i].position, Quaternion.identity, transform);
            newObj.transform.localScale = Vector3.zero;

            newObj.transform.DOScale(Vector3.one * 1.75f, 0.75f).SetEase(Ease.OutCubic);
        }


    }
}
