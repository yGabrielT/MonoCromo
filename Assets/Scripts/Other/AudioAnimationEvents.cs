using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioAnimationEvents : MonoBehaviour
{
    [SerializeField] private ThirdPersonController _third;
    
    
    public void EventOnFootstep(AnimationEvent animationEvent)
    {
        _third.OnFootstep(animationEvent);
    }
    public void EventOnLand(AnimationEvent animationEvent)
    {
        _third.OnLand(animationEvent);
    }
    public void EventOnRunFootStep(AnimationEvent animationEvent)
    {
        
    }
}
