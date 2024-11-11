using System;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private Transform _upperStepPoint;
    [SerializeField] private Transform _lowerStepPoint;

    [SerializeField] private float _radius;
    [SerializeField] private float _height;
    
    [SerializeField] private float _width = 0.08f;
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private float _stepHeight = 0.3f;

    private Collider _collider;

    private bool _isOnGround;
    private bool _isOnSlope;

    private float _heightMultiplier;
    private float _widthMultiplier;
    private float _slopeAngle;
    private float _groundDistance;

    private RaycastHit _groundHitInfo;

    public RaycastHit GroundHitInfo => _groundHitInfo;
    
    public bool IsOnGround => _isOnGround;
    public bool IsOnSlope => _isOnSlope;
    
    public float StepHeight => _stepHeight;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        _upperStepPoint.position = new Vector3(_upperStepPoint.position.x, _lowerStepPoint.position.y + _stepHeight,
            _upperStepPoint.position.z);
    }

    public bool IsBeforeStairs()
    {
        float lowerRayLength = 0.1f;
        float upperRayLength = 0.2f;
        float lowerRayCorrectedLength = lowerRayLength + _radius - _lowerStepPoint.localPosition.z;

        if (Physics.Raycast(_lowerStepPoint.position, _lowerStepPoint.forward, out _, lowerRayCorrectedLength,
                _groundLayer))
            return Physics.Raycast(_upperStepPoint.position, _upperStepPoint.forward, out _, upperRayLength,
                _groundLayer);

        return false;
    }

    public void CheckGround()
    {
        _heightMultiplier = 0.5f;
        _widthMultiplier = 2f;
        _groundDistance = _height * _heightMultiplier - _radius + _width * _widthMultiplier;

        if (Physics.SphereCast(_collider.bounds.center, _radius - _width, Vector3.down, 
                out _groundHitInfo, _groundDistance, _groundLayer))
        {
            _isOnGround = true;
            _slopeAngle = Vector3.Angle(Vector3.up, _groundHitInfo.normal);

            if (_slopeAngle < _maxSlopeAngle && _slopeAngle != 0)
                _isOnSlope = true;
            else
                _isOnSlope = false;
        }
        else
        {
            _isOnGround = false;
            _isOnSlope = false;
        }
    }
}
