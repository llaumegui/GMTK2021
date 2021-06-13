using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chrono : MonoBehaviour
{
    public static float Timer;

    Text _text;

    public static bool Play;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        if(Play)
        {
            Timer += Time.deltaTime;

            string t = string.Format("{0:0.00}", Timer);
            t = t.Replace(",", ":");

            _text.text = t;
        }
    }


    public static void PlayChrono()
    {
        Play = true;
    }

    public static void PauseChrono()
    {
        Play = false;
    }

    public static void ResetChrono()
    {
        Play = false;
        Timer = 0;
    }


}
