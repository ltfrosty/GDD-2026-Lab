using UnityEngine;
using UnityEngine.Rendering;

// URP's Rendering Debugger spawns a "[Debug Updater]" object with DontDestroyOnLoad in the Editor and
// development builds. We don't use the runtime debug UI, so switch it off before the first scene loads.
public static class DisableRenderingDebuggerUI
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Disable()
    {
        DebugManager.instance.enableRuntimeUI = false;
    }
}
