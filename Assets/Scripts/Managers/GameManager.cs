using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject endScreen;
    [SerializeField] private Button resetBtn;
    private Vector3 startPos = new Vector3(3, -3, 0);
    [HideInInspector] public bool IsEnded;

    private PlayerController playerController;
    private PalManager palManager;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        palManager = FindAnyObjectByType<PalManager>();
        resetBtn.onClick.AddListener(ResetLevel);
    }

    public void End()
    {
        endScreen.SetActive(true);
        IsEnded = true;
    }

    public void ResetLevel()
    {
        IsEnded = false;
        endScreen.SetActive(false);

        playerController.transform.position = startPos;

        for (int i = 0; i < palManager.pals.Count; i++)
        {
            palManager.pals[i].position = startPos + Vector3.left * (i + 1);
        }
    }
}
