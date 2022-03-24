using UnityEngine;

public class RollCredits : MonoBehaviour
{
    [SerializeField] private float scrollSpd = 60f;
    [SerializeField] private float endOfCredits;

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y <= endOfCredits) transform.localPosition += new Vector3(0f, scrollSpd * Time.deltaTime, 0f);
        else print("End");
    }
}
