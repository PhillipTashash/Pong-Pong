using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    //ResumeButton
    //QuitButton
    //VolumeSlider

    private string resumeButtonName = "ResumeButton";
    private string quitButtonName = "QuitButton";
    private string volumeSliderName = "VolumeSlider";
    private string mainMenuSceneName = "MainMenu";

    [SerializeField] private InputActionReference pauseActions;

    private VisualElement overlay;
    private bool isPaused = false;


    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        overlay = root.Q<VisualElement>("Root");
        overlay.AddToClassList("hidden");

        BindButton(root, resumeButtonName, ResumeGame);
        BindButton(root, quitButtonName, QuitToMainMenu);
        BindSlider(root, volumeSliderName, UpdateVolume);

        pauseActions.action.Enable();
    }

    void OnDisable()
    {
        pauseActions.action.Disable();
    }

    private void Update()
    {
        if (pauseActions.action.WasCompletedThisFrame())
        {
            TogglePause();
        }
    }
    
    private void TogglePause()
    {
        isPaused = !isPaused;
        overlay.EnableInClassList("hidden", !isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void ResumeGame()
    {
        AudioManager.Instance.PlayClick();
        TogglePause();
    }

    private void QuitToMainMenu()
    {
        AudioManager.Instance.PlayClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    private void BindButton(VisualElement root, string buttonName, System.Action action)
    {
        Button button = root.Q<Button>(buttonName);
        if (button != null)
        {
            button.clicked += action;
            button.RegisterCallback<PointerEnterEvent>(evt => AudioManager.Instance.PlayHover());
        }
        else
        {
            Debug.LogError("Button not found: " + buttonName);
        }
    }
    
    private void BindSlider(VisualElement root, string sliderName, System.Action<int> action)
    {
        SliderInt slider = root.Q<SliderInt>(sliderName);
        if (slider != null)
        {
            slider.value = Mathf.RoundToInt(AudioListener.volume * 100f);
            slider.RegisterValueChangedCallback(evt => action(evt.newValue));
        }
        else
        {
            Debug.LogError("Slider not found: " + sliderName);
        }
    }
    private void UpdateVolume(int value)
    {
        AudioManager.Instance.SetMasterVolume(value / 100f);
    }
}
