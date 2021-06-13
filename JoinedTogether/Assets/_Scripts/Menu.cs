using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    public KeyCode StartButton;

    public SpriteRenderer TextRenderer;

    private void Start()
    {
        TextRenderer.DOFade(0, 0);
        TextRenderer.DOFade(1, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(StartButton))
            SceneManager.LoadScene(1);
    }
}
