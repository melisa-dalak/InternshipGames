using UnityEngine;

public class SliderMove : MonoBehaviour
{
    public Transform target;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position - target.up;
            transform.rotation = target.rotation;
        }
        else
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }
}
