using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    
    [SerializeField] private string gameSceneName = "Pong";

    //Name of all the buttons on the menu.
    [SerializeField] private string resumeButtonName = "ResumeButton";
    [SerializeField] private string playButtonName = "PlayButton";
    [SerializeField] private string restartButtonName = "RestartButton";
    [SerializeField] private string quitButtonName = "QuitButton";


    

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        BindButton(root, playButtonName, StartGame);
        BindButton(root, quitButtonName, QuitGame);

    }

    private void BindButton(VisualElement root, string buttonName, System.Action action)
    {
        Button button = root.Q<Button>(buttonName);
        if (button != null)
        {
            button.clicked += action;
        }
        else
        {
            Debug.LogError("Button not found: " + buttonName);
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }
    private void QuitGame()
    {
        Application.Quit();
    }

}
