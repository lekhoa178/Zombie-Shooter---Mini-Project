using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventTrigger : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void TriggerOnMovementStateAnimationEnterEvent()
    {
        player.OnMovementStateAnimationEnterEvent();
    }

    public void TriggerOnMovementStateAnimationExitEvent()
    {
        player.OnMovementStateAnimationExitEvent();
    }

    public void TriggerOnMovementStateAnimationTransitionEvent()
    {
        player.OnMovementStateAnimationTransitionEvent();
    }

    private bool IsAnimationTransition(int layerIndex = 0)
    {
        return player.Animator.IsInTransition(layerIndex);
    }
}
