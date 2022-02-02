using System;

/// <summary>
/// Different experiment conditions
/// </summary>
public enum Condition
{
    Tutorial,
    Control,
    Custom
}

/// <summary>
/// Trial information which holds the current state of the experiment in each trail.
/// </summary>
public class TrialInfo
{
    /// <summary>
    /// Id of the current participant.
    /// </summary>
    public int participantID;

    /// <summary>
    /// The overall count of trials the participant is currently absolving.
    /// </summary>
    public int trialNumber;

    /// <summary>
    /// Experiment condition the participant is currently in.
    /// </summary>
    public Condition condition;

    public CardInformationScriptableObject cardInformation;

    // todo add header cols
    public ParticipantPreferences participantPreferences;

    // todo maybe some more information on the character needed? @Michael

    public string Header() 
    {
        return string.Format("{0},{1},{2},", 
            nameof(participantID), 
            nameof(trialNumber), 
            nameof(condition)) + cardInformation.Header();
    }

    
    /// <summary>
    /// Directly usable to record the experiment.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("{0},{1},{2},", 
            participantID, 
            trialNumber, 
            condition) + cardInformation;
    }
}
