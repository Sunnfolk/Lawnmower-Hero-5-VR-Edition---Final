using System.Collections.Generic;
using UnityEngine;

public class Gascrank : MonoBehaviour
{
    [Header("Hand")]
    public List<Transform> handsTransforms = new List<Transform>();
    public bool handSticked = false;
    public Transform trackedHand;

    private float _angleStickyOffset; // offset between wheel rotation and hand position on grab
    private float _wheelLastSpeed; // wheel speed at the moment of ungrab, then gradually decreases due to INERTIA
    private static float inertia = 0.95f; // 1-wheel never stops // 0 - wheel stops instantly
    
    [Tooltip("Maximum rotation of the wheel")]
    public float maxRotation = 20; 
    private static float wheelHapticFrequency = 360/12; //every wheel will click 12 times per wheel rotation

    [Header("Steering Wheel Relative Point")]
    public GameObject crankTarget;

    [Header("Wheel & Hand relative position")]
    public Vector3 relativePos;

    [Header("Output steering wheel angle")]
    public float outputAngle = 0;

    public float targetAngle = 0;
    public float deadZone = 5;
    
    public TextMesh textDisplay;

    //public SteeringWheelOutPut steeringWheelOutPut; Todo;

    [Header("Arrays Values (Debug)")]
    public List<float> lastValues = new List<float>(); // stores last angles
    public List<float> diffs = new List<float>(); // stores difference between each last angles
    public List<float> formulaDiffs = new List<float>(); // stores formulated diffs
    public List<float> increment = new List<float>(); // calculating incrementation

    private void Start()
    {
        CreateArrays(5); // CALLING FUNCTION WHICH CREATES ARRAYS
        _angleStickyOffset = 0f;
        handSticked = false;
        _wheelLastSpeed = 0;
    }

    private void OnStickedHandsChanged(InteractAble.Hand[] stickedHands)
    {
        foreach (InteractAble.Hand hand in stickedHands)
        {
            if (hand.Transform != null)
            {
                print(hand.Transform);
                
                handsTransforms.Add(hand.Transform);
                
                handSticked = true;
                CalculateOffset();
            }
            else
            {
                print(hand.LastFrameStickedHandTransform);
                
                handsTransforms.Remove(hand.LastFrameStickedHandTransform);
                handSticked = false;
                _wheelLastSpeed = outputAngle - lastValues[3];
            }
        }
    }

    private void CalculateOffset()
    {
        float rawAngle = CalculateRawAngle();
        _angleStickyOffset = outputAngle - rawAngle;
    }

    private float CalculateRawAngle()
    {
        relativePos = crankTarget.transform.InverseTransformPoint(handsTransforms[0].position); // GETTING RELATIVE POSITION BETWEEN STEERING WHEEL BASE AND HAND
        
        
        return Mathf.Atan2( relativePos.x, relativePos.z) * Mathf.Rad2Deg; // GETTING CIRCULAR DATA FROM X & Z RELATIVES  VECTORS
    }
    
    private void FixedUpdate()
    {
        //steeringWheelOutPut.outAngle = outputAngle; Todo;
        float angle;
        if (handSticked)
        {
            angle = CalculateRawAngle() + _angleStickyOffset; // When hands are holding the wheel hand dictates how the wheel moves
            // angleSticky Offset is calculated on wheel grab - makes wheel not to rotate instantly to the users hand
        }
        else
        {
            // when wheel is released we apply a little bit of inertia
            angle = targetAngle + _wheelLastSpeed; //last wheel speed is updated when wheel is ungrabbed and then gradually returns to zero
            _wheelLastSpeed *= inertia;
        }
        
        lastValues.RemoveAt(0); // REMOVING FIRST ITEM FROM ARRAY
        lastValues.Add(angle); // ADD LAST ITEM TO ARRAY

        targetAngle = HookedAngles(angle);// SETTING OUTPUT THROUGH FUNCTION
        if (textDisplay != null){
            textDisplay.text = Mathf.Round(outputAngle) + "" + ".00 deg. speed " + _wheelLastSpeed;
        }

        #region DeadZone
        
        if (targetAngle <= deadZone && targetAngle>= -deadZone)
        {
            //print("In dead zone, don't change crank pos");
            outputAngle = 0f;
        }
        else
        {
            outputAngle = targetAngle;
        }
        
        #endregion
        
        transform.localEulerAngles = new Vector3(crankTarget.transform.localEulerAngles.y, outputAngle, crankTarget.transform.localEulerAngles.z);// ROTATE WHEEL MODEL FACING TO THE PLAYER
        
        //transform.RotateAround(WheelBase.transform.position, Vector3.right, Diffs[^1]);
        
        float hapticSpeedCoeff = Mathf.Abs(lastValues[4] - lastValues[3]) + 1;
        if (Mathf.Abs(outputAngle % wheelHapticFrequency) <= hapticSpeedCoeff &&
            Mathf.Abs(lastValues[3] % wheelHapticFrequency) > hapticSpeedCoeff)
        {
            /*if (TrackedController != null)
            {
                TrackedController.TriggerHapticPulse(1000);
            }Todo Haptics?*/
        }
        
    }
    
    private void CreateArrays(int firstPparam) // FUNCTION WHICH CREATING ARRAYS
    {
        for (int i = 0; i < firstPparam; i++) // FOR LOOP WITH PARAM
        {
            lastValues.Add(0);  // LAST VALUES ARRAY
        }


        for (int i = 0; i < firstPparam - 1; i++) // FOR LOOP WITH PARAM -1
        {
            diffs.Add(0); // ARRAY TO STORE DIFFERENCE BETWEEN NEXT AND PREV
            formulaDiffs.Add(0); // ARRAY TO STORE FORMULATED DIFFS
            increment.Add(0); //  ARRAY TO STORE INCREMENT FOR EACH WHEEL SPIN

        }
    }

    public float HookedAngles(float angle) // FORMULATING AND CALCULATING FUNCTION WHICH COUNTS SPINS OF WHEEL//Also applying rotation limits
    {
        float period = 360;
        for (int i = 0; i < lastValues.Count - 1; i++)
        {
            diffs.RemoveAt(0);
            diffs.Add(lastValues[i + 1] - lastValues[i]);
        }

        for (int i = 0; i < formulaDiffs.Count; i++)
        {
            formulaDiffs.RemoveAt(0);
            var a = (diffs[i] + period / 2.0f);
            var b = period;
            var fdiff = a - Mathf.Floor(a / b) * b;
            formulaDiffs.Add(fdiff - period / 2);
        }

        for (int i = 0; i < formulaDiffs.Count; i++)
        {
            increment.RemoveAt(0);
            increment.Add(formulaDiffs[i] - diffs[i]);
        }

        for (int i = 1; i < formulaDiffs.Count; i++)
        {
            increment[i] += increment[i - 1];
        }

        lastValues[4] += increment[3];

        if (Mathf.Abs(lastValues[4]) > maxRotation)
        {
            lastValues[4] = lastValues[3];
            /*if (TrackedController != null)
            {
                TrackedController.TriggerHapticPulse(500);

            } Todo Haptics?*/
        }
        return lastValues[4]; // CALIBRATE TO ZERO WHEN STILL AND RETURN CALCULATED VALUE
        
    }
}
