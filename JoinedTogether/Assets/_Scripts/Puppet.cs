using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppet : MonoBehaviour
{
    public Controller ControllerScript;

    GameObject _magician;
    DistanceJoint2D _joint;
    [HideInInspector] public Rigidbody2D Rb;
    public float RopeLength;

    [Header("Friction")]
    public float MaxDistanceFriction;
    [Range(0, 1)] public float FrictionLerp;
    float _defaultFriction;

    [Header("Renderer")]
    public SpriteRenderer PuppetRenderer;
    Sprite _spriteDeactivated;
    public List<Sprite> ActivatedPuppet;

    [Header("Hands")]
    public Transform LeftHandUp;
    public Transform RightHandUp;
    public Transform LeftHand;
    public Transform RightHand;
    Vector3 _leftHandDefaultPos;
    Vector3 _rightHandDefaultPos;

    private void Start()
    {
        _spriteDeactivated = PuppetRenderer.sprite;
        _joint = GetComponent<DistanceJoint2D>();
        Rb = GetComponent<Rigidbody2D>();

        _defaultFriction = Rb.drag;
        _magician = ControllerScript.gameObject;
        _joint.distance = RopeLength;

        _leftHandDefaultPos = LeftHand.localPosition;
        _rightHandDefaultPos = RightHand.localPosition;
    }

    private void Update()
    {
        _joint.connectedAnchor = _magician.transform.position;


        if(AngularDrag())
        {
            Rb.drag = Mathf.Lerp(Rb.drag, 1, FrictionLerp * Time.deltaTime);
        }
        else
        {
            Rb.drag = _defaultFriction;
        }

        //renderers
        if (Rb.velocity.x > 0)
        {
            PuppetRenderer.sprite = ActivatedPuppet[1];
            LeftHand.position = LeftHandUp.position;
            RightHand.position = RightHandUp.position;

        }
        else
        {
            PuppetRenderer.sprite = ActivatedPuppet[0];

            LeftHand.localPosition = _leftHandDefaultPos;
            RightHand.localPosition = _rightHandDefaultPos;
        }

        if (Rb.drag > .75f)
        {
            PuppetRenderer.sprite = _spriteDeactivated;

            LeftHand.localPosition = _leftHandDefaultPos;
            RightHand.localPosition = _rightHandDefaultPos;
        }

    }

    bool AngularDrag()
    {
        bool result = false;

        if(Rb.position.y> MaxDistanceFriction)
        {
            result = true;
        }

        if (ControllerScript.X == 0)
            result = true;

        return result;
    }
}
