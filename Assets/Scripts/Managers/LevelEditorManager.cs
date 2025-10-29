using UnityEngine;

[System.Serializable]public class TileCategory
{
    public string categoryName;
    public GameObject[] variants;
}

public class LevelEditorManager : MonoBehaviour
{
    [SerializeField] private TileCategory[] tileCategories;
    private int selectedCategoryIndex = 0;
    private int selectedVariantIndex = 0;

    private void Update()
    {
        HandleTileSelection();
    }

    private void HandleTileSelection()
    {
        if (Input.mouseScrollDelta.y > 0)
            selectedVariantIndex = (selectedVariantIndex + 1) % tileCategories[selectedCategoryIndex].variants.Length;

        if (Input.mouseScrollDelta.y < 0)
            selectedVariantIndex = (selectedVariantIndex - 1 + tileCategories[selectedCategoryIndex].variants.Length) % tileCategories[selectedCategoryIndex].variants.Length;
    }

    public GameObject GetSelectedTile()
    {
        return tileCategories[selectedCategoryIndex].variants[selectedVariantIndex];
    }
}
