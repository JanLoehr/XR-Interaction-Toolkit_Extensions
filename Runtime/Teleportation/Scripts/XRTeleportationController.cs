using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRTeleportationController : XRController_Extension
{
    [Header("Teleporting")]

    [SerializeField]
    [Range(0, 1)]
    [Tooltip("The magnitude of the AxisVector where the teleport will be triggered when lower than that")]
    private float _triggerTeleportThreshold = 0.3f;
    public float TriggerTeleportThreshold => _triggerTeleportThreshold;

    private bool _performSetup = true;

    private GameObject _modelGameObject = null;

    void Update()
    {
        if (_performSetup)
        {
            PerformSetup();
            _performSetup = false;
        }

        if (enableInputTracking &&
            (updateTrackingType == UpdateType.Update ||
            updateTrackingType == UpdateType.UpdateAndBeforeRender))
        {
            UpdateTrackingInput();

        }

        if (enableInputActions)
        {
            UpdateInput();
        }
    }

    void UpdateInput()
    {
        // clear state for selection, activation and press state dependent on this frame
        m_SelectInteractionState.activatedThisFrame = m_SelectInteractionState.deActivatedThisFrame = false;

        HandleInteractionAction(InputHelpers.Button.Trigger, ref m_SelectInteractionState);
    }

    void HandleInteractionAction(InputHelpers.Button button, ref InteractionState interactionState)
    {
        if (inputDevice.TryGetFeatureValue(new InputFeatureUsage<Vector2>("Primary2DAxis"), out Vector2 value))
        {
            if (value.magnitude > _triggerTeleportThreshold)
            {
                m_SelectInteractionState.active = true;
            }
            else if (m_SelectInteractionState.active)
            {
                m_SelectInteractionState.active = false;
                m_SelectInteractionState.deActivatedThisFrame = true;
            }
        }
    }

    void PerformSetup()
    {
        SetupModel();
    }

    void SetupModel()
    {
        if (_modelGameObject)
            Destroy(_modelGameObject);
        var model = modelPrefab;
        if (model != null)
        {
            _modelGameObject = Instantiate(model).gameObject;
            _modelGameObject.transform.parent = modelTransform;
            _modelGameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            _modelGameObject.transform.localRotation = Quaternion.identity;
            _modelGameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _modelGameObject.transform.gameObject.SetActive(true);
        }
    }
}
