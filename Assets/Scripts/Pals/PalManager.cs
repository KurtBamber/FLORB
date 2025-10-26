using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PalManager : MonoBehaviour
{
    [SerializeField] private Transform palPrefab;
    [HideInInspector] public List<Transform> pals = new();
    private List<Vector3> positions = new();
    private Vector3 lastPalPos;

    private void Start()
    {
        positions.Add(transform.position);
    }

    public bool IsOccupied(Vector3 pos)
    {
        foreach (var pal in pals)
            if (pal.position == pos) return true;
        return false;
    }

    public void AddPal()
    {
        Vector3 spawnPos;
        if (pals.Count == 0) spawnPos = positions[positions.Count - 1] + Vector3.right;
        else spawnPos = lastPalPos;
        Transform newPal = Instantiate(palPrefab, spawnPos, Quaternion.identity);
        pals.Add(newPal);
        positions.Add(newPal.position);
        lastPalPos = newPal.position;
    }

    public void ChangePalsTargetPosition(Vector3 newHeadPos)
    {
        positions.Insert(0, newHeadPos);
    }

    public List<Vector3> GetCurrentPalPositions()
    {
        List<Vector3> currentPal = new();
        foreach (var pal in pals)
            currentPal.Add(pal.position);

        if (currentPal.Count == 0)
            currentPal.Add(transform.position);

        return currentPal;
    }

    public void MovePals(List<Vector3> startPositions, float progress, float hopHeight)
    {
        int count = Mathf.Min(pals.Count, startPositions.Count);
        for (int i = 0; i < count; i++)
        {
            Vector3 start = startPositions[i];
            Vector3 end;
            if (i + 1 < positions.Count)
                end = positions[i + 1];
            else end = start;
            float hop = Mathf.Sin(progress * Mathf.PI) * hopHeight;
            pals[i].position = Vector3.Lerp(start, end, progress) + Vector3.up * hop;
        }
    }

    public void CompleteMove(Vector3 headEndPos)
    {
        for (int i = 0; i < pals.Count; i++)
        {
            if (i + 1 < positions.Count)
                pals[i].position = positions[i + 1];
        }

        while (positions.Count > pals.Count)
        {
            lastPalPos = positions[positions.Count - 1];
            positions.RemoveAt(positions.Count - 1);
        }
    }

    public void FallDown(Vector3 offset)
    {
        for (int i = 0; i < pals.Count; i++)
        {
            pals[i].position += offset;
        }

        if (positions.Count == 0)
            positions.Add(transform.position);
        else
            positions[0] = transform.position;

        for (int i = 0; i < pals.Count; i++)
        {
            if (i + 1 < positions.Count)
                positions[i + 1] = pals[i].position;
            else
                positions.Add(pals[i].position);
        }
    }
}
