using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_UI : MonoBehaviour
{
  
   
    void OnEnable()
    {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(transform.DOScale(transform.localScale * 1.4f, 0.1f));

        mySequence.Append(transform.DOScale(transform.localScale, 0.1f));
    }
}
