using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo 
{
    static int gridSize = 5;
    static int startGrid = -200;

    static public int getGridSize(){ return gridSize;}
    static public int getStartGrid(){ return startGrid;}
    static public void setTileGrid() {
        gridSize = 20;
        
    }
    static public void setFurnitureGrid() { gridSize = 5;}
    

    //ÀÇÁ¸: PlaneGrid, Furniture
}
