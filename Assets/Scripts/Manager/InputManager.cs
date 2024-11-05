using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputType
{
    Fire, Zoom, Reload ,Pause
}

public class InputManager : PersistentMonoSingleton<InputManager>
{
    public SerializedDictionary<InputType, InputActionReference> inputActions = new SerializedDictionary<InputType, InputActionReference>();

}
