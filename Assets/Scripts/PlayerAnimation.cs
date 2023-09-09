using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum PlayerAnim
    {
        Run,
        Jump,
        Down,
        Dance
    }
    public static PlayerAnimation inst;
    private Animator animator;
    private void Start()
    {
        inst = this;
        animator = GetComponent<Animator>();
        Observer.StartGame += () => SetAnim(PlayerAnim.Run);
        Observer.EndGame += (x) => { if (x) SetAnim(PlayerAnim.Dance); };
        Observer.PlayerAnimJump += () => SetAnim(PlayerAnim.Jump);
        Observer.PlayerAnimDown += () => SetAnim(PlayerAnim.Down);
    }
    public void SetAnim(PlayerAnim playerAnim)
    {
        Reset();
        animator.SetBool(Enum.GetName(typeof(PlayerAnim), playerAnim), true);

    }
    private void Reset()
    {
        PlayerAnim[] allPlayerAnim = (PlayerAnim[])Enum.GetValues(typeof(PlayerAnim));

        foreach (PlayerAnim playerAnim in allPlayerAnim)
        {
            animator.SetBool(Enum.GetName(typeof(PlayerAnim), playerAnim), false);
        }
    }
    private void OnDestroy()
    {
        Observer.EndGame -= (x) => { if (x) SetAnim(PlayerAnim.Dance); };
        Observer.StartGame -= () => SetAnim(PlayerAnim.Run);
        Observer.PlayerAnimJump -= () => SetAnim(PlayerAnim.Jump);
        Observer.PlayerAnimDown -= () => SetAnim(PlayerAnim.Down);
    }
}
