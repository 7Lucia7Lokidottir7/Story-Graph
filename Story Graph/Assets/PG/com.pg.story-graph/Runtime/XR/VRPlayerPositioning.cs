using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class VRPlayerPositioning : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _origin;
    [SerializeField] private Transform _playerStart; // Точка старта игрока
    private CharacterController _characterController;

    [SerializeField] private InputActionProperty _recenterButton;

    public Transform originTransform => _origin;

    private void OnEnable()
    {
        _recenterButton.action.performed += RecenterInput;
    }
    private void OnDisable()
    {
        _recenterButton.action.performed -= RecenterInput;
    }
    void RecenterInput(InputAction.CallbackContext context) => ResetPosition();

    // Start is called before the first frame update
    void Start()
    {
        _characterController = _origin.GetComponentInChildren<CharacterController>();
        StartCoroutine(StartResetPosition());
    }

    [ContextMenu("Reset Position")]
    public void ResetPosition()
    {
        _characterController.center = new Vector3(_head.localPosition.x, _characterController.center.y, _head.localPosition.z);
        float rotationAngleY = _playerStart.rotation.eulerAngles.y - _head.transform.rotation.eulerAngles.y;
        _origin.Rotate(0, rotationAngleY, 0);
        Vector3 distanceDiff = _playerStart.position - _head.transform.position;
        _origin.position += new Vector3(distanceDiff.x, 0f, distanceDiff.z);
    }
    public void ResetPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        _characterController.center = new Vector3(_head.localPosition.x, _characterController.center.y, _head.localPosition.z);
        float rotationAngleY = targetRotation.eulerAngles.y - _head.transform.rotation.eulerAngles.y;
        _origin.Rotate(0, rotationAngleY, 0);
        Vector3 distanceDiff = targetPosition - _head.transform.position;
        _origin.position += new Vector3(distanceDiff.x, 0f, distanceDiff.z);
    }
    IEnumerator StartResetPosition()
    {
        yield return new WaitForSeconds(1f);
        ResetPosition();
    }

}
