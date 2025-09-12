using Unity.XR.CoreUtils;
using UnityEngine;

public class XRCameraHeightChanger : MonoBehaviour
{
    [SerializeField] private XROrigin xrOrigin;
    // Желаемая "виртуальная" высота (например, 1.7 метра)
    [SerializeField] private float targetHeight = 1.7f;

    void Start()
    {
        // Получаем текущую высоту головы (камеры)
        float headHeight = xrOrigin.CameraInOriginSpacePos.y;

        

        // Считаем разницу и опускаем/поднимаем XR Origin
        float offset = targetHeight - headHeight;

        // Смещаем XR Origin
        xrOrigin.CameraYOffset += offset;
    }

}
