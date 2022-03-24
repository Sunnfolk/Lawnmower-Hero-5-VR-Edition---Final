using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HandVisual : MonoBehaviour
{
    /*
    Contains the things needed to have a visual model of the hand opening and closing, and interacting with the HandFunctionality script
    */
    
    [Header("Hand Meshes")]
    [Tooltip("Default for hand when nothing is pressed")]
    [SerializeField] private Mesh _openHandDefault;
    [Tooltip("Default for hand when trigger is down")]
    [SerializeField] private Mesh _closedHandDefault;
    
    private MeshFilter _meshFilter;
    private Vector3 _scaleOnAwake;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _scaleOnAwake = transform.localScale;
    }

    private void OnValidate()
    {
        //Set hand to open hand default in sceneview already
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter.sharedMesh != _openHandDefault)
        {
            meshFilter.sharedMesh = _openHandDefault;
        }
        //Send warning if no default mesh for open and closed hand
        if (_openHandDefault == null || _closedHandDefault == null)
        {
            Debug.LogWarning($"Default hand(s) in {GetType().Name} in {gameObject.name} is not set. " +
                             $"This is dangerous and can lead the hand to not being rendered");
        }
    }

    public void SetOpenHandMesh(Mesh mesh)  //Sets open hand mesh, but is null checked and defaults to open hand default
    {
        _meshFilter.mesh = (mesh == null) ? _openHandDefault : mesh;
    }

    public void SetClosedHandMesh(Mesh mesh)    //Sets Closed hand mesh, but is null checked and defaults to Closed hand default
    {
        _meshFilter.mesh = (mesh == null) ? _closedHandDefault : mesh;
    }

    public void ResetScaleToAwakeScale()
    {
        transform.localScale = _scaleOnAwake;
    }
}
