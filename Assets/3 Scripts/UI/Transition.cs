using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class Transition : MonoBehaviour
{
    public System.Action InAnimationFinished;

    [SerializeField] private float partsAnimationDuration;
    [SerializeField] private Ease partsAnimationEase;
    [Space]
    [SerializeField] private Image topPart;
    [SerializeField] private Image bottomPart;

    private float _canvasHeight;
    private Coroutine _loadingTextRoutine;
    private bool isLoading;
    public Color transitionColor;

    public void Start()
    {
        _canvasHeight = GetComponent<RectTransform>().sizeDelta.x;

        topPart.transform.localPosition = new Vector2(0, 0);
        bottomPart.transform.localPosition = new Vector2(0, -0);

        bottomPart.color = topPart.color = transitionColor;
        StartInAnimation();
    }

  

    public void StartInAnimation()
    {
        isLoading = true;
        Animate(_canvasHeight, 1);
        //    Timer.StartNew(this, partsAnimationDuration, () => {
        //        _loadingTextRoutine = StartCoroutine(LoadingTextAnimation());
        //       InAnimationFinished();
        //    });

        //  Invoke("DelayedAnimate", 0.3f);
    }

    public void Close()
    {
        if (_loadingTextRoutine != null)
            StopCoroutine(_loadingTextRoutine);

        isLoading = false;

        Animate(0, 1);
        Invoke("DelayedAnimate", 0.75f);
        //  Timer.StartNew(this, 0.3f, () => Animate(_canvasHeight, 0));
        //  Timer.StartNew(this, 0.3f + partsAnimationDuration, () => gameObject.SetActive(false));
    }

    private void DelayedAnimate()
    {

        Animate(_canvasHeight, 0);
    }

    private void Animate(float targetPosition, float targetTextAlpha)
    {
        bottomPart.transform.DOLocalMoveX(targetPosition, partsAnimationDuration).SetEase(partsAnimationEase);
        topPart.transform.DOLocalMoveX(-targetPosition, partsAnimationDuration).SetEase(partsAnimationEase);

    }
}
