using UnityEngine;
public class Animation : MonoBehaviour
{
    public float scaleFactor = 1.5f; 
    public float duration;

    private Vector3 initialScale;
    private Vector3 targetScale; 

    private bool isAnimating; 
    private float elapsedTime; 

    private void Start()
    {
        initialScale = transform.localScale;
        targetScale = initialScale * scaleFactor;
    }

    private void Update()
    {
        if (isAnimating)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= duration)
            {
                elapsedTime = duration;
                isAnimating = false;
            }

            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(targetScale, initialScale, t); //punch scale
        }
    }

    public void PlayAnimation()
    {
        if (!isAnimating)
        {
            isAnimating = true;
            elapsedTime = 0f;
            transform.localScale = targetScale;
        }
    }
}