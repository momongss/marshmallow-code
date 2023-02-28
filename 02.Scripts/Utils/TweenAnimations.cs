using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TweenAnimations
{
    public static Sequence ScaleToZero(Transform transform, UnityAction callback = null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(
                transform
                    .DOScale(transform.localScale * 1.12f, 0.14f)
                    .SetEase(Ease.InOutBounce)
                    .OnComplete(() =>
                    {
                        transform
                            .DOScale(Vector3.zero, 0.25f)
                            .OnComplete(() =>
                            {
                                if (callback != null) callback();
                            });
                    })
            );

        return sequence;
    }

    public static Sequence UI_Scaling_Show(Transform transform, Vector3 scale)
    {
        transform.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(
                transform
                    .DOScale(scale, 0.25f)
                    .SetEase(Ease.OutBounce)
            );

        return sequence;
    }
}
