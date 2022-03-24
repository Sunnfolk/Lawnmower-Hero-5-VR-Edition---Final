using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class RespawnAble : MonoBehaviour
{
    /*
    Class respawns gameobject if certain conditions are met
    */
    
    private enum RespawnCondition   //respawn enums conditions (enum nice readable)
    {
        OutOfBounds, PickupAbleHeldSinceRespawn, SecondsSincePickupAbleHeld, PickupAbleNotInHand, 
        SecondsSinceRespawn, NotVisibleByRenderer, NotInRespawnArea
    }

    [Serializable]
    private class AndCollection //Innermost array in Respawn conditions is a class since unity does not support serialization for nested lists or arrays, and it is also more readable in inspector because of tooltips
    {
        [Tooltip("And pairs of conditions (Both must be true for the whole statement to be true)")]
        public RespawnCondition[] conditions;

        public AndCollection()
        {
            conditions = new RespawnCondition[] { RespawnCondition.OutOfBounds };
        }
    }

    [Header("Respawn Options")]
    [Tooltip("The transform the GameObject gets respawned to (Automatically set to awake position if null)")]
    [SerializeField] private Transform _repawnTransform;
    [Tooltip("The time between each check for the respawn conditions")] 
    [SerializeField] private float _conditionCheckInterval = 1f;
    [Tooltip("Conditions that make the GameObject respawn (If any is true it will respawn) (Options for some conditions down below)")]
    [SerializeField] private AndCollection[] _respawnConditions = new AndCollection[1];

    [Header("Out Of Bounds Options")] 
    [Tooltip("Distance from relativeTransform needed to respawn")]
    [SerializeField] private float _respawnDistance = 2f;
    [Tooltip("Distance is measured from this relative transform (Defaulted to respawnTransform if null")]
    [SerializeField] private Transform _relativeTransform;

    [Header("Seconds Since PickupAble Held")]
    [Tooltip("Seconds before respawning after last held (will not respawn if not held since last spawn")]
    [SerializeField] private float _maxTimeSincePickupableLastHeld = 5f;

    [Header("Seconds Since Respawn Options")]
    [Tooltip("Seconds before respawning relative to last respawn")]
    [SerializeField] private float _maxTimeSinceRespawn = 15f;

    [Header("Not In Respawn Area Options")]
    [Tooltip("Distance object can be from RespawnTransform without triggering respawn")]
    [SerializeField] private float _distanceTolerance = 1f;

    private Rigidbody _rigidBody;
    private PickupAble _pickupAble;
    private Renderer _renderer;
    private Vector3 _awakePos;
    private Quaternion _awakeRot;
    
    public float LastRespawnFixedTime { get; private set; }

    private Vector3 RespawnPos  //nullsafed with awakePos, but still works dynamically if transform not null
    {
        get
        {
            return (_repawnTransform == null) ? _awakePos : _repawnTransform.position;
        }
    }

    private Vector3 RelativePos //nullsafed with RespawnPos, but still works dynamically if transform not null
    {
        get
        {
            return (_relativeTransform == null) ? RespawnPos : _relativeTransform.position;
        }
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _pickupAble = GetComponent<PickupAble>();
        _renderer = GetComponent<Renderer>();
        _awakePos = transform.position;
        _awakeRot = transform.rotation;
    }

    private void OnEnable() //repeat respawn check at time intervals (to save performance with many objects
    {
        InvokeRepeating(nameof(CheckForRespawnInvokeRepeating), 0f, _conditionCheckInterval);
    }

    private void OnDisable()    //Cancel invoke on disable to only have it running when enabled
    {
        CancelInvoke(nameof(CheckForRespawnInvokeRepeating));
    }

    private void OnValidate()
    {
        //This script has no function if respawnConditions is empty, so the smallest size is 1 for both and the nested array
        if (_respawnConditions == null || _respawnConditions.Length < 1)
        {
            _respawnConditions = new AndCollection[1] { new AndCollection() };
        }
        foreach (AndCollection andCollection in _respawnConditions)
        {
            if (andCollection.conditions == null || andCollection.conditions.Length < 1)
            {
                andCollection.conditions = new RespawnCondition[1] {RespawnCondition.OutOfBounds};
            }
        }
        
        //Gameobject must have pickupAble script to use SecondsSincePickupAbleLastHeld
        foreach (AndCollection andCollection in _respawnConditions)
        {
            if (-1 == Array.FindIndex(andCollection.conditions, f =>
                f == RespawnCondition.SecondsSincePickupAbleHeld || f == RespawnCondition.PickupAbleHeldSinceRespawn
                                                                 || f == RespawnCondition.PickupAbleNotInHand))
                break;
            if (TryGetComponent(out PickupAble p))
                break;
            Debug.LogWarning($"{gameObject.name} needs a PickupAble component to be compatable with " +
                                 $"{RespawnCondition.SecondsSincePickupAbleHeld.ToString()} in {GetType().Name}. " +
                                 "Not adding the component will cause Errors in runtime.");
            break;
            
        }
    }

    public void Respawn()   //respawns the object (made public so other scripts can force respawn if functionality needed
    {
        transform.position = RespawnPos;
        transform.rotation = (_repawnTransform == null) ? _awakeRot : _repawnTransform.rotation;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        LastRespawnFixedTime = Time.fixedTime;

        if (_pickupAble == null)
            return;
        _pickupAble.heldSinceRespawn = false;
        _pickupAble.SetPickedUpYetState(false);
    }

    private void CheckForRespawnInvokeRepeating()   //Respawns the gameobject if any of the inspector defined states are all true
    {
        //CHECK RESPAWN CONDITIONS
        foreach (AndCollection andCollection in _respawnConditions)
        {
            //Generate array of bools for each condition
            RespawnCondition[] conditions = andCollection.conditions;
            bool[] conditionStates = new bool[conditions.Length];
            for (int i = 0; i < conditions.Length; ++i)
            {
                conditionStates[i] = CheckRespawnConditionState(conditions[i]);
            }
            //respawn if all booleans in array is true
            if (Array.TrueForAll(conditionStates, b => b))
            {
                Respawn();
                break;
            }
        }
    }

    private bool CheckRespawnConditionState(RespawnCondition respawnCondition)  //Gives a bool state for weither the input state is true
    {
        switch (respawnCondition)
        {
            case RespawnCondition.OutOfBounds:  //OutOfBounds
                return Vector3.Distance(RelativePos, transform.position) > _respawnDistance;
            
            case RespawnCondition.PickupAbleHeldSinceRespawn:   //PickupAbleHeldSinceRespawn
                return _pickupAble.heldSinceRespawn;
            
            case RespawnCondition.SecondsSincePickupAbleHeld:   //SecondsSincePickupAbleHeld
                return Time.fixedTime - _pickupAble.lastHeldFixedTime > _maxTimeSincePickupableLastHeld;
            
            case RespawnCondition.PickupAbleNotInHand:  //PickupAbleNotInHand
                return _pickupAble.currentHeldByHand == null;
            
            case RespawnCondition.SecondsSinceRespawn:  //SecondsSinceRespawn
                return Time.fixedTime - _maxTimeSinceRespawn > _maxTimeSinceRespawn;
            
            case RespawnCondition.NotVisibleByRenderer: //NotVisibleByRenderer
                return !_renderer.isVisible;
            
            case RespawnCondition.NotInRespawnArea: //NotVisibleByRenderer
                return Vector3.Distance(RespawnPos, transform.position) > _distanceTolerance;
            
            default:    //default
                Debug.LogWarning($"RespawnCondition \"{respawnCondition.ToString()}\" " +
                                 $"not recognized in switch statement in {gameObject.name} in {GetType().Name}");
                return false;
        }
    }
}
