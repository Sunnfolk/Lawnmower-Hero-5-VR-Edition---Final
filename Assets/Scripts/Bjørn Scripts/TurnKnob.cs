using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]

public class TurnKnob : MonoBehaviour
{
    Hand playerHand;
    [SerializeField] Rigidbody dial;

    private void HandAttachedUpdate()
    {
        Vector3 eulerRotation = new Vector3(0, 0, playerHand.transform.eulerAngles.z);
 
        transform.rotation = Quaternion.Euler(eulerRotation);
    }
 
    protected virtual void OnAttachedToHand(Hand hand)
    {
        playerHand = hand;
    }

    private void Update()
    {
        
    }
}
