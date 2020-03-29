using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XR.Interaction.Toolkit.Extensions
{
    public class XRGrabPointInteractable : XRGrabInteractable
    {
        [SerializeField]
        private bool _attachAtContactPoint = true;

        private Transform _previousAttachTransform;

        protected override void OnSelectEnter(XRBaseInteractor interactor)
        {
            if (_attachAtContactPoint)
            {
                _previousAttachTransform = attachTransform;

                Transform tempAttachTrans = new GameObject("TempAttachTransform").transform;
                tempAttachTrans.SetParent(transform);
                tempAttachTrans.SetPositionAndRotation(interactor.transform.position, interactor.transform.rotation);

                attachTransform = tempAttachTrans;
            }

            base.OnSelectEnter(interactor);
        }

        protected override void OnSelectExit(XRBaseInteractor interactor)
        {
            base.OnSelectExit(interactor);

            if (_attachAtContactPoint)
            {
                Destroy(attachTransform.gameObject);

                attachTransform = _previousAttachTransform;
            }
        }
    }
}