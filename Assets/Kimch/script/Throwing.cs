using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    const float THROWTIME = 0.2f;
    const float VELOCITY = 15f;
    const float Z_VELOCITY = 10f;
    const float DISTANCE = 20f; //상호작용 가능한 범위

    //재료 바구니를 누르면 해당 재료 생성
    private enum State
    {
        FOLLOW, //선택 시 마우스 따라옴
        THROW,  //던지기 가능
        THROWING, //던져지는 중
        OFF    //상호작용 불가
    }
    public enum Type
    {
        INGRED,
        PLATE
    }

    public Camera camera;
    public Type isType;
    private State state;

    private Ray ray;
    private RaycastHit hit;
    private GameObject target;

    private float time;
    private Vector3 preLocation;
    Rigidbody rigid;

    Ingredient ingr;

    private bool follow = false;

    void Start()
    {
        state = State.THROW; //테스트

        ingr = GetComponent<Ingredient>();
        if (ingr != null)
            isType = Type.INGRED;
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true;
    }

    void Update()
    {
        if (state == State.OFF)
        {
            ;
        }
        else if (state == State.THROWING)
        {
            ;
            //바닥이면 Off, 상호작용 가능 물체면 알맞은 state로 변경
            //리지드바디 다시 끄기
        }
        else if (state == State.FOLLOW)
        {
            //ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if (Physics.Raycast(ray, out hit, DISTANCE))
            //{
            //    target = hit.collider.gameObject;
            //}
        }
        else if (state == State.THROW)
        {

        }
    }

    public void OnMouseDown()
    {
        if (state == State.THROW || state == State.FOLLOW)
        {
            follow = true;
            StartCoroutine(drag());
        }
    }

    public void OnMouseDrag()
    {
        //if (state == State.FOLLOW || state == State.THROW)
        //{
        //    time += Time.deltaTime;
        //    Vector3 mousePosition = Input.mousePosition;
        //    Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, camera.transform.position.y));

        //    gameObject.transform.position = worldPosition;
        //    //gameObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, this.transform.position.z); //z축은 일정한데 mouse 좌표가 어긋나서 조정 필요

        //    if (time > THROWTIME)
        //    {
        //        time = 0f;
        //        preLocation = gameObject.transform.position; //조금 전 위치
        //    }
        //}
    }

    public void OnMouseUp()
    {
        follow = false;

        if (state == State.THROW)
        {
            //if (Input.mousePosition)                                                   
            //던져지지 않을 조건 -> 상호작용이 있는 곳에 둔다던가
            {

            }

            //상태 변화
            state = State.THROWING;
            rigid.isKinematic = false;
            //던지기
            Vector3 f = gameObject.transform.position - preLocation;
            rigid.AddForce(f.x * f.x * VELOCITY, VELOCITY, f.y * VELOCITY * Z_VELOCITY); //z축으로 훨씬 많이 감
        }
    }

    public void changeToOff()
    {
        state = State.OFF;
        rigid.isKinematic = true;
    }

    public void changeToMove() //throwing 상태에서 다른 것과 충돌 시 사용
    {
        rigid.isKinematic = true;

        if (isType == Type.INGRED)
            state = State.THROW;
        else
            state = State.FOLLOW;
    }

    IEnumerator drag()
    {
        while(follow)
        {
            if (state == State.FOLLOW || state == State.THROW)
            {
                time += Time.deltaTime;
                Vector3 mousePosition = Input.mousePosition;
                Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, camera.transform.position.y));

                gameObject.transform.position = worldPosition;
                //gameObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, this.transform.position.z); //z축은 일정한데 mouse 좌표가 어긋나서 조정 필요

                if (time > THROWTIME)
                {
                    time = 0f;
                    preLocation = gameObject.transform.position; //조금 전 위치
                }
            }

            yield return null;
        }
    }
}
