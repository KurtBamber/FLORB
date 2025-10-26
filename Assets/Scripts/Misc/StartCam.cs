using UnityEngine;

public class StartCam : MonoBehaviour
{
    [SerializeField] private Transform target;   
    [SerializeField] private float smoothSpeed = 5f;
    private Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }
}
