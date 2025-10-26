using UnityEngine;
using System.Collections;

public class PalBlinking : MonoBehaviour
{
    [Header("Blink Settings")]
    [SerializeField] private Transform leftEye;    
    [SerializeField] private Transform rightEye;   
    [SerializeField] private float minInterval = 2f; 
    [SerializeField] private float maxInterval = 6f; 
    [SerializeField] private float blinkDuration = 0.1f; 
    [SerializeField, Range(0.05f, 0.5f)] private float minScaleY = 0.2f; 

    private Vector3 leftEyeStartScale;
    private Vector3 rightEyeStartScale;

    void Start()
    {
        leftEyeStartScale = leftEye.localScale;
        rightEyeStartScale = rightEye.localScale;

        StartCoroutine(BlinkLoop());
    }

    private IEnumerator BlinkLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            leftEye.localScale = new Vector3(leftEyeStartScale.x, leftEyeStartScale.y * minScaleY, leftEyeStartScale.z);
            rightEye.localScale = new Vector3(rightEyeStartScale.x, rightEyeStartScale.y * minScaleY, rightEyeStartScale.z);

            yield return new WaitForSeconds(blinkDuration);

            leftEye.localScale = leftEyeStartScale;
            rightEye.localScale = rightEyeStartScale;
        }
    }
}
