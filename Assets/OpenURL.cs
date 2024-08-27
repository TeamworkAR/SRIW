using System.Runtime.InteropServices;
using UnityEngine;

public class OpenURLButton : MonoBehaviour
{
    public static void OpenURL(string url)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        OpenTab(url);
#endif
    }

    [DllImport("__Internal")]
    private static extern void OpenTab(string url);
}
