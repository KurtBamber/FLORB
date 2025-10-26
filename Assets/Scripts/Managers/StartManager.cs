using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    [SerializeField] private Transform mainPal;
    [SerializeField] private Transform[] otherPals;   
    [SerializeField] private GameObject road;         
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float scatterForce = 5f;
    [SerializeField] private float pauseBeforeFall = 0.5f;

    void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        Vector3 target = new Vector3(3, 41, 0);

        while (Vector3.Distance(mainPal.position, target) > 0.1f)
        {
            mainPal.position = Vector3.MoveTowards(mainPal.position, target, moveSpeed * Time.deltaTime);
            foreach (var pal in otherPals)
            {
                pal.position = Vector3.MoveTowards(pal.position, target, moveSpeed * Time.deltaTime);
            }
            yield return null;
        }

        if (road != null) road.SetActive(false);

        yield return new WaitForSeconds(pauseBeforeFall);

        foreach (var pal in otherPals)
        {
            Rigidbody2D rb = pal.GetComponent<Rigidbody2D>();
            if (rb == null) rb = pal.gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = false;
            rb.AddForce(Random.insideUnitCircle.normalized * scatterForce, ForceMode2D.Impulse);
        }

        Vector3 downTarget = new Vector3(3, -3, 0);
        while (Vector3.Distance(mainPal.position, downTarget) > 0.1f)
        {
            mainPal.position = Vector3.MoveTowards(mainPal.position, downTarget, moveSpeed * 2 * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        if (fadeCanvas != null)
        {
            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime;
                fadeCanvas.alpha = t;
                yield return null;
            }
        }
        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene("SampleScene");
    }
}
