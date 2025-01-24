using UnityEngine;

class ScrollAndPinch : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID
    [SerializeField] Transform _leftBound;
    [SerializeField] Transform _rightBound;
    [SerializeField] Transform _topBound;
    [SerializeField] Transform _bottomBound;
    [SerializeField] bool _rotate;
    
    Camera _camera;
    Plane _plane;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount >= 1)
            _plane.SetNormalAndPosition(transform.up, transform.position);

        if (Input.touchCount >= 1)
        {
            Scroll();
        }

        if (Input.touchCount >= 2)
        {
            Pinch();
        }
    }

    private void Scroll()
    {
        var Delta1 = PlanePositionDelta(Input.GetTouch(0));

        if (Input.GetTouch(0).phase == TouchPhase.Moved)
            _camera.transform.Translate(Delta1, Space.World);

        CheckInBounds();
        
    }

    void Pinch()
    {
        var pos1 = PlanePosition(Input.GetTouch(0).position);
        var pos2 = PlanePosition(Input.GetTouch(1).position);
        var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
        var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

        var zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);

        if (zoom == 0 || zoom > 10)
            return;

        float newFOV = _camera.fieldOfView / zoom;
        _camera.fieldOfView = Mathf.Clamp(newFOV, 50f, 70f);

        if (_rotate && pos2b != pos2)
        {
            _camera.transform.RotateAround(pos1, _plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, _plane.normal));
        }
    }

    void CheckInBounds()
    {
        Vector3 pos = _camera.transform.position;
        if (_camera.transform.position.x < _leftBound.position.x)
            pos.x = _leftBound.position.x;
        if (_camera.transform.position.x > _rightBound.position.x)
            pos.x = _rightBound.position.x;
        if (_camera.transform.position.z < _bottomBound.position.z)
            pos.z = _bottomBound.position.z;
        if (_camera.transform.position.z > _topBound.position.z)
            pos.z = _topBound.position.z;

        _camera.transform.position = pos;
    }

    Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta
        var rayBefore = _camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = _camera.ScreenPointToRay(touch.position);
        if (_plane.Raycast(rayBefore, out var enterBefore) && _plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = _camera.ScreenPointToRay(screenPos);
        if (_plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
#endif
}