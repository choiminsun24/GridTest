using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Furniture : MonoBehaviour
{
    //데이터 저장 형태를 어케할지 미정이라 일단 csv로 
    Dictionary<int, Dictionary<string, string>> data = new Dictionary<int, Dictionary<string, string>>(); //아이템 정보

    int GRIDSIZE;

    private Dictionary<string, string> itemData = new Dictionary<string, string>();
    public GameObject item; //선택된 아이템
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
        if (itemChoose) //선택한 가구가 있을 때 마우스를 따라 다닙니다.
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
            {
                Vector3 mouse = hit.point;

                item.transform.position = coordinate(mouse);
            }

            if (Input.GetMouseButtonDown(0)) //아이템 배치 시도
            {
                if (!existUIObject()) {
                    Instantiate(item, item.transform.position, Quaternion.identity);
                    //좌표에 아이템 데이터로 저장

                    //Item 오브젝트 짜피 반투명 처리랑 색상으로 피드백도 줘야하니까 Instantiate 대신 Item 오브젝트를 하나 만들어서 정보만 바꿀까?
                }
            }
        }
    }

    private Vector3 coordinate(Vector3 source) //그리드 좌표 계산
    {
        int width = int.Parse(itemData["width"]);
        int depth = int.Parse(itemData["depth"]);

        //피봇(좌상단) 좌표를 중앙 좌표로
        float centerX = source.x - (width * GRIDSIZE) / 2;
        float centerZ = source.z + (depth * GRIDSIZE) / 2; //좌표상 역방향

        //커서 크기
        centerX -= 0.7f;
        centerZ += 1.2f;

        //그리드 좌표로
        centerX -= centerX % GRIDSIZE;
        centerZ -= centerZ % GRIDSIZE;
        
        //반환값에 적용
        source.x = centerX;
        source.z = centerZ;
        source.y = fixedY;

        return source;
    }

    bool existUIObject()
    {
        //마우스 위치를 저장한 이벤트 데이터 생성
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.mousePosition;

        // 이벤트 시스템을 사용하여 UI 요소를 확인
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        //검사된 UI 요소가 있다면 UI 요소 위에 있다고 판단
        return results.Count > 0;
    }

    public void makeFurniture(int id) //선택한 아이템의 id
    {
        itemChoose = true;
        itemData = data[id];

        if (itemData["type"].Equals("Tile")) MapInfo.setTileGrid();
        else MapInfo.setFurnitureGrid(); //default, 5

        //아이템 세팅 - 일단 큐브로 대체합니다. 나중엔 프리팹 만들어서 그거 이용할 생각
        int width = int.Parse(itemData["width"]);
        int depth = int.Parse(itemData["depth"]);

        item.transform.localScale = new Vector3(width, item.transform.localScale.y, depth);

        //item = Instantiate(furniture[id], Input.mousePosition, Quaternion.identity);
    }

    //itemChoose는 가구 위치를 수정하기 위해 선택했을 때도 true가 되는데 어케 구현할지 미정

    public void cancleFurniture() //아이템 선택 취소 
    {
        itemChoose = false;
        item = null;
    }
}
