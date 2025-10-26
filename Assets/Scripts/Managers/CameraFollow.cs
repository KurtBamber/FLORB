using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform mainPal;
    [SerializeField] private Vector3 velocity = Vector3.zero;

    [Header("Screen Shake")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.3f;

    private Vector3 originalPos;
    private float currentShakeTime = 0f;

    void Start()
    {
        originalPos = transform.position;
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        float yThreshold = 1.1f;

        Vector3 targetPos = new Vector3(2.5f, transform.position.y, transform.position.z);
        Vector3 desiredPos = new Vector3(2.5f, mainPal.position.y + 2f, transform.position.z);

        if (Mathf.Abs(desiredPos.y - transform.position.y) > yThreshold)
        {
            targetPos.y = desiredPos.y;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 0.3f);

        if (currentShakeTime > 0)
        {
            transform.position += Random.insideUnitSphere * shakeMagnitude;
            transform.position = new Vector3(2.5f, transform.position.y, originalPos.z);
            currentShakeTime -= Time.deltaTime;
        }
    }

    public void TriggerShake(float magnitude, float duration)
    {
        shakeMagnitude = magnitude;
        shakeDuration = duration;
        currentShakeTime = duration;
    }
}
