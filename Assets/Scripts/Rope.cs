using System;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public float length;
    public Transform attachedTo;
    public Transform negateParent;
    
    bool _isAttachedToObject;
    Vector3 _initialPos;
    Vector3 AttachPos => _isAttachedToObject ? attachedTo.position : _initialPos;

    [SerializeField] bool doInitLength;

    void Start()
    {
        if (negateParent != null)
        {
            _lastParentPos = negateParent.position;
            _negateParent = true;
        }

        _isAttachedToObject = attachedTo != null;
        _initialPos = transform.position;
    }

    void OnValidate()
    {
        if (doInitLength)
        {
            doInitLength = false;
            InitLength();
        }
    }

    void InitLength()
    {
        length = attachedTo == null ? 0 : (attachedTo.transform.position - transform.position).magnitude;
    }

    Vector3 _lastParentPos;
    bool _negateParent;
    void Update()
    {
        if (_negateParent)
        {
            transform.position += _lastParentPos - negateParent.position;
            _lastParentPos = negateParent.position;
        }
        var vec = AttachPos - transform.position;
        vec.z = 0;
        
        var angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (vec.magnitude < length) return;
        transform.position += vec.normalized * (vec.magnitude - length);
    }
}