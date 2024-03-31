using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    const float THROWTIME = 0.2f;
    const float VELOCITY = 15f;
    const float Z_VELOCITY = 10f;
    const float DISTANCE = 20f; //��ȣ�ۿ� ������ ����

    //��� �ٱ��ϸ� ������ �ش� ��� ����
    private enum State
    {
        FOLLOW, //���� �� ���콺 �����
        THROW,  //������ ����
        THROWING, //�������� ��
        OFF    //��ȣ�ۿ� �Ұ�
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
        state = State.THROW; //�׽�Ʈ

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
            //�ٴ��̸� Off, ��ȣ�ۿ� ���� ��ü�� �˸��� state�� ����
            //������ٵ� �ٽ� ����
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
        //    //gameObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, this.transform.position.z); //z���� �����ѵ� mouse ��ǥ�� ��߳��� ���� �ʿ�

        //    if (time > THROWTIME)
        //    {
        //        time = 0f;
        //        preLocation = gameObject.transform.position; //���� �� ��ġ
        //    }
        //}
    }

    public void OnMouseUp()
    {
        follow = false;

        if (state == State.THROW)
        {
            //if (Input.mousePosition)                                                   
            //�������� ���� ���� -> ��ȣ�ۿ��� �ִ� ���� �дٴ���
            {

            }

            //���� ��ȭ
            state = State.THROWING;
            rigid.isKinematic = false;
            //������
            Vector3 f = gameObject.transform.position - preLocation;
            rigid.AddForce(f.x * f.x * VELOCITY, VELOCITY, f.y * VELOCITY * Z_VELOCITY); //z������ �ξ� ���� ��
        }
    }

    public void changeToOff()
    {
        state = State.OFF;
        rigid.isKinematic = true;
    }

    public void changeToMove() //throwing ���¿��� �ٸ� �Ͱ� �浹 �� ���
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
                //gameObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, this.transform.position.z); //z���� �����ѵ� mouse ��ǥ�� ��߳��� ���� �ʿ�

                if (time > THROWTIME)
                {
                    time = 0f;
                    preLocation = gameObject.transform.position; //���� �� ��ġ
                }
            }

            yield return null;
        }
    }
}
