using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    const float DETECT = 5.0F;
    const float WASH = 0.3f;

    Throwing thr;

    public Transform floor;
    public Transform plate;

    private float time = 0;

    private State state;

    enum State
    {
        FRESH,        //���
        TRIM,         //����
        RIPENSOURCE,  //����
        SEASNING,     //������
        SOAK,         //��ġ��
        MERGE         //������ -> 4�ܰ�
    }

    void Start()
    {
        thr = GetComponent<Throwing>();

        state = State.FRESH;
    }

    void Update()
    {
        Debug.Log(state);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("�ٴ�");
            thr.changeToOff();
        }
        else if(collision.gameObject.CompareTag("Plate"))
        {
            Debug.Log("�� �� �ε���");
            gameObject.transform.SetParent(collision.gameObject.transform);
            thr.changeToOff();
            state = State.MERGE;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Water"))
        {
            if (state <= State.TRIM) //���� ���̸�
            {
                time += Time.deltaTime;

                if (time > WASH)
                {
                    time = 0;
                    state = State.TRIM;  //�ٵ�� level ������ �ϴµ� �ϴ� ���� ��
                } //��� ���� �� �ľ�����
            }
        }
    }

    //public void crash()
    //{
    //    if (Vector3.Distance(gameObject.transform.position, floor.position) < DETECT)
    //    {

    //    }
    //}
}
