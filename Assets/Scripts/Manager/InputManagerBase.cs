using System;
using AYellowpaper.SerializedCollections;
using UnityCommunity.UnitySingleton;
using UnityEngine.InputSystem;

public abstract class InputManagerBase<TInputType, TInstance> : MonoSingleton<TInstance> where TInputType : Enum where TInstance : InputManagerBase<TInputType, TInstance>, new()
{
    public SerializedDictionary<TInputType, InputActionReference> inputActions = new SerializedDictionary<TInputType, InputActionReference>();
}
