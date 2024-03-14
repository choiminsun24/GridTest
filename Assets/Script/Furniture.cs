using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Furniture : MonoBehaviour
{
    //for interaction
    public PlaneGrid drawing;

    //for Item
    public GameObject item; //���õ� ������
    Dictionary<int, Dictionary<string, string>> data = new Dictionary<int, Dictionary<string, string>>(); //������ ������ ���� ���¸� �������� �����̶� �ϴ� csv�� 
    private Dictionary<string, string> itemData = new Dictionary<string, string>();
    private bool itemChoose = false;

    //for Item Move
    private int gridSize;
    private float fixedY = 0;

    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
        data = CSVReader.Read("ItemInfo", 1);
        gridSize = MapInfo.getGridSize(); 

    }

    void Update()
    {
        if (itemChoose) //������ ������ ���� �� ���콺�� ���� �ٴմϴ�.
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
            {
                Vector3 mouse = hit.point;

                item.transform.position = coordinate(mouse);
            }

            if (Input.GetMouseButtonDown(0)) //������ ��ġ �õ�
            {
                if (!existUIObject()) {
                    Instantiate(item, item.transform.position, Quaternion.identity);
                    //��ǥ�� ������ �����ͷ� ����

                    //Item ������Ʈ ¥�� ������ ó���� �������� �ǵ�鵵 ����ϴϱ� Instantiate ��� Item ������Ʈ�� �ϳ� ���� ������ �ٲܱ�?
                }
            }
        }
    }

    private Vector3 coordinate(Vector3 source) //�׸��� ��ǥ ���
    {
        int width = int.Parse(itemData["width"]);
        int depth = int.Parse(itemData["depth"]);

        ////�Ǻ�(�»��) ��ǥ�� �߾� ��ǥ��
        float targetX = source.x;
        float targetZ = source.z;

        //�׸��� ��ǥ��
        targetX -= item.transform.localScale.x * width / 2;  //��ü �߽� ����. width ���� ���� 5��. ���߿� Ÿ�� �и��ϸ� ���� ���ڵ� �ٲ��ּ���
        targetX -= targetX % gridSize;
        if (source.x <= 0) //������ ����� ���� ������ �޶�
        {
            targetX -= gridSize;
        }
        Debug.Log("sourceX: " + source.x + "targetX: " + targetX);
        //centerZ += centerZ % gridSize;

        targetZ += item.transform.localScale.z * width / 2;  //��ü �߽� ����. width ���� ���� 5��. ���߿� Ÿ�� �и��ϸ� ���� ���ڵ� �ٲ��ּ���
        targetZ -= targetZ % gridSize;
        if (source.z >= 0) //������ ����� ���� ������ �޶�
        {
            targetZ += gridSize;
        }

        //��ȯ���� ����
        source.x = targetX;
        source.z = targetZ;
        source.y = fixedY;

        return source;
    }

    bool existUIObject()
    {
        //���콺 ��ġ�� ������ �̺�Ʈ ������ ����
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.mousePosition;

        // �̺�Ʈ �ý����� ����Ͽ� UI ��Ҹ� Ȯ��
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        //�˻�� UI ��Ұ� �ִٸ� UI ��� ���� �ִٰ� �Ǵ�
        return results.Count > 0;
    }

    public void makeFurniture(int id) //������ �������� id
    {
        itemChoose = true;
        itemData = data[id];

        if (itemData["type"].Equals("Tile"))
        {
            MapInfo.setTileGrid();
            touchGrid();
        }
        else
        {
            MapInfo.setFurnitureGrid(); //default, 5
            touchGrid();
        }

        //������ ���� - �ϴ� ť��� ��ü�մϴ�. ���߿� ������ ���� �װ� �̿��� ����
        int width = int.Parse(itemData["width"]);
        int depth = int.Parse(itemData["depth"]);

        int ratio = gridSize / 5; //���� ���� ����� 5���ѵ� �̰� ���߿� ����
        item.transform.localScale = new Vector3(width*ratio, item.transform.localScale.y, depth*ratio); //������ ũ�Ⱑ �׸��� ����������, �׸��� ������� ������ ���� �� ���� �Ű� ���̴� �������� ���� ũ�Ⱑ ���� ��(�Ƹ���)

        //item = Instantiate(furniture[id], Input.mousePosition, Quaternion.identity);
    }

    //itemChoose�� ���� ��ġ�� �����ϱ� ���� �������� ���� true�� �Ǵµ� ���� �������� ����

    public void cancleFurniture() //������ ���� ��� 
    {
        itemChoose = false;
        item = null;
    }

    private void touchGrid()
    {
        gridSize = MapInfo.getGridSize();
        drawing.updateGrid();
    }
}
