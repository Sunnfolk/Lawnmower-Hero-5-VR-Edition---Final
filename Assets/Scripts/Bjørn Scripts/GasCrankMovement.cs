using System.Collections.Generic;
using UnityEngine;

public class GasCrankMovement : MonoBehaviour
{
    
    public float rotateSpd = 1f;
    public float minAngle = -18f;
    public float maxAngle = 45f;
    public float boostMaxAngle = 60f; //Max angle when boosting
    public float deadzoneAngle = 5f; //How big the zRot must be before the crank rotates
    
    private float _angleStickyOffset; // offset between wheel rotation and hand position on grab
    public List<Transform> stickedHandsTransforms = new List<Transform>(1);
    private bool _handSticked;
    
    public Vector3 relativePos;
    public GameObject crankBase;

    //public HingeJoint joint;
    
    [HideInInspector] public float power; //Used to change the z rotation into a float used to multiply with movement speed of lawnmower

    private float _zRot; //Targeted z rotation, but actual rotation can be overwritten based on the deadzone
    
    private void OnStickedHandsChanged(InteractAble.Hand[] stickedHands)    //set hand transform List every time hand is changed
    {
        foreach (InteractAble.Hand stickedHand in stickedHands)
        {
            if (stickedHand.Transform != null)
            {
                stickedHandsTransforms.Add(stickedHand.Transform);
                _handSticked = true;
                //CalculateOffset();_______________________________________________________________________________________________________________________ (Commented out by mathias, because of compilation error)
            }
            else
            {
                stickedHandsTransforms.Remove(stickedHand.LastFrameStickedHandTransform);
                _handSticked = false;
            }
        }
    }/*
    private void CalculateOffset()
    {
        float rawAngle = CalculateRawAngle();
        angleStickyOffset = power - rawAngle;
    }

    private float CalculateRawAngle()
    {
        RelativePos = crankBase.transform.InverseTransformPoint(_stickedHandsTransforms[0].position); // GETTING RELATIVE POSITION BETWEEN CRANK BASE AND HAND
        
        return Mathf.Atan2( RelativePos.y, RelativePos.x) * Mathf.Rad2Deg; // GETTING CIRCULAR DATA FROM X & Z RELATIVES  VECTORS
    }
    */
    void Update()
    {
        //TODO: Make compatible with VR input 
        //Changes crank rotation based on scroll wheel input 
        var rot = crankBase.transform.localRotation.eulerAngles;
        if (_handSticked)
        {
            //zRot = (CalculateRawAngle() + angleStickyOffset); // When hands are holding the wheel hand dictates how the wheel moves
            // angleSticky Offset is calculated on wheel grab - makes wheel not to rotate instantly to the users hand
        }
        /* Test Input with ScrollWheel
        var add = rotateSpd * Time.deltaTime;
        if (_Input.crankAxis > 0f)
        {
            if (zRot + add <= maxAngle || 
                zRot + add >= 360 + minAngle)
            {
                zRot += add;
            }
            else zRot = maxAngle;
            print("Forward");
        }

        if (_Input.crankAxis < 0f)
        {
            if (rot.z - add >= 360 + minAngle || 
                rot.z - add <= maxAngle)
            {
                zRot -= add;
            }
            else zRot = minAngle;
            print("Reverse");
        }
        */
        /*if (zRot <= 360 + minAngle || zRot >= maxAngle)
        {
            zRot = minAngle;
        }

        if (zRot >= maxAngle || zRot <= 360 + minAngle)
        {
            zRot = maxAngle;
        }*/
        
        
        
        Vector3 targetDir =  stickedHandsTransforms[0].position - crankBase.transform.position;
        Vector3 forward = crankBase.transform.forward;
        float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
        print(angle);
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        //Implementation of the deadzone
        //crankBase.transform.localEulerAngles = new Vector3(zRot,transform.localEulerAngles.y , transform.localEulerAngles.z);
        //if (Mathf.Abs(zRot) >= deadzoneAngle) crankBase.transform.localRotation = Quaternion.Euler(zRot, rot.y, rot.z);
        //else crankBase.transform.localRotation = Quaternion.Euler(0, rot.y, rot.z);
        
        //Sets power based on rotation
        //rot = transform.localRotation.eulerAngles;
        
        if (rot.x == 0) power = 0;
        else if (rot.x  <= 180) power = (rot.x) / maxAngle;
        else power = (360 - rot.x) / - (maxAngle);
    }
    
}
