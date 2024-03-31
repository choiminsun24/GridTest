using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGrid : MonoBehaviour
{
    public LineRenderer Ir;
    public float startc, startr;
    public int rowCount, colCount;
    public float gridSize;

    void OnValidate()
    {
        if (gridSize > 0 && rowCount > 0 && colCount > 0)
            makeGrid(Ir, startr, startc, rowCount, colCount);
    }

    void initLineRenderer(LineRenderer Ir) //Ir 정의
    {
        Ir.startWidth = Ir.endWidth = 0.35f; //라인 두께
        Ir.material.color = Color.green;    //라인 색상
    }

    void makeGrid(LineRenderer Ir, float startr, float startc, int rowCount, int colCount)
    {
        List<Vector3> gridPos = new List<Vector3>();

        float endc = startc + colCount * gridSize;

        gridPos.Add(new Vector3(startr, this.transform.position.y, startc));
        gridPos.Add(new Vector3(startr, this.transform.position.y, endc)); //2번째 점

        int direction = -1;
        Vector3 currentPos = new Vector3(startr, this.transform.position.y, endc); //2번째 점
        for (int i = 0; i <rowCount; i++)
        {
            currentPos.x += gridSize;
            gridPos.Add(currentPos);

            currentPos.z += (colCount * direction * gridSize);
            gridPos.Add(currentPos);

            direction *= -1;
        }

        currentPos.x = startr;
        gridPos.Add(currentPos);

        int colDirection = direction = 1;
        if (currentPos.z == endc) colDirection = -1; 

        for (int i = 0; i < colCount; i++)
        {
            currentPos.z += (colDirection * gridSize);
            gridPos.Add(currentPos);

            currentPos.x += (rowCount * direction * gridSize);
            gridPos.Add(currentPos);

            direction *= -1;
        }

        Ir.positionCount = gridPos.Count;
        Ir.SetPositions(gridPos.ToArray());
    }

    // Start is called before the first frame update
    void Start()
    {
        Ir = this.GetComponent<LineRenderer>();
        initLineRenderer(Ir);

        gridSize = MapInfo.getGridSize();
        startr = startc = MapInfo.getStartGrid();
        makeGrid(Ir, startr, startc, rowCount, colCount);
    }

    public void updateGrid()
    {
        gridSize = MapInfo.getGridSize();
        makeGrid(Ir, startr, startc, rowCount, colCount);
    }
}
