using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;
    private float shakeDure = 1f, shakeAmount = 0.04f, decreaseFactor = 1.5f;

    private Vector3 originPosition;
    private void Start()
    {
        camTransform = GetComponent<Transform>();
        originPosition = camTransform.localPosition;

    }
    private void Update()
    {
        if (shakeDure > 0)
        {
            camTransform.localPosition = originPosition + Random.insideUnitSphere * shakeAmount;
            shakeDure -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            shakeDure = 0;
            camTransform.localPosition = originPosition;
        }
    }
}
