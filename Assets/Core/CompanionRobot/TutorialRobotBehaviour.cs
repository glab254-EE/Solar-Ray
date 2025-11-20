using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialRobotBehaviour : MonoBehaviour
{
    [field:SerializeField]
    private Transform PlayerTransform;
    [field:SerializeField]
    private List<Transform> WayPoints = new();
    [field:SerializeField]
    private float MoveSpeed = 1;
    [field:SerializeField]
    private GameObject HoverOverInterface;
    internal string currentTask{get;private set;}
    internal bool IsMoving {get;private set;} = false;
    private bool isPlayerPointingOver = false;
    void Start()
    {
        if (HoverOverInterface.TryGetComponent(out TutorialDisplayUIHandler handler))
        {
            handler.source = this;
        }        
    }
    void Update()
    {
        if (isPlayerPointingOver && !HoverOverInterface.activeInHierarchy)
        {
            HoverOverInterface.SetActive(true);
        }
        else if (!isPlayerPointingOver && HoverOverInterface.activeInHierarchy)
        {
            HoverOverInterface.SetActive(false);
        }
        if (IsMoving)
        {
            isPlayerPointingOver = false;
        }
    }
    void OnMouseEnter()
    {
        isPlayerPointingOver = true;
    }
    void OnMouseExit()
    {
        isPlayerPointingOver = false;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == PlayerTransform)
        {
            MoveToNextWaypoint();
        }
    }
    private void MoveToNextWaypoint()
    {
        if (WayPoints.Count < 1)
        {
            Destroy(gameObject);
            return;
        }
        Transform nextPoint =  WayPoints[0];
        if (nextPoint != null && !IsMoving)
        {
            WayPoints.RemoveAt(0);
            IsMoving = true;
            float duration = Vector3.Distance(transform.position,nextPoint.position) / MoveSpeed;
            Tween moving = transform.DOMove(nextPoint.position,MoveSpeed);
            moving.OnComplete( () => {
                IsMoving = false;
            });
        } 
    }
}
