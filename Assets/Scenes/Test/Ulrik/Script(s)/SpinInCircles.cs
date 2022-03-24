using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinInCircles : MonoBehaviour
{
    [SerializeField]private float rotateSpeed = 20f;
    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime,Space.World);
    }
}
