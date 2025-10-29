using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]public class TileCategory
{
    public string categoryName;
    public GameObject[] variants;
}

public class LevelEditorManager : MonoBehaviour
{
    [SerializeField] private TileCategory[] tileCategories;
    [SerializeField] private Transform categoryBar;
    [SerializeField] private GameObject categoryButtonPrefab;
    private int selectedCategoryIndex = 0;
    private int selectedVariantIndex = 0;
    private List<Button> categoryButtons = new();

    private void Start()
    {
        GenerateCategoryButtons();
        UpdateCategoryVisuals();
    }

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

    private void GenerateCategoryButtons()
    {
        foreach (var category in tileCategories)
        {
            GameObject buttonObj = Instantiate(categoryButtonPrefab, categoryBar);
            Button button = buttonObj.GetComponent<Button>();

            Image img = buttonObj.GetComponentInChildren<Image>();
            if (category.variants.Length > 0)
            {
                Sprite categorySprite = category.variants[0].GetComponent<SpriteRenderer>().sprite;
                img.sprite = categorySprite;
            }

            int index = categoryButtons.Count;
            button.onClick.AddListener(() => SelectCategory(index));
            categoryButtons.Add(button);
        }
    }

    private void SelectCategory(int index)
    {
        selectedCategoryIndex = index;
        selectedVariantIndex = 0;
        UpdateCategoryVisuals();
    }

    private void UpdateCategoryVisuals()
    {
        for (int i = 0; i < categoryButtons.Count; i++)
        {
            Image img = categoryButtons[i].GetComponent<Image>();
            if (i == selectedCategoryIndex)
                img.color = Color.yellow;
            else
                img.color = Color.white;
        }
    }
}
