using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardInformation")]
public class CardInformationScriptableObject : ScriptableObject
{
    public int numberOfHandCardsAtStart;
    public CardStaple cardStaple;

    public string Header()
    {
        var strs = new string[cardStaple.cardValuesArray.Length];
        for (int i = 0; i < strs.Length; i++)
            strs[i] = $"HandCard_{i}";
        
        return string.Join(",", strs);
    }
    
    public override string ToString()
    {
        return string.Join(
            ",",
            cardStaple.cardValuesArray.Select(card => card.ToString())
        );
    }
}

public enum CardValues
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Twelve = 12,
    Thirteen = 13
}

[Serializable]
public class CardStaple
{
    public Stack<CardValues> values;
    public CardValues[] cardValuesArray;
    private int i = 0;

    public CardValues GetNextCardFromStaple() 
        => i < cardValuesArray.Length
        ? cardValuesArray[i++]
        : CardValues.Eight;
}
