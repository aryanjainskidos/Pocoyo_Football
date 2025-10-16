using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransform : MonoBehaviour {

    [Tooltip("Displace Local Position Of GameObject On Demand")]
    public Vector2 PositionDelta;
    [Tooltip("Multiply Local Scale Of GameObject On Demand")]
    public Vector2 ScaleDelta;
    [Tooltip("Displace Local Z Rotation Of GameObject On Demand")]
    public float RotateDeltaZ;

    private RectTransform _transform;
    private Vector2 _OldPosition;
    private Vector2 _OldScale;
    private float _OldZRotation;

    private bool _positionActivated;
    private bool _scaleActivated;
    private bool _rotationActivated;

    public void DoPosition()
    {
        _positionActivated = true;
        _transform.localPosition = _OldPosition + PositionDelta;
    }

    public void UndoPosition()
    {
        _positionActivated = false;
        _transform.localPosition = _OldPosition;
    }

    public void DoScale()
    {
        _scaleActivated = true;
        _transform.localScale = _OldScale * ScaleDelta;
    }

    public void UndoScale()
    {
        _scaleActivated = false;
        _transform.localScale = _OldScale;
    }

    public void DoRotation()
    {
        _rotationActivated = true;
        _transform.localRotation = Quaternion.Euler(0f, 0f, _OldZRotation + RotateDeltaZ);
    }

    public void UndoRotation()
    {
        _rotationActivated = false;
        _transform.localRotation = Quaternion.Euler(0f, 0f, _OldZRotation);
    }

    public void Enter()
    {
        if(_positionActivated)
            _transform.localPosition = _OldPosition + PositionDelta;

        if (_scaleActivated)
            _transform.localScale = _OldScale * ScaleDelta;

        if (_rotationActivated)
            _transform.localRotation = Quaternion.Euler(0f, 0f, _OldZRotation + RotateDeltaZ);
    }

    public void Exit()
    {
        if (_positionActivated)
            _transform.localPosition = _OldPosition;
        if (_scaleActivated)
            _transform.localScale = _OldScale;
        if (_rotationActivated)
            _transform.localRotation = Quaternion.Euler(0f, 0f, _OldZRotation);
    }
    // Use this for initialization
    void Start () {
        _transform = GetComponent<RectTransform>();

        _OldPosition = _transform.localPosition;
        _OldScale = _transform.localScale;
        _OldZRotation = _transform.localRotation.eulerAngles.z;
	}
}
