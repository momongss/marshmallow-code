using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UICanvas : MonoBehaviour
{
    public Canvas canvas;
    public Transform panel;

    protected Vector3 originScale;

    UnityEvent openEvent = new UnityEvent();
    UnityEvent closeEvent = new UnityEvent();

    [SerializeField] AudioSource openSound;
    [SerializeField] AudioSource closeSound;

    public bool isHideOnPlay = true;

    protected virtual void Awake()
    {
        originScale = panel.localScale;

        if (openSound)
        {
            openEvent.AddListener(() =>
            {
                openSound.Play();
            });
        }

        if (closeSound)
        {
            closeEvent.AddListener(() =>
            {
                closeSound.Play();
            });
        }

        if (isHideOnPlay)
        {
            Hide();
        }
    }

    public virtual void Show()
    {
        canvas.enabled = true;
        openEvent.Invoke();
    }

    public virtual void Hide()
    {
        canvas.enabled = false;
        closeEvent.Invoke();
    }

    protected virtual void OnValidate()
    {
        panel = transform.GetChild(0);
        canvas = GetComponent<Canvas>();
    }
}
