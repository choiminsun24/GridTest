using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlate : MonoBehaviour
{
    public GameObject plate;
    public Camera camera;

    private Throwing targetTh;


    // Start is called before the first frame update
    void OnMouseDown()
    {
        Debug.Log("Å¬¸¯");

        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, camera.transform.position.y));

        GameObject target = Instantiate(plate, worldPosition, Quaternion.identity);
        targetTh = target.GetComponent<Throwing>();
        targetTh.OnMouseDown();
    }

    void OnMouseUp()
    {
        if (targetTh)
        {
            targetTh.OnMouseUp();
        }
    }
}
