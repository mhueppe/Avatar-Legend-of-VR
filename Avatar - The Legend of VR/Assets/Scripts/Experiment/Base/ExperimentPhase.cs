/// <summary>
/// In which phase the experiment is currently in.
/// </summary>
public enum ExperimentPhase
{
    InitializeExperiment, // the very start of all
    InitializeTrial, // present at each beginning of a new trial
    TrialDone, // when trial is over
    ExperimentFinished // the very end of all
}
