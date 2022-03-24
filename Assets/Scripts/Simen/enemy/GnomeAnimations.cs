using UnityEngine;

public class GnomeAnimations : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private EnemyMovement _enemyMovement;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_enemyMovement.inCombat)
        {
            print("Walking");
            _animator.Play("Gnome_Walk");
        }
        else if (!_enemyMovement.inCombat)
        {
            print("Standing still");
            _animator.Play("Gnome_Idle");
        }
    }
}