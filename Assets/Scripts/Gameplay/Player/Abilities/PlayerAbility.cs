﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbility
{
    public bool doing;          // Ability is in use at this moment
    public bool input;          // Player is attempting to use ability
    protected bool coroutineUsed; // Does the ability make use of a coroutine
    protected bool active;      // Player has obtained this ability
    protected bool permitted;   // Ability is permitted at this moment
    protected bool cooling;     // Ability is currently cooling down
    protected float cooldown;   // Time between uses

    // Allows access to state information
    protected PlayerController player;

    protected PlayerAbility(PlayerController p, bool c)
    {
        player = p;
        coroutineUsed = c;
    }

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }

    public virtual void Perform()
    {
        if (!active || !permitted || !input || doing || cooling) return;

        DoAbility();
        if (coroutineUsed)
        {
            IEnumerator coroutine = DoAbilityCoroutine();
            player.StartCoroutine(coroutine);
        }
        Cooldown();
    }

    public abstract void CheckPermitted();
    public abstract void CheckInput();
    protected virtual void DoAbility()
    {

    }
    protected virtual IEnumerator DoAbilityCoroutine()
    {
        yield return null;
    }
    protected virtual void Cooldown()
    {
        if (cooldown != 0)
        {
            IEnumerator coroutine = CooldownLoop();
            player.StartCoroutine(coroutine);
        }
    }

    private IEnumerator CooldownLoop()
    {
        cooling = true;
        yield return new WaitForSeconds(cooldown);
        cooling = false;
    }
}
