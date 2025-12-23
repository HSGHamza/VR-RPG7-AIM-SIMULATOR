using UnityEngine;
using System.Collections;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;

public class ForceAttachToSpecificHand : MonoBehaviour
{
    [Header("Assign the hand grabber (e.g. PhysicsRightHand)")]
    public HVRHandGrabber rightHand;

    [Header("Assign the weapon with HVRGrabbable")]
    public HVRGrabbable weapon;

    [Header("Attach delay (seconds)")]
    public float attachDelay = 0.1f;

    [Header("Optional: local position/rotation offset")]
    public Vector3 localPositionOffset;
    public Vector3 localRotationOffset;

    private void Start()
    {
        if (rightHand == null || weapon == null)
        {
            Debug.LogError("[ForceAttach] Missing references!");
            return;
        }

        StartCoroutine(AttachWeaponToHand());
    }

    private IEnumerator AttachWeaponToHand()
    {
        yield return new WaitForSeconds(attachDelay);

        // Disable physics
        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Parent weapon to the hand transform
        Transform handTransform = rightHand.transform;
        weapon.transform.SetParent(handTransform);

        // Apply offset (if needed)
        weapon.transform.localPosition = localPositionOffset;
        weapon.transform.localRotation = Quaternion.Euler(localRotationOffset);

        Debug.Log("[ForceAttach] Weapon parented to hand.");
    }
}
