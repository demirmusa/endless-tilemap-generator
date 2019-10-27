using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MapGenerator : MonoBehaviour
{
    public Transform Player;

    [SerializeField]
    private float _tileSize;

    [SerializeField]
    private float _roadWidth = 10f;

    [Header("Pool Information")]
    [SerializeField]
    private Vector3 _poolObjectPosition;

    [SerializeField]
    private int _poolLength = 4;

    [Header("Tile Map Parts")]
    [SerializeField]
    private TileMapPart _homeMapPart;

    [SerializeField]
    private TileMapPart[] _mapParts;


    private Queue<TileMapPart> _tileQueue;

    List<TileMapPart> _activeMapParts = new List<TileMapPart>();

    [Header("Test")]
    public Text TestText;

    private float _safeAreaMin;
    private float _safeAreaMax;

    private int _homeMapId;
    void Start()
    {
        InitializeMapPartQueue();
        InstantiateHomeMapPart();

        var roadPercent = _roadWidth / _tileSize;
        _safeAreaMin = .5f - roadPercent;
        _safeAreaMax = .5f + roadPercent;
    }

    void Update()
    {
        UpdateMap();
    }

    private void InitializeMapPartQueue()
    {
        _tileQueue = new Queue<TileMapPart>(_mapParts.Length);
        _mapParts = _mapParts.OrderBy(x => Guid.NewGuid()).ToArray(); //suffle 

        var multiplexer = _poolLength / _mapParts.Length;
        foreach (var mapPart in _mapParts)
        {
            for (int i = 0; i < multiplexer; i++)
            {
                var mapPartObj = Instantiate(mapPart, _poolObjectPosition, Quaternion.identity);

                _tileQueue.Enqueue(mapPartObj);
            }
        }

        //Make sure the pool is filled with elements in the number of pool widths.
        var missingItemCount = _poolLength - (multiplexer * _mapParts.Length);
        if (missingItemCount > 0)
        {
            for (int i = 0; i < missingItemCount; i++)
            {
                var randomMapPart = _mapParts[UnityEngine.Random.Range(0, _mapParts.Length - 1)];

                var mapPartObj = Instantiate(randomMapPart, _poolObjectPosition, Quaternion.identity);

                _tileQueue.Enqueue(mapPartObj);
            }
        }
    }

    private void InstantiateHomeMapPart()
    {
        var homeMapPart = Instantiate(_homeMapPart, Vector3.zero, Quaternion.identity);
        homeMapPart.gameObject.SetActive(true);
        homeMapPart.CurrentPosition = new Int2(0, 0);
        _homeMapId = homeMapPart.GetInstanceID();
        _activeMapParts.Add(homeMapPart);
    }

    private void UpdateMap()
    {
        var nextPosition = CalculateNextPositions();
        if (_activeMapParts.Any(x => x.CurrentPosition == nextPosition))
        {
            return;
        }

        var mapPart = _tileQueue.Dequeue();

        mapPart.transform.position = new Vector3(nextPosition.X * _tileSize, 0, nextPosition.Y * _tileSize);
        var rotation = mapPart.transform.rotation;
        rotation.eulerAngles = HandleRotation(nextPosition, mapPart.transform.rotation.eulerAngles, mapPart);
        mapPart.transform.rotation = rotation;

        mapPart.CurrentPosition = nextPosition;
        mapPart.gameObject.SetActive(true);

        _activeMapParts.Add(mapPart);

        DeletePreviousMapParts();
    }

    private Vector3 HandleRotation(Int2 nextPosition, Vector3 currentRotation, TileMapPart mapPart)
    {
        var currentPosition = CalculateCurrentPosition();
        if (nextPosition.X > currentPosition.X) //going to right
        {
            currentRotation.y = SideManager.GetRotation(SideManager.Right, mapPart);
        }
        else if (nextPosition.X < currentPosition.X)//going to left
        {
            currentRotation.y = SideManager.GetRotation(SideManager.Left, mapPart);
        }
        else if (nextPosition.Y > currentPosition.Y)//going to top
        {
            currentRotation.y = SideManager.GetRotation(SideManager.Top, mapPart);
        }
        else if (nextPosition.Y < currentPosition.Y)//going to down
        {
            currentRotation.y = SideManager.GetRotation(SideManager.Down, mapPart);
        }
        return currentRotation;
    }

    private void DeletePreviousMapParts()
    {
        //last 3 map parts can be active at the same time
        if (_activeMapParts.Count <= 3)
        {
            return;
        }

        var farthestMapPart = _activeMapParts.OrderByDescending(x => Vector3.Distance(x.transform.position, Player.position)).First();

        farthestMapPart.gameObject.SetActive(false);
        farthestMapPart.CurrentPosition = null;

        if (farthestMapPart.GetInstanceID() != _homeMapId)
        {
            _tileQueue.Enqueue(farthestMapPart);
        }

        _activeMapParts.Remove(farthestMapPart);

        DeletePreviousMapParts();
    }

    Int2 CalculateNextPositions()
    {
        var x = (Player.position.x + (_tileSize / 2)) / _tileSize;
        var y = (Player.position.z + (_tileSize / 2)) / _tileSize;

        var currentPosition = CalculateCurrentPosition();

        Int2 nextPosition = new Int2
        {
            X = GetFixedCalculatedNextPosition(currentPosition.X, x),
            Y = GetFixedCalculatedNextPosition(currentPosition.Y, y)
        };

        if (TestText)
        {
            TestText.text = currentPosition + " - " + nextPosition;
        }
        return nextPosition;
    }

    Int2 CalculateCurrentPosition()
    {
        Int2 position = new Int2
        {
            X = Mathf.FloorToInt((Player.position.x + (_tileSize / 2)) / _tileSize),
            Y = Mathf.FloorToInt((Player.position.z + (_tileSize / 2)) / _tileSize)
        };
        return position;
    }

    int GetFixedCalculatedNextPosition(int currentValue, float val)
    {
        bool negative = val < 0;

        var decimalPartOfAbsVal = Mathf.Abs(val) - (int)Mathf.Abs(val);

        int retVal = currentValue; //return current value if user in safe area

        //if user not in safe area
        if (decimalPartOfAbsVal > _safeAreaMax)
        {
            //make its absolute value bigger
            retVal = negative
                ? currentValue - 1
                : currentValue + 1;
        }
        else if (decimalPartOfAbsVal < _safeAreaMin)
        {
            //make its absolute value smaller
            retVal = negative
                ? currentValue + 1
                : currentValue - 1;
        }
        return retVal;
    }
}