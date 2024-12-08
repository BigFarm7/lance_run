using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PauseCon : MonoBehaviour
{
    private void OnEnable()
    {
       CanvasGroup _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.DOFade(1, 0.2f).SetUpdate(true);
    }
}
