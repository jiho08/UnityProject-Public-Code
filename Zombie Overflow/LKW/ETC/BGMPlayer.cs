using System;
using Feedbacks.SFX;
using UnityEngine;
using UnityEngine.Events;

public class BGMPlayer : MonoBehaviour
{
    public UnityEvent InitEvent;

    private void Start()
    {
        InitEvent?.Invoke();
    }
}
