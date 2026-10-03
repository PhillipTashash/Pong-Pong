using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    private string gameSceneName = "Pong";
    private string playButtonName = "PlayButton";
    private string quitButtonName = "QuitButton";
    private string leftNameField = "LeftNameField";
    private string rightNameField = "RightNameField";
    private string volumeSlider = "VolumeSlider";

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        BindButton(root, playButtonName, StartGame);
        BindButton(root, quitButtonName, QuitGame);
        BindTextField(root, leftNameField, TypeSound);
        BindTextField(root, rightNameField, TypeSound);
        BindSlider(root, volumeSlider, UpdateVolume);
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

    private void BindTextField(VisualElement root, string textFieldName, System.Action action)
    {
        TextField textField = root.Q<TextField>(textFieldName);
        if (textField != null)
        {
            textField.RegisterValueChangedCallback(evt => action());
        }
        else
        {
            Debug.LogError("TextField not found: " + textFieldName);
        }
    }

    private void UpdateVolume(int value)
    {
        AudioManager.Instance.SetMasterVolume(value / 100f);
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

    private void TypeSound()
    {
        AudioManager.Instance.PlayTypeSound();
    }

    private void StartGame()
    {
        AudioManager.Instance.PlayClick();
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }
    private void QuitGame()
    {
        Application.Quit();
    }

}
