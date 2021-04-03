using System;
using UnityEngine;

public class MouseInputHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) LeftMouseDown();
        if (Input.GetMouseButtonUp(0)) LeftMouseUp();
        if (Input.GetMouseButtonDown(1)) RightMouseDown();
    }

    GameObject dragFrom, dragTo;
    void LeftMouseDown()
    {
        var obj = GetObjectUnderMouse();
        if (obj != null) dragFrom = obj;
    }

    void LeftMouseUp()
    {
        var obj = GetObjectUnderMouse();
        if (obj != null)
        {
            dragTo = obj;
            if (dragFrom != dragTo)
                Route.TryCreateFromTwoObjects(dragFrom, dragTo);
            else
            {
                if (dragFrom.CompareTag("Route"))
                {
                    var route = dragFrom.GetComponent<RouteObject>();
                    route.route.WorkersAmount++;
                }
            }
        }
        dragTo = null;
        dragFrom = null;
    }

    void RightMouseDown()
    {
        
        var obj = GetObjectUnderMouse();
        if (obj != null)
        {
            if (obj.CompareTag("Route"))
            {
                obj.GetComponent<RouteObject>().route.WorkersAmount--;
            }
        }
    }

    static GameObject GetObjectUnderMouse()
    {
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
            return hit.collider.gameObject;
        else return null;
    }
}