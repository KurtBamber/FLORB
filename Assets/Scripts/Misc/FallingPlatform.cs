using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 2.0f;
    [SerializeField] private float disappearDelay = 2.0f;
    [SerializeField] private float respawnDelay = 3.0f;     

    [Header("Audio")]
    [SerializeField] private AudioClip fallClip;
    [SerializeField] private float minVolume = 0.3f;
    [SerializeField] private float maxVolume = 1.0f;

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        startPos = transform.position;
        startRot = transform.rotation;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallCycle());
        }
    }

    IEnumerator FallCycle()
    {
        float shakeDuration = fallDelay;
        float shakeMagnitude = 0.05f;
        float shakeSpeed = 40f;

        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float x = Mathf.Sin(elapsed * shakeSpeed) * shakeMagnitude;
            transform.position = originalPos + new Vector3(x, 0f, 0f);
            yield return null;
        }

        transform.position = originalPos;

        if (fallClip != null)
            AudioSource.PlayClipAtPoint(fallClip, transform.position, Random.Range(minVolume, maxVolume));

        rb.constraints = RigidbodyConstraints2D.None;
        rb.isKinematic = true;
        rb.isKinematic = false;
        col.enabled = false;

        yield return new WaitForSeconds(disappearDelay);

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        transform.position = startPos;
        transform.rotation = startRot;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        col.enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;
    }
}
