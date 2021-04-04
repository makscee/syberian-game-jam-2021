using System;
using UnityEngine;

public class MouseInputHandler : MonoBehaviour
{
    void Start()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        if (GameManager.timeScaleUpdatedThisFrame)
        {
            _mouseDown = false;
            return;
        }
        if (Input.GetMouseButtonDown(0)) LeftMouseDown();
        if (Input.GetMouseButtonUp(0)) LeftMouseUp();
        if (Input.GetMouseButtonDown(1)) RightMouseDown();
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ZoomOrthoCamera(_camera.ScreenToWorldPoint(Input.mousePosition), 0.2f);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ZoomOrthoCamera(_camera.ScreenToWorldPoint(Input.mousePosition), -0.2f);
        }

        if (_mouseDown && dragFrom != null)
        {
            var mdo = MouseDragObject.instance;
            mdo.from = dragFrom.transform.position;
            var obj = GetObjectUnderMouse();
            mdo.to = obj != null && (obj.CompareTag("City") || obj.CompareTag("Flower")) ? obj.transform.position : (Vector3)MouseWorldPos();
        }

        if (_mouseDown && dragFrom == null)
        {
            _camera.transform.position = _cameraStartDrag -
                                         (_camera.ScreenToWorldPoint(Input.mousePosition) -
                                          _camera.ScreenToWorldPoint(_mouseStartDrag));
        }
    }

    GameObject dragFrom, dragTo;
    Vector2 _mouseStartDrag;
    Vector3 _cameraStartDrag;
    bool _mouseDown;
    void LeftMouseDown()
    {
        _mouseStartDrag = Input.mousePosition;
        _cameraStartDrag = Camera.main.transform.position;
        _mouseDown = true;
        var obj = GetObjectUnderMouse();
        if (obj != null && (obj.CompareTag("City") || obj.CompareTag("Flower")))
        {
            dragFrom = obj;
            
            var mdo = MouseDragObject.instance;
            mdo.SetActive(true);
        }
    }

    void LeftMouseUp()
    {
        _mouseDown = false;
        var mdo = MouseDragObject.instance;
        mdo.SetActive(false);
        var obj = GetObjectUnderMouse();
        if (obj && obj.CompareTag("Route"))
        {
            var route = obj.GetComponent<RouteObject>();
            route.route.WorkersAmount++;
        }
        else if (obj != null && dragFrom != null)
        {
            dragTo = obj;
            if (dragFrom != dragTo)
                Route.TryCreateFromTwoObjects(dragFrom, dragTo);
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
            } else if (obj.CompareTag("Flower"))
            {
                obj.GetComponent<Flower>().Destroy();
            }
        }
    }

    static GameObject GetObjectUnderMouse()
    {
        var hit = Physics2D.Raycast(MouseWorldPos(), Vector2.zero);

        if (hit.collider != null)
            return hit.collider.gameObject;
        else return null;
    }

    static Vector2 MouseWorldPos()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }
    
    const float MinZoom = 2f;
    const float MaxZoom = 20f;

    static Camera _camera;
    static void ZoomOrthoCamera(Vector3 zoomTowards, float amount)
    {
        var orthographicSize = _camera.orthographicSize;
        var multiplier = (1.0f / orthographicSize * amount);
        _camera.transform.position += (zoomTowards - _camera.transform.position) * multiplier;
        _camera.orthographicSize = Mathf.Clamp(orthographicSize - amount, MinZoom, MaxZoom);
    }
}