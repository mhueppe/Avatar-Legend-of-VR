using System.Collections;
using UnityEngine;
using VRAvatar;

public class RefillHandCards : IState
{
    private readonly TrialInfo _currentTrial;
    private readonly VRPlayer _player;
    private readonly float _every;
    private readonly int _amount;
    private readonly CardStaple _staple;
    
    private int _refilled;
    
    public bool AreRefilled => _refilled == _amount;


    public RefillHandCards(CardStaple staple, VRPlayer player, int amount, float every)
    {
        _staple = staple;
        _player = player;
        _amount = amount;
        _every = every;
    }

    public void OnStateEnter()
    {
        _refilled = 0;
        _player.StartCoroutine(Refill());
    }
    
    /// <summary>
    /// Refills one hand card every <see cref="_every"/> seconds until amount is 0.
    /// </summary>
    private IEnumerator Refill()
    {
        while (_refilled < _amount) // refill cards as long as _amount is larger than 0.
        {
            var card = _staple.GetNextCardFromStaple();
            _player.leftHand.handCards.AddCard(card);
            _refilled++;
            yield return new WaitForSeconds(_every);
        }
    }

    public void Tick() { }
    
    public void OnStateExit()
    {
        _player.StopCoroutine(Refill());
    }
}