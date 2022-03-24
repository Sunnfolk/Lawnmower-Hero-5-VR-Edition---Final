using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FollowHandWhyWhenSticked : MonoBehaviour
{
    public List<Transform> _stickedHandsTransforms = new List<Transform>(2);
    
    private void OnStickedHandsChanged(InteractAble.Hand[] stickedHands)    //set hand transform List every time hand is changed
    {
        foreach (InteractAble.Hand stickedHand in stickedHands)
        {
            if (stickedHand.Transform != null)
            {
                _stickedHandsTransforms.Add(stickedHand.Transform);
            }
            else
            {
                _stickedHandsTransforms.Remove(stickedHand.LastFrameStickedHandTransform);
            }
        }
    }

    private void Update()   //set y pos to average y of every hand sticked to object
    {
        if (_stickedHandsTransforms.Count == 0)
            return;
        
        float handsSum = 0f;
        foreach (Transform trans in _stickedHandsTransforms)
        {
            handsSum += trans.position.y;
            
        }
        transform.position = new Vector3(transform.position.x, handsSum /_stickedHandsTransforms.Count, transform.position.z);
    }
}
