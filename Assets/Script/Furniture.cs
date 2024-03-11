using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Furniture : MonoBehaviour
{

    //더미
    Dictionary<int, GameObject> furniture = new Dictionary<int, GameObject>();
    public GameObject Chair;
    public int width, height = 2;
    //가구 id와 해당 가구를 연결하는 데이터는 외부에 두고
    //여기서는 버튼에 id를 부여하면 해당 데이터를 조회해서 매치되는 가구를 가져오는 것으로 구현
    //하고 싶은데 일단은 하드코딩합니다.

    int GRIDSIZE;

    private GameObject Item; //선택된 아이템
    private float fixedY = 0; //모든 오브젝트의 pivot은 0으로 통일해야 할 것 같아요

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
        if (Item) //선택한 가구가 있을 때 마우스를 따라 다닙니다.
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
            {
                Vector3 mouse = hit.point;

                Item.transform.position = coordinate(mouse, width, height);
            }

            if (Input.GetMouseButtonDown(0)) //아이템 배치 시도
            {
                if (!existUIObject()) {
                    Instantiate(Item, Item.transform.position, Quaternion.identity); //Item에서 아이템 정보 가져오던가
                    //좌표에 아이템 데이터로 저장


                    //그럼 배치 아이템 취소랑 변경이 가능해야 하고 -> cancleFurniture
                    //Item 오브젝트 짜피 반투명 처리랑 색상으로 피드백도 줘야하니까 Instantiate 대신 Item 오브젝트를 하나 만들어서 정보만 바꿀까?
                }
            }
        }
    }

    private Vector3 coordinate(Vector3 source, int width, int height) //그리드 좌표 계산
    {
        //피봇(좌상단) 좌표를 중앙 좌표로
        float centerX = source.x + (width * GRIDSIZE) / 2;
        float centerZ = source.z - (height * GRIDSIZE) / 2; //좌표상 역방향

        //커서 크기
        //centerX += 0.7f;
        //centerZ -= 1.2f;

        //그리드 좌표로
        centerX -= centerX % GRIDSIZE; centerX -= 5;
        centerZ -= centerZ % GRIDSIZE; centerZ += 5; 
        
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
        //레이캐스트를 eventDataCurrentPosition로 쏴서 해당 위치에 있는 UI를 results에 저장
        //eventDataCurrentPosition에는 래이캐스트의 위치, 방향, 시작점 등 마우스 포인터의 여러 정보가 저장된대요
        //요거 노션에 적어놓기

        //검사된 UI 요소가 있다면 UI 요소 위에 있다고 판단
        return results.Count > 0;
    }

    public void makeFurniture(int id) //선택한 아이템의 id
    {
        Item = Instantiate(furniture[id], Input.mousePosition, Quaternion.identity);
        //아이템 관련 정보를 가져옵니다. > 오브젝트, size 정보...
    }

    public void cancleFurniture() //아이템 선택 취소 
    {
        Item = null;
    }
}
