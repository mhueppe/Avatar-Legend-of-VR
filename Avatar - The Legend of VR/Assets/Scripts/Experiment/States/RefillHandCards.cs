using System.Collections;
using UnityEngine;
using Avatar = VRAvatar.Avatar;

public class RefillHandCards : IState
{
    private readonly TrialInfo _currentTrial;
    private readonly Avatar _avatar;
    private readonly float every;
    
    
    private CardStaple _staple;
    private int _amount = 1;
    private bool _init = false;
    
    public bool AreRefilled => _amount <= 0;


    public RefillHandCards(CardStaple staple, Avatar avatar, int amount, float every)
    {
        _staple = staple;
        _avatar = avatar;
        _amount = amount;
        this.every = every;
        _init = true;
    }
    
    public RefillHandCards(TrialInfo currentTrial, Avatar avatar, float every)
    {
        _currentTrial = currentTrial;
        this.every = every;
        _avatar = avatar;
    }

    public void OnStateEnter()
    {
        Debug.Log("Entered Refill hand");
        if (!_init)
        {
            _amount = _currentTrial.cardInformation.numberOfHandCardsAtStart;
            _staple = _currentTrial.cardInformation.cardStaple;
            _init = true;
        }
            
        _avatar.StartCoroutine(Refill());
    }

    /// <summary>
    /// Refills one hand card every <see cref="every"/> seconds until amount is 0.
    /// </summary>
    private IEnumerator Refill()
    {
        while (_amount > 0) // refill cards as long as _amount is larger than 0.
        {
            _avatar.leftHand.CardPositioner.AddCard(_staple.GetNextCardFromStaple());
            _amount--;
            yield return new WaitForSeconds(every);
        }
    }

    public void Tick() { }
    

    /// <summary>
    /// Use this to method to reinitialize this class to use again.
    /// </summary>
    /// <param name="amount">How many cards will be refilled.</param>
    /// <returns>this</returns>
    public RefillHandCards Refill(int amount)
    {
        _amount = amount;
        return this;
    }

    public void OnStateExit()
    {
        _avatar.StopCoroutine(Refill());
        Debug.Log("Exited Refill hand");
    }
}