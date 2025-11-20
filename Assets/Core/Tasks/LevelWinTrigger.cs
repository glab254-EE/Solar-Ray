using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelWinTrigger : MonoBehaviour
{
    [field:SerializeField]
    private GameObject WinCameraObject;
    [field:SerializeField]
    private List<Transform> Positions;
    [field:SerializeField]
    private List<float> Durations;

    public void Trigger()
    {
        if (WinCameraObject != null)
        {
            WinCameraObject.SetActive(true);
            if (Positions.Count >= 1)
            {
                StartCoroutine(CameraMovementEnumerator());
            }
        }
    }
    private IEnumerator CameraMovementEnumerator()
    {
        WinCameraObject.transform.SetPositionAndRotation(
            Positions[0].position, 
            Positions[0].rotation);
        if (Positions.Count>1 && Durations.Count >= Positions.Count)
        {
            yield return new WaitForSeconds(Durations[0]);
            for (int i= 1; i < Positions.Count; i++)
            {
                float duration = Durations[i];
                Transform point = Positions[i];

                Tween moveTween = WinCameraObject.transform.DOMove(point.position,duration);
                WinCameraObject.transform.DORotateQuaternion(point.rotation,duration);
                yield return moveTween.WaitForCompletion();
            }
        }
        GameScenesManager.Instance.AvailableLastSceneIndex ++ ;
    }
}
