using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    [field:SerializeField]
    private GameObject MainFrameReference;
    [field:SerializeField]
    private List<GameObject> Screens;
    [field:SerializeField]
    private List<AlternativeValuePair<Slider,string>> AudioSliders;
    [field:SerializeField]
    private AudioMixer AudioMixer;
    private InputSystem_Actions inputActions;
    private void Start()
    {
        inputActions = new();
        inputActions.Player.Pause.performed += OnEscapePress;
        inputActions.Player.Pause.Enable();
        AssignSliders();
    }
    private void OnDestroy()
    {
        inputActions.Player.Pause.performed -= OnEscapePress;
        inputActions.Player.Pause.Disable();     
    }
    private void OnEscapePress(InputAction.CallbackContext _)
    {
        if (MainFrameReference.activeInHierarchy)
        {
            ClosePauseMenu();
        } else
        {
            MainFrameReference.SetActive(true);
            OpenFrame(0);
            Time.timeScale = 0;            
        }
    }
    private void AssignSliders()
    {
        foreach(AlternativeValuePair<Slider,string> keyValuePair in AudioSliders)
        {
            keyValuePair.Key.onValueChanged.AddListener(v => OnSliderChange(keyValuePair.Value,v));
        }
    }
    private void OnSliderChange(string mixer, float value)
    {
        AudioMixer.SetFloat(mixer,value);
    }
    public void ClosePauseMenu()
    {
        MainFrameReference.SetActive(false);
        Time.timeScale = 1;        
    }
    public void ExitGame() => Application.Quit(0);
    public void OpenFrame(int index)
    {
        if (index >= 0 && index < Screens.Count)
        {
            foreach(GameObject screen in Screens)
            {
                screen.SetActive(false);
            }
            Screens[index].SetActive(true);
        }
    }
}
[System.Serializable]
public struct AlternativeValuePair<T1, T2>
{
    [field:SerializeField]
    public T1 Key;
    [field:SerializeField]
    public T2 Value;
}