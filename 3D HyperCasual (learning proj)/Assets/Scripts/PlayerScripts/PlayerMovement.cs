using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("MovementVariables")]
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _horizontalSpeedPointer;
    [SerializeField] private float _horizontalLimit;
    [SerializeField] private Animator _animator;



    [Header("Input")]
    [SerializeField] private KeyCode _moveLeft;
    [SerializeField] private KeyCode _moveRight;
    private Vector3 _targetPosition;

    private Camera _camera;

    private void Start()
    {
        _targetPosition = transform.position;
        _camera = Camera.main;  
    }

    private void Update()
    {
        MoveForward();
        HandleKeybordInput();
        HandlePointerInput();
    }

    private void HandleKeybordInput()
    {
        float moveX = 0f;

        if(Input.GetKey(_moveLeft))
        {
            moveX = -1f;
        }
        else if(Input.GetKey(_moveRight))
        {
            moveX = 1f;
        }
        if(moveX != 0)
        {
            _targetPosition += Vector3.right * moveX * _horizontalSpeed * Time.deltaTime;
            _targetPosition.x = Mathf.Clamp(_targetPosition.x, -_horizontalLimit, _horizontalLimit);

            Vector3 finalTargetPosition = new Vector3(_targetPosition.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, finalTargetPosition, 0.2f);
        }
    }

    private void HandlePointerInput()
    {
        Vector2 screenPosition = Vector2.zero;
        bool isActive = false;
        if (Input.touchCount > 0)
        {
            screenPosition = Input.GetTouch(0).position;
            isActive = true;
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            screenPosition = Mouse.current.position.ReadValue();
            isActive = true;
        }

        if (isActive == false)
        {
            return;
        }

        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        {
            if(plane.Raycast(ray, out float enter))
            {
                Vector3 worldPoint = ray.GetPoint(enter);
                float targetX = Mathf.Clamp(worldPoint.x, -_horizontalLimit, _horizontalLimit);

                Vector3 newPos = new Vector3(targetX, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, newPos, _horizontalSpeedPointer);
                _targetPosition = transform.position;
            }
        }
    }


    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _forwardSpeed * Time.deltaTime);
    }
}
