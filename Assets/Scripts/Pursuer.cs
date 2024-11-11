using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(Mover))]
public class Pursuer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _minTargetDistance = 4f;

    private Rotator _rotator;
    private Mover _mover;
    private Transform _transform;

    private float _targetDistance;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rotator = GetComponent<Rotator>();
        _mover = GetComponent<Mover>();
    }

    private void FixedUpdate()
    {
        _targetDistance = Vector3.Distance(_transform.position, _target.position);

        if (_target != null)
        {
            _rotator.RotateToTarget(_target);

            if (_minTargetDistance < _targetDistance)
                _mover.MoveTowardsTarget(_target);
        }
    }
}
