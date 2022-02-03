using System;
using UnityEngine;
using Random = System.Random;

[Serializable]
public struct CardSetupInfos
{
    public int numberOfHandCardsAtStart;
    public CardStaple cardStaple;
}

[Serializable]
public class CardStaple
{
    [SerializeField] private int rndSeed;
    private Random _rnd;
    private string[] _enumNames;

    public CardValues GetNextCardFromStaple()
    {
        if (_rnd == null)
        {
            _rnd = new Random(rndSeed);
            _enumNames = Enum.GetNames(typeof(CardValues));
        }

        var idx = _rnd.Next(_enumNames.Length);
        return Enum.Parse<CardValues>(_enumNames[idx]);
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