using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Calculates the positions for <see cref="numberOfCards"/> cards along a part of a circle arc specified with a <see cref="radius"/> and <see cref="cardSpan"/>.
/// The <see cref="cardSpan"/> specifies how long the arc is where the cards will be placed.
/// </summary>
public class CardPositioner : MonoBehaviour
{
    [SerializeField] private float height;
    [SerializeField] [Min(float.Epsilon)] private float radius = 1f;
    [SerializeField] [Min(float.Epsilon)] private float cardSpan = 0.8f;
    [SerializeField] [Min(0)] private int numberOfCards = 4;
    
    private Vector3 _center;

    public GameObject cardPrefab;
    private List<Transform> cards = new();

    public void OnTouchpadMoved(LeftHand leftHand, Vector2 vec)
    {
        switch (numberOfCards)
        {
            case 0:
                leftHand.SetSelectedCard(-1);
                return;
            case 1:
                leftHand.SetSelectedCard(0);
                return;
        }

        var x = (vec.x + 1) / 2;
        
        var border =  1 / (float)numberOfCards;
        var borders = new float[numberOfCards];
        
        for (var i = 0; i < numberOfCards; i++)
            borders[i] = Mathf.Abs(x - border*i - border/2);
        var selectedCard = Array.IndexOf(borders, borders.Min());
        leftHand.SetSelectedCard(selectedCard);

    }
    
    
    private void Update()
    {
        while (cards.Count != numberOfCards)
        {
            if (cards.Count > numberOfCards)
            {
                foreach (var card in cards.GetRange(numberOfCards, cards.Count - numberOfCards))
                    Destroy(card.gameObject);
                
                cards = cards.GetRange(0, numberOfCards);
                break;
            }
            var go = Instantiate(cardPrefab, transform, true);
            cards.Add(go.transform);
        }
        ApplyPositionsToCards(cards.ToArray());
    }

    public Vector3[] GetDirectionsOnArc()
    {
        if (numberOfCards < 1) 
            return Array.Empty<Vector3>();
        if (numberOfCards == 1)
            return new[] { transform.forward };

        var positions = new Vector3[numberOfCards];

        var occupiedAngle = cardSpan * 360 / (2 * Mathf.PI * radius);
        var directionToRightBound = Quaternion.AngleAxis(occupiedAngle/2, transform.up) * transform.forward;
        var rotateFraction = Quaternion.AngleAxis(-occupiedAngle / (numberOfCards - 1), transform.up);

        for (var i = 0; i < numberOfCards; i++)
        {
            positions[i] = directionToRightBound;
            directionToRightBound = rotateFraction * directionToRightBound;
        }

        return positions;
    }

    public Vector3[] GetPointsOnArc()
    {
        _center = transform.position - transform.forward * (radius - height);
        return GetDirectionsOnArc()
            .Select(direction => _center + direction * radius)
            .ToArray();
    }

    public void ApplyPositionsToCards(Transform[] transforms)
    {
        var directions = GetDirectionsOnArc();
        var positions = GetPointsOnArc();
        for (int i = 0; i < directions.Length; i++)
        {
            transforms[i].right = -directions[i];
            transforms[i].localEulerAngles += new Vector3(-135f, 0f, 0f);
            transforms[i].position = positions[i];
        }
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // the center where the disc must start.
        _center = transform.position - transform.forward * (radius - height);
        // Handles.color = Color.white;
        // Handles.DrawWireDisc(_center, transform.up, radius);

        var theta = cardSpan * 360 / (2 * Mathf.PI * radius);
        
        var directionToLeftBound = Quaternion.AngleAxis(theta/2, transform.up) * transform.forward;

        foreach (var point in GetPointsOnArc())
            Handles.DrawWireCube(point, Vector3.one * radius * .02f);
        
        
        Handles.color = Color.red;
        Handles.DrawWireArc(_center, transform.up, directionToLeftBound, -theta, radius);

    }
    #endif
}
