using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VRAvatar
{
    /// <summary>
    /// Calculates the positions for <see cref="NumberOfCards"/> cards along a part of a circle arc specified with a <see cref="radius"/> and <see cref="cardSpan"/>.
    /// The <see cref="cardSpan"/> specifies how long the arc is where the cards will be placed.
    /// </summary>
    public class HandCards : MonoBehaviour
    {
        #region Positioning Math Fields
        
        [SerializeField] private float height;
        [SerializeField] [Min(float.Epsilon)] private float radius = 1f;
        [SerializeField] [Min(float.Epsilon)] private float cardSpan = 0.8f;
        public int NumberOfCards => _handCards.Count;
        public Vector3 offset = new(-135f, 0f, 0f);

        private Vector3 _center;
        
        #endregion

        [SerializeField] private GameObject[] cardPrefabs;
        private readonly List<Card> _handCards = new();
        private int _selectedCardIdx = -1;

        public Card CurrentlySelectedCard => _selectedCardIdx != -1 ? _handCards[_selectedCardIdx] : null;
        public bool CanSelect { get; set; }
        private bool _thumbOnTouchpad;

        public UnityEvent<Card> onCardSelectionChanged;

        private void Start() => onCardSelectionChanged.AddListener(HighlightCardOnSelection);
       

        /// <summary>
        /// Sets the currently selected hand card with thumb selector.
        /// </summary>
        /// <param name="value"></param>
        private void SetSelectedCard(int value)
        {
            if (value == _selectedCardIdx)
                return;

            _selectedCardIdx = value;
            onCardSelectionChanged?.Invoke(CurrentlySelectedCard);
        }
        
        
        /// <summary>
        /// Add a card to the hand of the player.
        /// </summary>
        /// <param name="value"></param>
        public void AddCard(CardValues value)
        {
            var newCard = Instantiate(cardPrefabs[(int)value - 1], parent: this.transform);
            var card = newCard.GetComponent<Card>();
            card.steps = value;
            _handCards.Add(card);
            
            ApplyPositionsToCards(_handCards.Select(c => c.transform).ToList());
            SetSelectedCard(-1);
        }

        /// <summary>
        /// Remove a card from the hand cards of the player.
        /// </summary>
        /// <param name="card">The card to remove</param>
        public void DeleteCard(Card card)
        {
            _handCards.Remove(card);
            Destroy(card.gameObject);
            ApplyPositionsToCards(_handCards.Select(c=>c.transform).ToList());
            SetSelectedCard(-1);
        }

        #region Event Functions

        /// <summary>
        /// Subscribes to <see cref="LeftHand"/>.onTouchpadTouchedChanged and deselects selected cards if thumb isn't on the pad.
        /// </summary>
        /// <param name="leftHand"></param>
        /// <param name="touched"></param>
        public void OnTouchpadTouchedChanged(LeftHand leftHand, bool touched)
        {
            _thumbOnTouchpad = touched;
            if (!touched) SetSelectedCard(-1);
        }
        
        /// <summary>
        /// Subscribes to <see cref="LeftHand"/>.onTouchpadChanged and set's the selected card based on the x pos of the touchpad pos.
        /// </summary>
        /// <param name="leftHand"></param>
        /// <param name="vec"></param>
        public void OnTouchpadMoved(LeftHand leftHand, Vector2 vec)
        {
            if (NumberOfCards == 0 || !_thumbOnTouchpad || !CanSelect)
            {
                SetSelectedCard(-1);
                return;
            }

            if (NumberOfCards == 1)
            {
                SetSelectedCard(0);
                return;
            }

            var x = (-vec.x + 1) / 2;
            
            var border = 1 / (float)NumberOfCards;
            var borders = new float[NumberOfCards];

            for (var i = 0; i < NumberOfCards; i++)
                borders[i] = Mathf.Abs(x - border * i - border / 2);
            var selectedCard = Array.IndexOf(borders, borders.Min());
            SetSelectedCard(selectedCard);
        }
        
        /// <summary>
        /// Subscribes to <see cref="onCardSelectionChanged"/> to highlight the selected card.
        /// </summary>
        /// <param name="card"></param>
        private void HighlightCardOnSelection(Card card)
        {
            foreach (var handCard in _handCards)
                handCard.Dim();
            if (card != null) card.Highlight();
        }

        #endregion
        
        private void OnDestroy() => onCardSelectionChanged.RemoveAllListeners();
        

        #region Positioning Math

        private Vector3[] GetDirectionsOnArc()
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

        private Vector3[] GetPointsOnArc()
        {
            _center = transform.position - transform.forward * (radius - height);
            return GetDirectionsOnArc()
                .Select(direction => _center + direction * radius)
                .ToArray();
        }

        private void ApplyPositionsToCards(IReadOnlyList<Transform> transforms)
        {
            var directions = GetDirectionsOnArc();
            var positions = GetPointsOnArc();
            for (var i = 0; i < directions.Length; i++)
            {
                transforms[i].right = -directions[i];
                transforms[i].localEulerAngles += offset;
                transforms[i].position = positions[i];
                transforms[i].localPosition += new Vector3(0f, -0.0002f * i, 0f);
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
