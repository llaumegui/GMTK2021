using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static bool End;

    public Puppet PuppetScript;

    public int Life = 1;

    public Transform MagicianFoot;
    public LayerMask GroundLayer;
    public Transform MagicianView;

    [Header("Controls Magician Values")]
    public float Speed;
    public float ViewIntensity;
    [Range(0,1)] public float LerpIntensity;
    Vector3 _momentum;


    [Header("Projection")]
    [Range(1, 50)] public float ProjectionPower;
    public AnimationCurve PowerCurve;

    [Header("Keycodes")]
    public KeyCode ActionKey;
    public KeyCode HoldKey;
    public KeyCode RestartKey;

    Rigidbody2D _magicianRb;

    [Header("Animation")]
    public GameObject ProjectionVFX;
    Animator _anim;

    float _x;
    [HideInInspector] public float X { get { return _x; } } 
    float _y;
    bool _dead;
    bool _walkSoundAntispam;
    
    public Vector3 LastCheckpoint = Vector3.up;

    bool _canJump;

    bool _inAir;
    bool _propulsionAntispam;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _magicianRb = GetComponent<Rigidbody2D>();

        ProjectionVFX.SetActive(false);
        End = false;
    }

    private void Update()
    {
        if(!_dead && !End)
        InputActions();
    }

    private void InputActions()
    {
        if (Input.GetKeyDown(RestartKey))
        {
            Respawn();
            return;
        }

        if (Input.GetKeyDown(ActionKey) && !_propulsionAntispam)
            Propulsion();
    }

    private void FixedUpdate()
    {
        if (!_dead && !End)
        {
            Inputs();

            if (IsGrounded())
            {
                if(!_propulsionAntispam)
                    _canJump = true;

                _anim.SetBool("Projection", false);

                if(_magicianRb.velocity.magnitude<5)
                    ProjectionVFX.SetActive(false);

                _momentum = Vector3.Lerp(_momentum, Vector3.zero, LerpIntensity/3);

                if(_inAir)
                {
                    _inAir = false;
                    SoundManager.PlaySound(SoundManager.Sound.OnLand);
                }
            }
            else
            {
                _inAir = true;
                _anim.SetBool("Projection", true);
            }
        }
    }

    void Inputs()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _x = Mathf.Lerp(_x, x, LerpIntensity);
        _y = Mathf.Lerp(_y, y, LerpIntensity);

        if (_x < .05f && _x > -.05f)
            _x = 0;
        if (_y < .05f && _y > -.05f)
            _y = 0;

        transform.position += Vector3.right * _x * Speed * Time.deltaTime + _momentum;

        MagicianView.transform.position = transform.position + Vector3.up * _y * ViewIntensity;

        if(_x<0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;


        if(!_walkSoundAntispam && x!=0 && IsGrounded())
        {
            StartCoroutine(WalkSound());
        }
    }

    IEnumerator WalkSound()
    {
        _walkSoundAntispam = true;
        SoundManager.PlaySound(SoundManager.Sound.Walk);
        yield return new WaitForSeconds(.25f);

        _walkSoundAntispam = false;
    }

    void Propulsion()
    {
        if (PuppetScript == null)
            return;



        if(_canJump)
        {
            float magnitude = PuppetScript.Rb.velocity.magnitude;
            if (magnitude > 4)
                magnitude = 4;
            magnitude /= 4;

            _magicianRb.AddForce(PuppetScript.Rb.velocity.normalized * ProjectionPower * PowerCurve.Evaluate(magnitude), ForceMode2D.Impulse);

            _momentum.x += PuppetScript.Rb.velocity.x/100;

            PuppetScript.Rb.velocity = Vector3.zero;

            ProjectionVFX.SetActive(true);
            SoundManager.PlaySound(SoundManager.Sound.Projection);

            if (_magicianRb.velocity.magnitude > 5)
                SoundManager.PlaySound(SoundManager.Sound.Momentum);

            StartCoroutine(PropulsionCooldown());
        }
    }

    IEnumerator PropulsionCooldown()
    {
        _canJump = false;
        _propulsionAntispam = true;
        yield return new WaitForSeconds(.5f);
        _propulsionAntispam = false;
    }

    public bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(MagicianFoot.transform.position, .1f, GroundLayer))
            return true;

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ennemy")
        {
            Death();

        }
    }

    void Death()
    {
        _magicianRb.constraints = RigidbodyConstraints2D.FreezePosition;

        _dead = true;

        SoundManager.PlaySound(SoundManager.Sound.Explosion);

        if (transform.position.y > 0)
            _anim.SetBool("Dead", true);
        else
            _anim.SetBool("DeadY", true);

        _anim.SetBool("Projection", false);
        ProjectionVFX.SetActive(false);
    }

    void OnAnimationEnd()
    {
        _anim.SetBool("Dead", false);
        _anim.SetBool("DeadY", false);
        _dead = false;
        Respawn();
    }

    void Respawn()
    {
        _magicianRb.constraints = RigidbodyConstraints2D.None;
        _magicianRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.position = LastCheckpoint;
        _momentum = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(MagicianFoot.transform.position, .1f);
    }

}
