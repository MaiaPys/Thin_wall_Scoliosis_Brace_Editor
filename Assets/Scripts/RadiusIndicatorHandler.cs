using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System;

public class RadiusIndicatorHandler : MonoBehaviour {

    public GameObject scoliosisBraceObject;
    public GameObject radiusSlider;
    public MeshDeformer meshDeformer;
    public Material opaqueMaterial;
    public Material transparentMaterial;
    public bool rotationActivated = false;
    public bool movementActivated = false;

    private float sphereRadius;
    private IMixedRealityPointer leftHandPointer;
    private IMixedRealityPointer rightHandPointer;
    private IMixedRealityPointer activePointer;
    private bool isUpdateRunning = true;

    private void Start () {
        transform.localScale = Vector3.zero;
    }

    private void Update () {
        if ( !rotationActivated ) {
            if ( !movementActivated ) {

                foreach ( var inputSource in CoreServices.InputSystem.DetectedInputSources ) {
                    foreach ( var pointer in inputSource.Pointers ) {
                        if ( pointer.PointerName == "Left_ShellHandRayPointer(Clone)" )
                            leftHandPointer = pointer;
                        else if ( pointer.PointerName == "Right_ShellHandRayPointer(Clone)" )
                            rightHandPointer = pointer;
                    }
                }

                if ( leftHandPointer != null && leftHandPointer.IsInteractionEnabled )
                    activePointer = leftHandPointer;
                else if ( rightHandPointer != null && rightHandPointer.IsInteractionEnabled )
                    activePointer = rightHandPointer;
                else
                    activePointer = null;

                if ( isUpdateRunning ) {
                    sphereRadius = meshDeformer.selectionRadius * 2;
                    transform.localScale = new Vector3( sphereRadius, sphereRadius, sphereRadius );

                    try {
                        if ( activePointer != null && activePointer.IsInteractionEnabled ) {
                            if ( activePointer.Result.CurrentPointerTarget == scoliosisBraceObject )
                                transform.position = activePointer.Result.Details.Point;
                            else
                                transform.localScale = Vector3.zero;
                        } else {
                            transform.localScale = Vector3.zero;
                        }
                    } catch ( NullReferenceException ) { }
                }
            }
        }
    }

    public void ChangeSphereSize () {
        sphereRadius = meshDeformer.selectionRadius * 2;
        transform.localScale = new Vector3( sphereRadius, sphereRadius, sphereRadius );
    }

    public void StopUpdate () {
        isUpdateRunning = false;
        transform.position = new Vector3( radiusSlider.transform.position.x, radiusSlider.transform.position.y + 0.3f, radiusSlider.transform.position.z );
        GetComponent<Renderer>().material = opaqueMaterial;
    }

    public void StartUpdate () {
        isUpdateRunning = true;
        GetComponent<Renderer>().material = transparentMaterial;
    }
}