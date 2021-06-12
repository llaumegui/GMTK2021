using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppet : MonoBehaviour
{
    public Controller ControllerScript;

    GameObject _magician;
    DistanceJoint2D _joint;
    Rigidbody2D _rb;
    public float RopeLength;

    [Header("Friction")]
    public float MaxDistanceFriction;
    [Range(0, 1)] public float FrictionLerp;
    float _defaultFriction;

    [Header("Renderer")]
    public SpriteRenderer PuppetRenderer;
    Sprite _spriteDeactivated;
    public List<Sprite> ActivatedPuppet;

    private void Start()
    {
        _spriteDeactivated = PuppetRenderer.sprite;
        _joint = GetComponent<DistanceJoint2D>();
        _rb = GetComponent<Rigidbody2D>();

        _defaultFriction = _rb.drag;
        _magician = ControllerScript.gameObject;
        _joint.distance = RopeLength;
    }

    private void Update()
    {
        _joint.connectedAnchor = _magician.transform.position;


        if(AngularDrag())
        {
            _rb.drag = Mathf.Lerp(_rb.drag, 1, FrictionLerp * Time.deltaTime);
        }
        else
        {
            _rb.drag = _defaultFriction;
        }

        //renderers
        if (_rb.velocity.x > 0)
            PuppetRenderer.sprite = ActivatedPuppet[1];
        else
            PuppetRenderer.sprite = ActivatedPuppet[0];

        if (_rb.drag > .75f)
            PuppetRenderer.sprite = _spriteDeactivated;

    }

    bool AngularDrag()
    {
        bool result = false;

        if(_rb.position.y> MaxDistanceFriction)
        {
            result = true;
        }

        if (ControllerScript.X == 0)
            result = true;

        return result;
    }
}
