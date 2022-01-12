using System;

/// <summary>
/// This interface defines what's needed for the <see cref="Recorder"/> to record an object or value over time.
/// </summary>
public interface IRecordable
{
    /// <summary>
    /// The number of values which will be returned from <see cref="GetHeader"/> and <see cref="Record"/>.
    /// If any exception in <see cref="Record"/> or <see cref="GetHeader"/> occurs it's important for the <see cref="Recorder"/> to know how many placeholder must be inserted the respective frame.
    /// </summary>
    public int NumberOfColumnsNeeded { get; }
    
    /// <summary>
    /// This function will be called from the <see cref="Recorder"/> to record one frame.
    /// It should return whatever should be recorded with a constant array size over the whole recording.
    /// </summary>
    public string[] Record();
    
    /// <summary>
    /// Returns the csv column headers for the values returned by <see cref="Record"/>.
    /// </summary>
    public string[] GetHeader();

    /// <summary>
    /// Called when an exception occured after calling this objects <see cref="Record"/>.
    /// </summary>
    /// <param name="e">The caught exception</param>
    /// <returns>What will be written into the csv row instead.</returns>
    public string OnRecordingExceptionCaught(Exception e);

    /// <summary>
    /// Called when an exception occured after after calling this objects <see cref="GetHeader"/>.
    /// </summary>
    /// <param name="e">The caught exception.</param>
    /// <param name="failedHeaderNo">The index included into the header of this object.</param>
    public void OnGetHeaderExceptionCaught(Exception e, int failedHeaderNo);
}
