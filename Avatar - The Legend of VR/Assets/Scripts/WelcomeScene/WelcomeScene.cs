using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Preloads the questionnaire scene until a <see cref="Button"/> was pressed from the UI to signal the start of the questionnaire.
/// </summary>
[RequireComponent(typeof(UIDocument))]
public class WelcomeScene : MonoBehaviour
{
    void Start()
    {
        // Preload the questionnaire scene until the user starts with the experiment.
        var loadSceneAsync = SceneManager.LoadSceneAsync("Questionaire", LoadSceneMode.Single);
        loadSceneAsync.allowSceneActivation = false;

        // Release the async loaded scene after the start button was pressed.
        GetComponent<UIDocument>()
            .rootVisualElement
            .Q<Button>("start-experiment")
            .clickable
            .clicked += () => loadSceneAsync.allowSceneActivation = true;
    }
}
