using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Telekinesis : MonoBehaviour
{
    [SerializeField] InputActionReference telekinesisRotationReference = null;
    [SerializeField] InputActionReference selectAction = null;
    [SerializeField] float minRotation = 0f;
    [SerializeField] float maxRotation = 0f;
    [SerializeField] bool rightHand = false;

    bool overTelekinesisObject = false;
    bool grabObject = false;
    XRRayInteractor rayInteractor = null;

    private void Awake()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
    }

    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.CompareTag("cube"))
        {
            overTelekinesisObject = true;
            selectAction.action.Disable();
        }
    }

    public void OnHoverExited(HoverExitEventArgs args)
    {
        overTelekinesisObject = false;
        selectAction.action.Enable();
    }

    private void Update()
    {
        float value = telekinesisRotationReference.action.ReadValue<Quaternion>().eulerAngles.z;
        bool canGrabObject = false;

        if (rightHand)
        {
            canGrabObject = value < maxRotation && value != 0f && value > minRotation;
        }
        else
        {
            canGrabObject = value > minRotation && value < maxRotation;
        }

        if (canGrabObject && overTelekinesisObject && !grabObject)
        {
            grabObject = true;
            rayInteractor.hoverToSelect = true;
        }
        else if (!canGrabObject && overTelekinesisObject && grabObject)
        {
            grabObject = false;
            rayInteractor.hoverToSelect = false;
        }
    }
}