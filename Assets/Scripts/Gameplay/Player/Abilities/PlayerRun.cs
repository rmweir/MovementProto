﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : PlayerAbility
{
    // Parameters
    [SerializeField] float runSpeed = 0.2f;
    [SerializeField] float floatSpeed = 0.1f;
    [SerializeField] float accelerationTimeAirborne = 0.2f;

    float direction;
    float smoothing;

    // Constructor
    public PlayerRun(PlayerController p) : base(p, false)
    {
        permitted = true;
        input = true;
    }

    public override void CheckInput()
    {
        direction = Input.GetAxisRaw("Horizontal");
    }

    public override void CheckPermitted()
    {

    }

    protected override void DoAbility()
    {
        if (direction != 0)
        {
            player.state.facing = (direction == 1) ? true : false;
        }

        if (player.surfaceCollsions.Collisions.below)
        {
            if (!player.abilities.dodge.doing)
            {
                player.state.velocity.x = direction * runSpeed;
            }
        }
        else // airborne
        {
            // TODO currently this slows down player to floatspeed, even if going fast (like jumping)
            // Add checks to see if current velocity exceeds floatspeed. If so, don't smoothdamp
            float targetVelocityX = direction * floatSpeed;
            player.state.velocity.x = Mathf.SmoothDamp(player.state.velocity.x, targetVelocityX, ref smoothing, accelerationTimeAirborne);
        }
    }
}
