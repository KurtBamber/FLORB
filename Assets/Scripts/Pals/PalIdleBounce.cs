using System.Collections;
using UnityEngine;

public class PalIdleBounce : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 0.5f;   
    [SerializeField] private float jumpDuration = 0.2f; 
    [SerializeField] private int jumpsPerCycle = 2;     
    [SerializeField] private float minWait = 1f;       
    [SerializeField] private float maxWait = 3f;        

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(JumpLoop());
    }

    private IEnumerator JumpLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minWait, maxWait);
            yield return new WaitForSeconds(waitTime);

            for (int i = 0; i < jumpsPerCycle; i++)
            {
                yield return StartCoroutine(MoveTo(startPos + Vector3.up * jumpHeight, jumpDuration));
                yield return StartCoroutine(MoveTo(startPos, jumpDuration));
            }
        }
    }

    private IEnumerator MoveTo(Vector3 target, float duration)
    {
        Vector3 initial = transform.position;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(initial, target, t / duration);
            yield return null;
        }

        transform.position = target;
    }
}
