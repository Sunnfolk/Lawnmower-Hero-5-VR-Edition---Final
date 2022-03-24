using System;
using UnityEngine;

[RequireComponent(typeof(PickupAble))]
[RequireComponent(typeof(InteractAble))]
public class PickupAbleChangeColorWhenHeld : MonoBehaviour
{
    [SerializeField] private Material _heldMaterial;
    
    private Material _notHeldMaterial;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnHeldByHandChanged(InteractAble.Hand holdingHand)
    {
        if (holdingHand.GameObject != null)    //null = not held, else held
        {
            _notHeldMaterial = _meshRenderer.material;
            _meshRenderer.material = _heldMaterial;
        }
        else
        {
            _meshRenderer.material = _notHeldMaterial;
        }
    }

    private void OnTrackpadButtonChanged(bool state)
    {
        print("Also registers trackpad press state :))");
    }
}
