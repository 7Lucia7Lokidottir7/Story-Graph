using Unity.XR.CoreUtils;
using UnityEngine;

public class XRCameraHeightChanger : MonoBehaviour
{
    [SerializeField] private XROrigin xrOrigin;
    // �������� "�����������" ������ (��������, 1.7 �����)
    [SerializeField] private float targetHeight = 1.7f;

    void Start()
    {
        // �������� ������� ������ ������ (������)
        float headHeight = xrOrigin.CameraInOriginSpacePos.y;

        

        // ������� ������� � ��������/��������� XR Origin
        float offset = targetHeight - headHeight;

        // ������� XR Origin
        xrOrigin.CameraYOffset += offset;
    }

}
