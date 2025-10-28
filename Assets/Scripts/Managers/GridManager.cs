using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private GameObject tilePrefab;
    private Dictionary<Vector2Int, GameObject> placedTiles = new();
    private Vector2Int hoveredTile;

    private void Update()
    {
        Vector3 mouseToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseToWorld.z = 0f;
        hoveredTile = WorldToGrid(mouseToWorld);

        if (Input.GetMouseButtonDown(0))
            PlaceTile(hoveredTile);

        if (Input.GetMouseButtonDown(1))
            RemoveTile(hoveredTile);
    }

    private void PlaceTile(Vector2Int tile)
    {
        if (placedTiles.ContainsKey(tile))
        {
            Debug.Log("Tile already exists here!");
            return;
        }

        Vector3 worldPos = GridToWorld(tile);
        GameObject newTile = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
        placedTiles[tile] = newTile;
    }

    private void RemoveTile(Vector2Int tile)
    {
        if(!placedTiles.ContainsKey(tile)) return;

        Destroy(placedTiles[tile]);
        placedTiles.Remove(tile);
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / tileSize);
        int y = Mathf.RoundToInt(worldPos.y / tileSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * tileSize, gridPos.y * tileSize, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                Vector3 pos = new Vector3(x * tileSize, y * tileSize, 0);
                Gizmos.DrawWireCube(pos, new Vector3(1, 1, 0) * tileSize);
            }
        }

        Gizmos.color = Color.yellow;
        Vector3 hoveredToWorld = GridToWorld(hoveredTile);
        Gizmos.DrawWireCube(hoveredToWorld, new Vector3(1, 1, 0) * tileSize);
    }

    public bool IsTileOccupied(Vector2Int gridPos)
    {
        return placedTiles.ContainsKey(gridPos);
    }
}
