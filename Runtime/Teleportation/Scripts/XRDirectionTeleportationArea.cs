using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDirectionTeleportationArea : BaseTeleportationInteractable
{
    private bool _updateReticle;

    Dictionary<XRBaseInteractor, GameObject> m_ReticleCache = new Dictionary<XRBaseInteractor, GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in m_ReticleCache)
        {
            XRController_Extension controller = item.Key.GetComponent<XRController_Extension>();

            if (controller.inputDevice.TryGetFeatureValue(new InputFeatureUsage<Vector2>("Primary2DAxis"), out Vector2 value))
            {
                if (value.magnitude > (controller as XRTeleportationController).TriggerTeleportThreshold)
                {
                    Vector3 camForward = Camera.main.transform.forward;
                    camForward.y = 0;
                    camForward = Quaternion.LookRotation(new Vector3(value.x, 0, value.y)) * camForward;
                    item.Value.transform.GetChild(0).rotation = Quaternion.LookRotation(camForward);
                }
                else
                {
                    TeleportRequest tr = new TeleportRequest();
                    tr.matchOrientation = MatchOrientation.Camera;
                    tr.requestTime = Time.time;
                    if (GenerateTeleportRequest(item.Key, new RaycastHit(), ref tr))
                    {
                        m_TeleportationProvider.QueueTeleportRequest(tr);
                    }

                    onSelectEnter.Invoke(item.Key);
                }
            }
        }
    }

    protected override bool GenerateTeleportRequest(XRBaseInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
    {
        Transform lookTrans = m_ReticleCache[interactor].transform.GetChild(0);

        teleportRequest.destinationPosition = lookTrans.position;
        teleportRequest.destinationUpVector = lookTrans.up; // use the area transform for data.
        teleportRequest.destinationForwardVector = lookTrans.forward;
        teleportRequest.destinationRotation = lookTrans.rotation;

        return true;
    }

    public override void AttachCustomReticle(XRBaseInteractor interactor)
    {
        if (interactor == null)
            return;

        // try and find any attached reticle and swap it
        IXRCustomReticleProvider ilv = interactor.transform.GetComponent<IXRCustomReticleProvider>();
        if (ilv != null)
        {

            GameObject prevReticle;
            if (m_ReticleCache.TryGetValue(interactor, out prevReticle))
            {
                Destroy(prevReticle);
                m_ReticleCache.Remove(interactor);
            }
            if (customReticle != null)
            {
                var rInstance = Instantiate(customReticle);
                m_ReticleCache.Add(interactor, rInstance);
                ilv.AttachCustomReticle(rInstance);
            }
        }
    }

    public override void RemoveCustomReticle(XRBaseInteractor interactor)
    {
        if (interactor == null)
            return;

        // try and find any attached reticle and swap it            
        IXRCustomReticleProvider ilv = interactor.transform.GetComponent<IXRCustomReticleProvider>();
        if (ilv != null)
        {
            GameObject reticle;
            bool setCustomReticle = false;
            if (m_ReticleCache.TryGetValue(interactor, out reticle))
            {
                Destroy(reticle);
                m_ReticleCache.Remove(interactor);
                setCustomReticle = true;
            }
            if (setCustomReticle)
            {
                ilv.RemoveCustomReticle();
            }
        }
    }
}
