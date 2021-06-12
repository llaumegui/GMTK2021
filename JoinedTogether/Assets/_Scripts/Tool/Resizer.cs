using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Resizer : MonoBehaviour
{
    public List<SpriteRenderer> Renderers;
    BoxCollider2D _collider;

    float _defaultSize = .32f;

    public bool Apply;

    private void OnValidate()
    {
        if (Apply)
            Apply = false;

        _collider = GetComponent<BoxCollider2D>();

        foreach(SpriteRenderer r in Renderers)
        {
            r.size = _defaultSize * _collider.size;
        }
    }
}
