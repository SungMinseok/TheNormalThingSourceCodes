#if UNITY_ANDROID// && !UNITY_EDITOR
#define ISANDROID
#endif
using UnityEngine;
using System.Collections;
 //https://malbongcode.tistory.com/41
public static class Vibration
{
 
#if ISANDROID
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
//#else
    // public static AndroidJavaClass unityPlayer;
    // public static AndroidJavaObject currentActivity;
    // public static AndroidJavaObject vibrator;
//#endif
 
    public static void Vibrate()
    {
        if (isAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }
 
 
    public static void Vibrate(long milliseconds)
    {
        if (isAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }
 
    public static void Vibrate(long[] pattern, int repeat)
    {
        if (isAndroid())
            vibrator.Call("vibrate", pattern, repeat);
        else
            Handheld.Vibrate();
    }
 
    public static bool HasVibrator()
    {
        return isAndroid();
    }
 
    public static void Cancel()
    {
        if (isAndroid())
            vibrator.Call("cancel");
    }
 
    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
#endif
}
