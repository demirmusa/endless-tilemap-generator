using System.Collections;
using UnityEngine;

/** \brief Camera Following */
public class CameraFollow : MonoBehaviour
{
    public Transform Player;

    [SerializeField]
    private float _height = 20.0f;

    [SerializeField]
    private bool _followplayer = false;

    private float _currentheight = 20.0f;

    void LateUpdate()
    {
        if (!_followplayer) return;

        var wantedPosition = Player.position + new Vector3(0, _currentheight, 0);
        transform.position = wantedPosition;
    }

    void Start()
    {
        _currentheight = _height;
    }

    void OnGUI()
    {
        _currentheight = GUILayout.HorizontalSlider(_currentheight, _height, _height * 5);
        GUILayout.Label("Camera height");
    }
}