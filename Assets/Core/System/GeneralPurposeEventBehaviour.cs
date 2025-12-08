using System;
using UnityEngine;

public class GeneralPurposeEventBehaviour : MonoBehaviour
{
    public event Action connections;

    public void Trigger()
    {
        connections?.Invoke();
    }
}
