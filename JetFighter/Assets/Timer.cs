using System;
using UnityEngine;

[Serializable]
public class Timer
{
    public float duration;
    public bool done;
    private float elapsedTime;

    public Timer(float duration)
    {
        this.duration = duration;
        done = false;
    }
   
    public void Update()
    {
        if (!done)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration)
            {
                done = true ;
                elapsedTime = duration;
            }
        }
    }
    public float GetRemainingTime()
    {
        if (!done)
        {
            return Mathf.Max(duration - elapsedTime, 0f);
        }
        else
        {
            return 0f;
        }
    }

   public void ReStart()
    {
        done = false;
        elapsedTime = 0f;
    }
    public bool isDone()
    {
        return (done);
    }

    public void ForceComplete()
    {
        done = true;
    }
}