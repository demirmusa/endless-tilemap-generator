using UnityEngine;
using System.Collections;

/** \brief Sample script to move a target */
public class TargetMovement : MonoBehaviour
{
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }


    void Update()
    {
        if (_cam == null) return;

        UpdateTargetPosition();
    }

    public void UpdateTargetPosition()
    {
        if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity))
        {
            if (hit.point != transform.position)
                transform.position = hit.point;
        }
    }
}