using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcess : MonoBehaviour
{
    static PostProcess _i;
    public static PostProcess I { get { return _i; } }

    public List<PostProcessProfile> Profiles;

    PostProcessVolume _volume;

    int _selected;

    private void Awake()
    {
        if (_i != null && _i != this)
            Destroy(gameObject);

        _i = this;
        DontDestroyOnLoad(this);


        _volume = GetComponent<PostProcessVolume>();

        Apply();

    }

    public void Increment(bool plus = true)
    {
        if (plus)
            _selected++;
        else
            _selected--;

        if (_selected >= Profiles.Count)
            _selected = 0;

        if (_selected < 0)
            _selected = Profiles.Count - 1;

        Apply();
    }

    void Apply()
    {
        _volume.profile = Profiles[_selected];
    }
}
