using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class GoodButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 originScale;

    public float speed = 2.0f;

    Sequence sequence;

    public Vector3 scaling = new Vector3(1.2f, 0.8f, 1.2f);
    public float durationSquash;
    public float durationStretch;

    [SerializeField] UnityEvent e;

    private void Start()
    {
        if (originScale == Vector3.zero)
        {
            originScale = transform.localScale;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        Squash();
    }

    public void OnPointerUp(PointerEventData data)
    {
        Stretch();
    }

    public void Squash()
    {
        if (sequence != null) sequence.Kill();

        Vector3 target_scale = new Vector3(
            originScale.x * scaling.x,
            originScale.y * scaling.y,
            originScale.z * scaling.z
            );

        sequence = DOTween.Sequence();
        sequence.Append(transform
            .DOScale(target_scale, 0.3f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                sequence = null;
            }));
    }

    public void Stretch()
    {
        if (sequence != null) sequence.Kill();

        sequence = DOTween.Sequence();
        sequence.Append(transform
            .DOScale(originScale, 0.2f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                e.Invoke();
                sequence = null;
            }));
    }
}
