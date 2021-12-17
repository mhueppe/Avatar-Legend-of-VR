using UnityEngine;
using UnityEngine.Events;

public abstract class ExperimentBaseEvents : MonoBehaviour
{
    /// <summary>
    /// The phase the experiment is currently in.
    /// (not sure if needed at all or if it should be moved to the derived class)
    /// </summary>
    public ExperimentPhase Phase { get; protected set; }
    
    /// <summary>
    /// Called once after the experiment was created.
    /// !! Please make sure to unsubscribe from the event if the subscribed object dies !!
    /// </summary>
    public event OnExperimentInitiateAction OnExperimentInitiate;
    public delegate void OnExperimentInitiateAction();
    [SerializeField] private UnityEvent OnExperimentInitiateUnity; // to use in editor to allow for easy interactions.
    
    /// <summary>
    /// Called at the beginning of every new trial.
    /// !! Please make sure to unsubscribe from the event if the subscribed object dies !!
    /// </summary>
    public event OnTrialInitiateAction OnTrialInitiate;
    public delegate void OnTrialInitiateAction(TrialInfo info);
    [SerializeField] private UnityEvent<TrialInfo> OnTrailInitiateUnity;// to use in editor to allow for easy interactions.


    /// <summary>
    /// Called at the end of every trial.
    /// !! Please make sure to unsubscribe from the event if the subscribed object dies !!
    /// </summary>
    public event OnTrialEndAction OnTrialEnd;
    public delegate void OnTrialEndAction(TrialInfo info);
    [SerializeField] private UnityEvent<TrialInfo> OnTrialEndUnity;// to use in editor to allow for easy interactions.


    /// <summary>
    /// Called at the end of the entire experiment for clean-up matters.
    /// !! Please make sure to unsubscribe from the event if the subscribed object dies !!
    /// </summary>
    public event OnExperimentEndAction OnExperimentEnd;
    public delegate void OnExperimentEndAction();
    [SerializeField] private UnityEvent OnExperimentEndUnity;// to use in editor to allow for easy interactions.

    private void Awake()
    {
        // Make the .Invoke method of the respective unity events a listener of the actual event. 
        OnExperimentInitiate += () => OnExperimentInitiateUnity.Invoke();
        OnTrialInitiate += info => OnTrailInitiateUnity.Invoke(info);
        OnTrialEnd += info => OnTrialEndUnity.Invoke(info);
        OnExperimentEnd += () => OnExperimentEndUnity.Invoke();
    }
    
    /// <summary>
    /// Invokes the OnExperimentInitiate event.
    /// </summary>
    protected void InvokeOnExperimentInitiate() => OnExperimentInitiate?.Invoke();
    
    /// <summary>
    /// Invokes the OnTrialInitiate event.
    /// </summary>
    /// <param name="info">required parameter for the event with information about the current trial.</param>
    protected void InvokeOnTrialInitiate(TrialInfo info) => OnTrialInitiate?.Invoke(info);
    
    /// <summary>
    /// Invokes the OnTrialEnd event.
    /// </summary>
    /// <param name="info">required parameter for the event with information about the current trial.</param>
    protected void InvokeOnTrialEnd(TrialInfo info) => OnTrialEnd?.Invoke(info);
    
    /// <summary>
    /// Invokes the OnExperimentEnd event.
    /// </summary>
    protected void InvokeOnExperimentEnd() => OnExperimentEnd?.Invoke();
}
