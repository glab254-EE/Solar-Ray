using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialDisplayUIHandler : MonoBehaviour
{
    [field:SerializeField]
    private List<Transform> frames;
    [field:SerializeField]
    private float ShowDuration = 5f;
    internal TutorialRobotBehaviour source;
    private InputSystem_Actions inputActions;
    private int CurrentFrame = 0;
    void Start()
    {
        inputActions = new();
        inputActions.Player.Option1.performed += OnOption1Press;
        inputActions.Player.Option2.performed += OnOption2Press;
        inputActions.Player.Option1.Enable();
        inputActions.Player.Option2.Enable();
    }
    void OnDestroy()
    {
        inputActions.Player.Option1.performed -= OnOption1Press;
        inputActions.Player.Option2.performed -= OnOption2Press;
        inputActions.Player.Option1.Disable();
        inputActions.Player.Option2.Disable();        
    }
    private void UpdateFrames()
    {
        foreach(Transform frame in frames)
        {
            frame.gameObject.SetActive(false);
        }
        if (frames.Count > CurrentFrame && CurrentFrame >= 0)
        {
            frames[CurrentFrame].gameObject.SetActive(true);
            if (frames[CurrentFrame].gameObject.TryGetComponent(out AudioSource source))
            {
                source.Play();
            }
        }
    }
    private void OnOption1Press(InputAction.CallbackContext _)
    {
        if (!gameObject.activeInHierarchy) return;
        if (CurrentFrame == 0 || CurrentFrame == 1)
        {
            CurrentFrame = CurrentFrame == 1 ? 0 : 1;
            UpdateFrames();
            StartCoroutine(ShowJokeEnumerator());
        }
    }
    private void OnOption2Press(InputAction.CallbackContext _)
    {
        if (!gameObject.activeInHierarchy) return;
        if (CurrentFrame == 0 || CurrentFrame == 2)
        {
            CurrentFrame = CurrentFrame == 2 ? 0 : 2;
            UpdateFrames();
            StartCoroutine(ShowJokeEnumerator());
        } 
    }
    private IEnumerator ShowJokeEnumerator()
    {
        yield return new WaitForSeconds(ShowDuration);
        CurrentFrame = 0;
        UpdateFrames();
    }
}
