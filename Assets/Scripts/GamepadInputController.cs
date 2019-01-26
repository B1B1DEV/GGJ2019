using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadInputController : InputController
{
    public override float getWheelInput()
    {
        return Input.GetAxis("Wheel");
    }

    public override Vector2 getHeadInput()
    {
        return new Vector2(Input.GetAxis("HeadHorizontal"), Input.GetAxis("HeadVertical"));
    }

    public override bool isBreaking()
    {
        return Input.GetButton("Brake");
    }
}
