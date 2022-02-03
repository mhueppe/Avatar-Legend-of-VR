using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardInformation")]
public class TrialSetupScriptableObject : ScriptableObject
{
    public AvatarSetupInfos avatarSetupInfos;
    public CardSetupInfos cardSetupInfos;
}

