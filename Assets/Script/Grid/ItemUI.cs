using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public GameObject furniturePanel;
    public GameObject tilePanel;

    void Start()
    {
        clickFurnitureMenu();
    }

    public void clickFurnitureMenu()
    {
        furniturePanel.SetActive(true);
        tilePanel.SetActive(false);
    }

    public void clickTilePanel()
    {
        furniturePanel.SetActive(false);
        tilePanel.SetActive(true);
    }
}
