using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CollisionDetector))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _groundDrag = 8;
    [SerializeField] private float _gravityFactor = 3;

    private Rigidbody _rigidbody;
    private Transform _transform;
    private CollisionDetector _collisionDetector;

    private Vector3 _targetDirection;
    private Vector3 _verticalVelocity;

    private float _planeVelocityMultiplier = 9f;
    private float _slopeVelocityMultiplier = 4f;
    private float _stairsVelocityMultiplier = 17f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();

        _collisionDetector = GetComponent<CollisionDetector>();

        _verticalVelocity = Physics.gravity * _gravityFactor;
    }

    private void FixedUpdate()
    {
        _collisionDetector.CheckGround();

        if (_collisionDetector.IsOnGround)
        {
            _rigidbody.drag = _groundDrag;
        }
        else
        {
            _rigidbody.drag = 0;
            _rigidbody.AddForce(_verticalVelocity, ForceMode.Force);
        }
    }

    public void MoveTowardsTarget(Transform target)
    {
        _targetDirection = new Vector3(target.position.x - _transform.position.x, 0,
            target.position.z - _transform.position.z).normalized;

        if (_collisionDetector.IsOnSlope)
        {
            _rigidbody.useGravity = false;
            _rigidbody.AddForce(GetSlopeMovementDirection() * _speed * _slopeVelocityMultiplier, ForceMode.Force);
        }
        else if (_collisionDetector.IsBeforeStairs())
        {
            _rigidbody.useGravity = false;
            _rigidbody.AddForce(_transform.up * _stairsVelocityMultiplier * _collisionDetector.StepHeight * _speed, ForceMode.Force);
        }
        else
        {
            _rigidbody.useGravity = true;
            _rigidbody.AddForce(_targetDirection * _planeVelocityMultiplier * _speed, ForceMode.Force);
        }

        LimitVelocity();
    }

    private Vector3 GetSlopeMovementDirection()
    {
        return Vector3.ProjectOnPlane(_targetDirection, _collisionDetector.GroundHitInfo.normal).normalized;
    }

    private void LimitVelocity()
    {
        Vector3 currentVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

        if (currentVelocity.magnitude > _speed)
        {
            Vector3 maximalVelocity = currentVelocity.normalized * _speed;
            _rigidbody.velocity = new Vector3(maximalVelocity.x, _rigidbody.velocity.y, maximalVelocity.z);
        }
    }
}

