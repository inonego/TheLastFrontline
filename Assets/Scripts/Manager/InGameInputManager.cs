using AYellowpaper.SerializedCollections;
using UnityCommunity.UnitySingleton;
using UnityEngine.InputSystem;

public class InGameInputManager : InputManagerBase<InGameInputManager.InputType, InGameInputManager>
{
    public enum InputType
    {
        Fire, Zoom, Reload, Pause
    }
}
