using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector3 LocomotionInputValues => _locomotionInputValues;

    private Vector3 _locomotionInputValues;

    private float _horizontalInputValue, _verticalInputValue;

    // Update is called once per frame
    void Update()
    {
        float lastHorizontalValue = _horizontalInputValue;
        float lastVerticalValue = _verticalInputValue;
        if (Input.GetKeyDown(KeyCode.W))
        {
            _verticalInputValue++;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            _verticalInputValue--;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _verticalInputValue--;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            _verticalInputValue++;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _horizontalInputValue++;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            _horizontalInputValue--;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _horizontalInputValue--;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            _horizontalInputValue++;
        }
        if(lastHorizontalValue != _horizontalInputValue || lastVerticalValue != _verticalInputValue)
        {
            _locomotionInputValues.x = _horizontalInputValue;
            _locomotionInputValues.z = _verticalInputValue;
            _locomotionInputValues = Vector3.ClampMagnitude(_locomotionInputValues, 1f);
        }
    }
}
