using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This <see cref="ScriptableObject"/> holds all information needed to initialize
/// a dogs game with arbitrary board state and hand cards.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/DogsSetup")]
public class DogsSetupScriptableObject : ScriptableObject
{
    [Header("Super geiles Board setup!")]
    [Tooltip("Hand cards of the participant - choose size to define number of hand cards.")]
    public CardType[] handCards;
    [Tooltip("The card type the participant will receive from his partner.")]
    public CardType swappedCard;
    
    [Header("Board positions of the teams pieces.\n* 64 -> in home\n* -1, -2, -3, -4 for in goal positions (while -4 is the furthest back goal field)")]
    public TeamSetup redTeam;
    public TeamSetup blueTeam;
    public TeamSetup greenTeam;
    public TeamSetup yellowTeam;
    
}

public enum CardType
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine, 
    Ten, 
    Eleven, 
    Twelve,
    Thirteen, 
    Switch
}

[Serializable]
public struct TeamSetup
{
    public int positionPiece1;
    public int positionPiece2;
    public int positionPiece3;
    public int positionPiece4;
}
