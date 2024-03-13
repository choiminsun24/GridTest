using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Furniture : MonoBehaviour
{
    //������ ���� ���¸� �������� �����̶� �ϴ� csv�� 
    Dictionary<int, Dictionary<string, string>> data = new Dictionary<int, Dictionary<string, string>>(); //������ ����

    int GRIDSIZE;

    private Dictionary<string, string> itemData = new Dictionary<string, string>();
    public GameObject item; //���õ� ������
    private bool itemChoose = false;
    private float fixedY = 0;

    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
        data = CSVReader.Read("ItemInfo", 1);
        GRIDSIZE = MapInfo.getGridSize(); 

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

        //�Ǻ�(�»��) ��ǥ�� �߾� ��ǥ��
        float centerX = source.x - (width * GRIDSIZE) / 2;
        float centerZ = source.z + (depth * GRIDSIZE) / 2; //��ǥ�� ������

        //Ŀ�� ũ��
        centerX -= 0.7f;
        centerZ += 1.2f;

        //�׸��� ��ǥ��
        centerX -= centerX % GRIDSIZE;
        centerZ -= centerZ % GRIDSIZE;
        
        //��ȯ���� ����
        source.x = centerX;
        source.z = centerZ;
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

        if (itemData["type"].Equals("Tile")) MapInfo.setTileGrid();
        else MapInfo.setFurnitureGrid(); //default, 5

        //������ ���� - �ϴ� ť��� ��ü�մϴ�. ���߿� ������ ���� �װ� �̿��� ����
        int width = int.Parse(itemData["width"]);
        int depth = int.Parse(itemData["depth"]);

        item.transform.localScale = new Vector3(width, item.transform.localScale.y, depth);

        //item = Instantiate(furniture[id], Input.mousePosition, Quaternion.identity);
    }

    //itemChoose�� ���� ��ġ�� �����ϱ� ���� �������� ���� true�� �Ǵµ� ���� �������� ����

    public void cancleFurniture() //������ ���� ��� 
    {
        itemChoose = false;
        item = null;
    }
}
