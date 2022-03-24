
using UnityEngine;

public class GnomeTilt : MonoBehaviour
{
    [SerializeField] private LayerMask groundEquals;
    
    private Quaternion slopeRotation;

    // Update is called once per frame
    void Update()
    {
        RayCast();
    }

    private void RayCast()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundEquals);
        // Gets the slopeRotation through the raycast, then Slerps (Smooths out) the rotation and then sets the lawnmowers rotation to the ground rotation
        slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, 1f * Time.deltaTime);
    }
}
