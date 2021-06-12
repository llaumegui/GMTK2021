using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Puppet PuppetScript;

    public int Life = 1;

    public Transform MagicianFoot;
    public LayerMask GroundLayer;
    public Transform MagicianView;

    [Header("Controls Magician Values")]
    public float Speed;
    public float ViewIntensity;
    [Range(0,1)] public float LerpIntensity;
    [Range(1,10)] public float ProjectionPower;

    [Header("Keycodes")]
    public KeyCode ActionKey;
    public KeyCode HoldKey;

    Rigidbody2D _magicianRb;

    [Header("Animation")]
    public GameObject ProjectionVFX;
    Animator _anim;

    float _x;
    [HideInInspector] public float X { get { return _x; } } 
    float _y;
    bool _dead;
    [HideInInspector] public Vector3 LastCheckpoint = Vector3.up;

    bool _canJump;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _magicianRb = GetComponent<Rigidbody2D>();

        ProjectionVFX.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!_dead)
        {
            Inputs();

            if (IsGrounded())
            {
                _canJump = true;

                _anim.SetBool("Projection", false);

                if(_magicianRb.velocity.magnitude<5)
                    ProjectionVFX.SetActive(false);
            }
            else
            {
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

        transform.position += Vector3.right * _x * Speed * Time.deltaTime;

        MagicianView.transform.position = transform.position + Vector3.up * _y * ViewIntensity;

        if(_x<0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;


        if(Input.GetKeyDown(ActionKey))
            Propulsion();
    }

    void Propulsion()
    {
        if (PuppetScript == null)
            return;

        if(_canJump)
        {
            _canJump = false;

            _magicianRb.AddForce(PuppetScript.Rb.velocity * ProjectionPower, ForceMode2D.Impulse);
            PuppetScript.Rb.velocity = Vector3.zero;

            ProjectionVFX.SetActive(true);
        }
    }

    public bool IsGrounded()
    {
        if (Physics2D.Raycast(MagicianFoot.transform.position, Vector3.down, .05f, GroundLayer))
            return true;

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ennemy")
        {
            Death();
        }
    }

    void Death()
    {
        _dead = true;
        _anim.SetBool("Dead", true);
        _anim.SetBool("Projection", false);
        ProjectionVFX.SetActive(false);
    }

    void OnAnimationEnd()
    {
        _anim.SetBool("Dead", false);
        _dead = false;
        Respawn();
    }

    void Respawn()
    {
        transform.position = LastCheckpoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(MagicianFoot.transform.position, Vector3.down * .05f);
    }

}
