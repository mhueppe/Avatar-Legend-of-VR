using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VRAvatar
{
    /// <summary>
    /// Calculates the positions for <see cref="NumberOfCards"/> cards along a part of a circle arc specified with a <see cref="radius"/> and <see cref="cardSpan"/>.
    /// The <see cref="cardSpan"/> specifies how long the arc is where the cards will be placed.
    /// </summary>
    public class CardPositioner : MonoBehaviour
    {
        [SerializeField] private float height;
        [SerializeField] [Min(float.Epsilon)] private float radius = 1f;
        [SerializeField] [Min(float.Epsilon)] private float cardSpan = 0.8f;
        private int NumberOfCards => handCards.Count;
        public Vector3 offset = new(-135f, 0f, 0f);

        private Vector3 _center;

        public GameObject[] cardPrefabs;
        private readonly List<Transform> handCards = new();

        public void AddCard(CardValues value)
        {
            Debug.Log(value);
            var newCard = Instantiate(cardPrefabs[(int)value - 1], transform);
            handCards.Add(newCard.transform);
            ApplyPositionsToCards(handCards);
        }

        public void OnTouchpadMoved(LeftHand leftHand, Vector2 vec)
        {
            switch (NumberOfCards)
            {
                case 0:
                    leftHand.SetSelectedCard(-1);
                    return;
                case 1:
                    leftHand.SetSelectedCard(0);
                    return;
            }

            var x = (vec.x + 1) / 2;

            var border = 1 / (float)NumberOfCards;
            var borders = new float[NumberOfCards];

            for (var i = 0; i < NumberOfCards; i++)
                borders[i] = Mathf.Abs(x - border * i - border / 2);
            var selectedCard = Array.IndexOf(borders, borders.Min());
            leftHand.SetSelectedCard(selectedCard);

        }


        // private void Update()
        // {
        //     while (handCards.Count != numberOfCards)
        //     {
        //         if (handCards.Count > numberOfCards)
        //         {
        //             foreach (var card in handCards.GetRange(numberOfCards, handCards.Count - numberOfCards))
        //                 Destroy(card.gameObject);
        //             
        //             handCards = handCards.GetRange(0, numberOfCards);
        //             break;
        //         }
        //         var go = Instantiate(cardPrefabs[0], transform, true);
        //         handCards.Add(go.transform);
        //     }
        //     ApplyPositionsToCards(handCards);
        // }

        #region Positioning Math

        public Vector3[] GetDirectionsOnArc()
        {
            if (NumberOfCards < 1)
                return Array.Empty<Vector3>();
            if (NumberOfCards == 1)
                return new[] { transform.forward };

            var positions = new Vector3[NumberOfCards];

            var occupiedAngle = cardSpan * 360 / (2 * Mathf.PI * radius);
            var directionToRightBound = Quaternion.AngleAxis(occupiedAngle / 2, transform.up) * transform.forward;
            var rotateFraction = Quaternion.AngleAxis(-occupiedAngle / (NumberOfCards - 1), transform.up);

            for (var i = 0; i < NumberOfCards; i++)
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

        public void ApplyPositionsToCards(List<Transform> transforms)
        {
            var directions = GetDirectionsOnArc();
            var positions = GetPointsOnArc();
            for (int i = 0; i < directions.Length; i++)
            {
                transforms[i].right = -directions[i];
                transforms[i].localEulerAngles += offset;
                transforms[i].position = positions[i];
            }
        }

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // the center where the disc must start.
            _center = transform.position - transform.forward * (radius - height);
            // Handles.color = Color.white;
            // Handles.DrawWireDisc(_center, transform.up, radius);

            var theta = cardSpan * 360 / (2 * Mathf.PI * radius);

            var directionToLeftBound = Quaternion.AngleAxis(theta / 2, transform.up) * transform.forward;

            foreach (var point in GetPointsOnArc())
                Handles.DrawWireCube(point, Vector3.one * radius * .02f);


            Handles.color = Color.red;
            Handles.DrawWireArc(_center, transform.up, directionToLeftBound, -theta, radius);

        }
#endif
    }
}
