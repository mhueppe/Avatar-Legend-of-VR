using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour
{
    public static ExperimentController Instance { get; private set; }

    private const string StartScene = "StartScene";
    private const string Questionnaire = "Questionnaire";
    private const string Dog = "DOG";
    private const string End = "EndScene";
    private const string Debug = "VR Grab";
    
    private static readonly Dictionary<string, string> SceneTransitions = new()
    {
        {StartScene, Questionnaire},
        {Questionnaire, Debug},
        {Dog, Dog},
    };

    [SerializeField] private TrialSetupScriptableObject[] trialSetups;

    public ParticipantPreferences Preferences { get; set; } = new();
    public int ParticipantID { get; set; }
    
    private int _currentTrial = -1;

    public void Awake()
    {
        // Singleton pattern with dont destroy on load (lives across scenes) 
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(this);
    }
    
    public static void TransitionToNextScene()
    {
        var nextScene = SceneTransitions[SceneManager.GetActiveScene().name];
        SceneManager.LoadScene(nextScene);
    }

    public TrialInfo InitNewTrial()
    {
        _currentTrial++;
        if (_currentTrial == trialSetups.Length)
        {
            SceneTransitions[Dog] = End;
            TransitionToNextScene();
            return null;
        }
        return new TrialInfo
        {
            avatarSetupInfos = trialSetups[_currentTrial].avatarSetupInfos,
            cardSetupInfos = trialSetups[_currentTrial].cardSetupInfos,
            participantPreferences = Preferences,
            trialNumber = _currentTrial,
            participantID = ParticipantID
        };;
    }
}