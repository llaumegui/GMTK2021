using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour
{
    public LineRenderer LeftLine;
    public LineRenderer RightLine;

    public Transform LeftSource;
    public Transform RightSource;

    public Transform LeftTarget;
    public Transform RightTarget;

    private void Update()
    {
        LeftLine.SetPosition(0, LeftSource.position);
        LeftLine.SetPosition(1, LeftTarget.position);
        RightLine.SetPosition(0, RightSource.position);
        RightLine.SetPosition(1, RightTarget.position);
    }
}
