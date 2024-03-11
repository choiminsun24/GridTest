using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Furniture : MonoBehaviour
{

    //����
    Dictionary<int, GameObject> furniture = new Dictionary<int, GameObject>();
    public GameObject Chair;
    public int width, height = 2;
    //���� id�� �ش� ������ �����ϴ� �����ʹ� �ܺο� �ΰ�
    //���⼭�� ��ư�� id�� �ο��ϸ� �ش� �����͸� ��ȸ�ؼ� ��ġ�Ǵ� ������ �������� ������ ����
    //�ϰ� ������ �ϴ��� �ϵ��ڵ��մϴ�.

    int GRIDSIZE;

    private GameObject Item; //���õ� ������
    private float fixedY = 0; //��� ������Ʈ�� pivot�� 0���� �����ؾ� �� �� ���ƿ�

    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
        GRIDSIZE = MapInfo.getGridSize();
        furniture.Add(1, Chair);

        Vector3 test = new Vector3(26, 3, 27);
        Debug.Log(coordinate(test, 2, 2));
    }

    void Update()
    {
        if (Item) //������ ������ ���� �� ���콺�� ���� �ٴմϴ�.
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
            {
                Vector3 mouse = hit.point;

                Item.transform.position = coordinate(mouse, width, height);
            }

            if (Input.GetMouseButtonDown(0)) //������ ��ġ �õ�
            {
                if (!existUIObject()) {
                    Instantiate(Item, Item.transform.position, Quaternion.identity); //Item���� ������ ���� ����������
                    //��ǥ�� ������ �����ͷ� ����


                    //�׷� ��ġ ������ ��Ҷ� ������ �����ؾ� �ϰ� -> cancleFurniture
                    //Item ������Ʈ ¥�� ������ ó���� �������� �ǵ�鵵 ����ϴϱ� Instantiate ��� Item ������Ʈ�� �ϳ� ���� ������ �ٲܱ�?
                }
            }
        }
    }

    private Vector3 coordinate(Vector3 source, int width, int height) //�׸��� ��ǥ ���
    {
        //�Ǻ�(�»��) ��ǥ�� �߾� ��ǥ��
        float centerX = source.x + (width * GRIDSIZE) / 2;
        float centerZ = source.z - (height * GRIDSIZE) / 2; //��ǥ�� ������

        //Ŀ�� ũ��
        //centerX += 0.7f;
        //centerZ -= 1.2f;

        //�׸��� ��ǥ��
        centerX -= centerX % GRIDSIZE; centerX -= 5;
        centerZ -= centerZ % GRIDSIZE; centerZ += 5; 
        
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
        //����ĳ��Ʈ�� eventDataCurrentPosition�� ���� �ش� ��ġ�� �ִ� UI�� results�� ����
        //eventDataCurrentPosition���� ����ĳ��Ʈ�� ��ġ, ����, ������ �� ���콺 �������� ���� ������ ����ȴ��
        //��� ��ǿ� �������

        //�˻�� UI ��Ұ� �ִٸ� UI ��� ���� �ִٰ� �Ǵ�
        return results.Count > 0;
    }

    public void makeFurniture(int id) //������ �������� id
    {
        Item = Instantiate(furniture[id], Input.mousePosition, Quaternion.identity);
        //������ ���� ������ �����ɴϴ�. > ������Ʈ, size ����...
    }

    public void cancleFurniture() //������ ���� ��� 
    {
        Item = null;
    }
}
