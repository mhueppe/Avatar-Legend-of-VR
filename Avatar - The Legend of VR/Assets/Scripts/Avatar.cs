using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    private int _questionnaireMatch;
    public int QuestionnaireMatch { get => _questionnaireMatch; set => _questionnaireMatch = Mathf.Clamp(value, 0, 100); }

}
