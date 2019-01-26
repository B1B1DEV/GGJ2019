using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : MonoBehaviour
{
    public abstract float getWheelInput();
    public abstract Vector2 getHeadInput();
    public abstract bool isBreaking();
}
