using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndTrigger : MonoBehaviour
{
    public GameObject EndUI;
    public Text ChronoScore;
    public GameObject Music;
    public GameObject Jingle;

    public KeyCode BackToMenu;


    private void Awake()
    {
        Jingle.SetActive(false);
        EndUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            End();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(BackToMenu))
        {
            SceneManager.LoadScene(0);
        }
    }

    void End()
    {
        SoundManager.PlaySound(SoundManager.Sound.Explosion);

        Chrono.PauseChrono();

        Music.SetActive(false);

        StartCoroutine(EndCoroutine());
    }

    IEnumerator EndCoroutine()
    {
        yield return new WaitForSeconds(2);

        Jingle.SetActive(true);
        EndUI.SetActive(true);

        string t = string.Format("{0:0.00}", Chrono.Timer);
        t = t.Replace(",", ":");

        ChronoScore.text ="Time : "+ t;

        Controller.End = true;
    }
}
