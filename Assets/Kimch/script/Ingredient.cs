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
        FRESH,        //재료
        TRIM,         //손질
        RIPENSOURCE,  //김장
        SEASNING,     //겉절이
        SOAK,         //동치미
        MERGE         //접시행 -> 4단계
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
            Debug.Log("바닥");
            thr.changeToOff();
        }
        else if(collision.gameObject.CompareTag("Plate"))
        {
            Debug.Log("왜 안 부딪힘");
            gameObject.transform.SetParent(collision.gameObject.transform);
            thr.changeToOff();
            state = State.MERGE;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Water"))
        {
            if (state <= State.TRIM) //손질 전이면
            {
                time += Time.deltaTime;

                if (time > WASH)
                {
                    time = 0;
                    state = State.TRIM;  //다듬기 level 만들어야 하는데 일단 이케 함
                } //양념 뭍은 거 씻어버릴까ㅎ
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
