using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Furniture : MonoBehaviour
{
    //for interaction
    public PlaneGrid drawing;

    //for Item
    public GameObject item; //선택된 아이템
    Dictionary<int, Dictionary<string, string>> data = new Dictionary<int, Dictionary<string, string>>(); //아이템 데이터 저장 형태를 어케할지 미정이라 일단 csv로 
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

        ////피봇(좌상단) 좌표를 중앙 좌표로
        float targetX = source.x;
        float targetZ = source.z;

        //그리드 좌표로
        targetX -= item.transform.localScale.x * width / 2;  //물체 중심 기준. width 기준 값이 5라서. 나중에 타일 분리하면 여기 숫자도 바꿔주세요
        targetX -= targetX % gridSize;
        if (source.x <= 0) //음수랑 양수랑 뺄셈 방향이 달라서
        {
            targetX -= gridSize;
        }
        Debug.Log("sourceX: " + source.x + "targetX: " + targetX);
        //centerZ += centerZ % gridSize;

        targetZ += item.transform.localScale.z * width / 2;  //물체 중심 기준. width 기준 값이 5라서. 나중에 타일 분리하면 여기 숫자도 바꿔주세요
        targetZ -= targetZ % gridSize;
        if (source.z >= 0) //음수랑 양수랑 뺄셈 방향이 달라서
        {
            targetZ += gridSize;
        }

        //반환값에 적용
        source.x = targetX;
        source.z = targetZ;
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

        //아이템 세팅 - 일단 큐브로 대체합니다. 나중엔 프리팹 만들어서 그거 이용할 생각
        int width = int.Parse(itemData["width"]);
        int depth = int.Parse(itemData["depth"]);

        int ratio = gridSize / 5; //현재 기준 사이즈가 5긴한데 이거 나중에 수정
        item.transform.localScale = new Vector3(width*ratio, item.transform.localScale.y, depth*ratio); //지금은 크기가 그리드 사이즈지만, 그리드 사이즈는 데이터 저장 시 사용될 거고 보이는 아이템은 고유 크기가 있을 것(아마도)

        //item = Instantiate(furniture[id], Input.mousePosition, Quaternion.identity);
    }

    //itemChoose는 가구 위치를 수정하기 위해 선택했을 때도 true가 되는데 어케 구현할지 미정

    public void cancleFurniture() //아이템 선택 취소 
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
