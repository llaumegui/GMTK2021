using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool _activated;
    SpriteRenderer _renderer;
    GameObject _activationObject;

    private void Awake()
    {
        _activationObject = transform.GetChild(0).gameObject;
        _renderer = GetComponent<SpriteRenderer>();
        _activationObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !_activated)
        {
            _activated = true;
            _renderer.enabled = false;
            _activationObject.SetActive(true);

            SoundManager.PlaySound(SoundManager.Sound.Checkpoint);


            if(collision.gameObject.TryGetComponent(out Controller script))
            {
                script.LastCheckpoint = transform.position;
            }
        }
    }
}
