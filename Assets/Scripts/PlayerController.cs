using System.Collections;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveDuration = 0.15f;
    [SerializeField] private float hopHeight = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask climbLayer;
    private bool isMoving;
    private Vector3 targetPos;

    [Header("References")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameManager gameManager;
    private PalManager palManager;

    private void Awake()
    {
        palManager = GetComponent<PalManager>();
        targetPos = transform.position;
    }

    private void Update()
    {
        if (isMoving || gameManager.IsEnded) return;

        HandleInput();

        if (!IsGrounded())
        {
            StartCoroutine(ApplyGravity());
            return;
        }

    }

    private void HandleInput()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W)) direction = Vector3.up;
        if (Input.GetKeyDown(KeyCode.A)) direction = Vector3.left;
        if (Input.GetKeyDown(KeyCode.S)) direction = Vector3.down;
        if (Input.GetKeyDown(KeyCode.D)) direction = Vector3.right;

        if (direction != Vector3.zero)
            TryMove(direction);
    }

    private void TryMove(Vector3 direction)
    {
        Vector3 newPos = targetPos + direction;

        if (palManager.IsOccupied(newPos))
        {
            StartCoroutine(CollisionFeedback(direction));
            return;
        }

        if (!Physics2D.OverlapCircle(newPos, 0.3f, climbLayer) && Physics2D.OverlapCircle(newPos, 0.3f, groundLayer))
        {
            StartCoroutine(CollisionFeedback(direction));
            return;
        }

        StartCoroutine(Move(direction));
    }

    private IEnumerator Move(Vector3 direction)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction;
        palManager.ChangePalsTargetPosition(endPos);

        var palsStartPos = palManager.GetCurrentPalPositions();
        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / moveDuration);

            float headHop = Mathf.Sin(progress * Mathf.PI) * hopHeight;
            transform.position = Vector3.Lerp(startPos, endPos, progress) + Vector3.up * headHop;

            palManager.MovePals(palsStartPos, progress, hopHeight * 0.8f);
            yield return null;
        }

        transform.position = endPos;
        palManager.CompleteMove(endPos);

        targetPos = endPos;
        isMoving = false;
    }

    IEnumerator ApplyGravity()
    {
        isMoving = true;
        while (!IsGrounded())
        {
            transform.position += Vector3.down * 0.5f;
            palManager.FallDown(Vector3.down * 0.5f);
            yield return new WaitForSeconds(0.1f);
        }
        targetPos = transform.position;
        isMoving = false;
    }

    private bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(transform.position + Vector3.down * 0.5f, 0.3f, groundLayer | climbLayer)) return true;
        foreach (Transform pal in palManager.pals)
            if (Physics2D.OverlapCircle(pal.position + Vector3.down * 0.5f, 0.3f, groundLayer | climbLayer)) return true;
        return false;
    }

    private IEnumerator CollisionFeedback(Vector3 direction)
    {
        isMoving = true;
        Vector3 start = transform.position;
        Vector3 collision = start + direction * 0.2f;

        float t = 0f;
        while (t < 0.1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, collision, t / 0.1f);
            yield return null;
        }

        t = 0f;
        while (t < 0.1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(collision, start, t / 0.1f);
            yield return null;
        }

        transform.position = start;
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pal"))
        {
            Destroy(collision.gameObject);
            palManager.AddPal();
            audioManager.PlayPickupSound(transform.position);
        }
        else if (collision.CompareTag("End"))
        {
            gameManager.End();
        }
    }
}
