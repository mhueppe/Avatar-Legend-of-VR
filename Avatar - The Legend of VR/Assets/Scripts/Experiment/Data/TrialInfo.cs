using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trial information which holds the current state of the experiment in each trail.
/// </summary>
public class TrialInfo : IRecordable
{
    /// <summary>
    /// Id of the current participant.
    /// </summary>
    public int participantID;

    /// <summary>
    /// The overall count of trials the participant is currently absolving.
    /// </summary>
    public int trialNumber;

    public AvatarSetupInfos avatarSetupInfos;

    public CardSetupInfos cardSetupInfos;

    // todo add header cols
    public ParticipantPreferences participantPreferences;

    // todo maybe some more information on the character needed? @Michael
    
    public readonly List<CardValues> cardsPicked = new();
    
    #region IRecordable Implementation
    public int NumberOfColumnsNeeded { get; }
    public string[] Record()
    {
        throw new NotImplementedException();
    }

    public string[] GetHeader()
    {
        throw new NotImplementedException();
    }

    public string OnRecordingExceptionCaught(Exception e)
    {
        throw new NotImplementedException();
    }

    public void OnGetHeaderExceptionCaught(Exception e, int failedHeaderNo)
    {
        Debug.LogError($"Exception occured while recording: {e.Message}");
    }
    #endregion
}
