using UnityEngine;

public static class Timer
{
    private static float time;
    private static bool running;

    public static void StartTimer()
    {
        time = 0f;
        running = true;
    }

    public static void StopTimer()
    {
        running = false;
    }

    public static void PauseTimer()
    {
        running = false;
    }

    public static void ResumeTimer()
    {
        running = true;
    }

    public static void ResetTimer()
    {
        time = 0f;
        running = false;
    }

    public static void UpdateTimer()
    {
        if (running)
            time += Time.deltaTime;
    }

    public static float GetTime()
    {
        return time;
    }
}
