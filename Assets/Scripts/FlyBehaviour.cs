using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _flightRadius = 2.0f;
    [SerializeField]
    private Vector2 _anchorPos = Vector2.zero;

    [SerializeField]
    private float _flightSpeed = 1.0f;

    private int _flightDirection;

    void Start()
    {
        _flightDirection = (Random.Range(0, 1) < 0.5f) ? -1 : 1;
        Vector2 initPos = _anchorPos;
        initPos.x = Random.Range(-_flightRadius, _flightRadius);
    }

    float velo = 0f;
    void Update()
    {
        float time = Time.time;
        Vector2 oldPos = transform.position;

        if(Mathf.Abs(oldPos.x - _anchorPos.x) >= _flightRadius)
        {
            _flightDirection *= -1;
        }
        float x = Mathf.SmoothDamp(oldPos.x, _anchorPos.x + (_flightDirection * (_flightRadius + 0.2f)), ref velo, 0.5f, 1f);
        float y = Mathf.Cos(time);
        Vector2 newPos = new Vector2(x, y);
        this.transform.position = newPos;
    }
}
