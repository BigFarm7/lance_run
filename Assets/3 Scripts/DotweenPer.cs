using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class DotweenPer : MonoBehaviour
{
    TextMeshProUGUI text;  // 내려갈 텍스트 UI 객체
    public TextMeshProUGUI flip;
    public float moveDuration = 2f;     // 이동 시간
    public float moveDistance = 100f;   // 이동 거리
    public float shakeDuration = 1f;    // 흔들림 시간
    public float shakeStrength = 10f;   // 흔들림 강도
    public int shakeVibrato = 10;      // 흔들림 횟수
    int Score;
    private void Awake()
    {
        Score = 0;
        text = GetComponent<TextMeshProUGUI>();
    }
    void OnEnable()
    {
        Score = 0;
        text.text = "0";
        text.transform.DOMoveY(text.transform.position.y - moveDistance, moveDuration)
        .SetEase(Ease.OutCubic);

    }

    public void GetPoint(int getScore)
    {
        Score += getScore;

        text.text = Score.ToString();
    }

    public void Up()
    {
        text.transform.DOMoveY(text.transform.position.y + moveDistance, moveDuration)
       .SetEase(Ease.OutCubic).OnKill(() => text.gameObject.SetActive(false));
    }

    public void fliping(int i)
    {
        if (i == 0)
        {
            flip.gameObject.SetActive(true);
            flip.text = "360도 프런트플립!";

            flip.alpha = 0f;
            flip.DOFade(1f, 0.5f);
            flip.transform.localScale = new Vector3(1f, 1f, 1f);
            flip.transform.DOScale(0.8f, 2f).OnKill(() => flip.gameObject.SetActive(false));
        }
        else
        {
            flip.gameObject.SetActive(true);
            flip.text = "360도 백플립!";

            flip.alpha = 0f;
            flip.DOFade(1f, 0.5f);
            flip.transform.localScale = new Vector3(1f, 1f, 1f);
            flip.transform.DOScale(0.8f, 2f).OnKill(() => flip.gameObject.SetActive(false));
        }
        AudioManager.instance.PlaySfx(5);
        GetPoint(10);
    }
}
