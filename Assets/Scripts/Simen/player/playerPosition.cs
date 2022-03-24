using UnityEngine;

public class playerPosition : MonoBehaviour
{
    public transformVariable trans;

    private void Update()
    {
        trans.playerTransform = transform;
    }
}