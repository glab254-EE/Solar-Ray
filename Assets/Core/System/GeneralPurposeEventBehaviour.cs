using System;
using UnityEngine;

public class GeneralPurposeEventBehaviour : MonoBehaviour
{
    public event Action connections;

    internal void Trigger()
    {
        connections?.Invoke();
    }
}
