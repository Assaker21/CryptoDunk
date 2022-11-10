using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] ParticleSystem smokeParticleSystem;
    [SerializeField] ParticleSystem fireParticleSystem;
    [SerializeField] BasketManager basketManager;

    public void OnJump()
    { 
    
    }

    public void OnScore()
    {
        smokeParticleSystem.Stop();
        fireParticleSystem.Stop();
    }

    public void OnSwishScore()
    {
        smokeParticleSystem.Play();
        fireParticleSystem.Stop();
    }

    public void OnSwishScoreTwice()
    {
        smokeParticleSystem.Play();
        fireParticleSystem.Play();
    }

    public void OnSwishScoreThrice()
    { 
    
    }

    public void OnSwishScoreEndless()
    { 
    
    }

    public void OnMiss()
    {
        smokeParticleSystem.Stop();
        fireParticleSystem.Stop();
    }

	public void OnHit()
	{
		
	}

    public void OnHitUnscoredBasket()
    {
        smokeParticleSystem.Stop();
        fireParticleSystem.Stop();
    }

    public void OnBasketScoreNormal(int index)
    {
        basketManager.baskets[index].OnBasketScoreNormal();
    }

    public void OnBasketScoreSwish(int index)
    {
        basketManager.baskets[index].OnBasketScoreSwish();
    }

    public void OnBasketRespawn(int index)
    {
        basketManager.baskets[index].OnBasketRespawn();
    }
}
