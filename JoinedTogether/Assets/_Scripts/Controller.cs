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


    Rigidbody2D _magicianRb;

    float _x;
    [HideInInspector] public float X { get { return _x; } } 
    float _y;
    bool _dead;
    bool _deathAntispam;

    private void Start()
    {
        _magicianRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (IsGrounded() && !_dead)
            Inputs();

        if(_dead && !_deathAntispam)
        {
            _deathAntispam = true;
            Death();
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
        _magicianRb.velocity = Vector2.right * _x * Speed;

        MagicianView.transform.position = transform.position + Vector3.up * _y * ViewIntensity;

        if(_x<0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    public bool IsGrounded()
    {
        if (Physics2D.Raycast(MagicianFoot.transform.position, Vector3.down, .05f, GroundLayer))
            return true;

        return false;
    }

    void Death()
    {
        //

        _deathAntispam = true;
        _dead = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(MagicianFoot.transform.position, Vector3.down * .05f);
    }

}
