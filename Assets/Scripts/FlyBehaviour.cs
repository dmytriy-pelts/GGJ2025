using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _flightRadius = 2.0f;
    [SerializeField]
    private float _maxFlightHeight = 0.3f;
    [SerializeField]
    private Vector2 _anchorPos = Vector2.zero;

    [SerializeField]
    private float _flightSpeed = 1.0f;

    private int _flightDirection;

    private bool _isAbleToEvade = false;
    private bool _isEvading = false;
    private Vector2 _evadingPos = Vector2.zero;
    private float _evadingSpeed = 4.0f;

    private float _evadionTotalCooldown = 1.0f;
    private float _evadionCooldown = 1.0f;

    void Start()
    {
        _flightDirection = (Random.Range(0, 1) < 0.5f) ? -1 : 1;
        Vector2 initPos = _anchorPos;
        initPos.x = Random.Range(-_flightRadius, _flightRadius);
    }

    Vector2 velo2D = Vector2.one;
    void Update()
    {
        float time = Time.time;
        Vector2 oldPos = transform.position;
        Vector2 finalPos = Vector2.zero;

        if(_isEvading && _isAbleToEvade)
        {
            finalPos = Vector2.SmoothDamp(oldPos, _evadingPos, ref velo2D, 0.5f, _evadingSpeed);

            if(Vector2.Distance(oldPos, finalPos) < 0.001f)
            {
                _isEvading = false;
            }
        }
        else
        {
            if(Mathf.Abs(oldPos.x - _anchorPos.x) >= _flightRadius)
            {
                _flightDirection *= -1;
            }

            float heighAjustment = Mathf.Cos(time * 2.0f) * 2.0f;
            Vector2 target = new Vector2(_anchorPos.x + (_flightDirection * (_flightRadius + 0.2f)), _anchorPos.y + heighAjustment);
            finalPos = Vector2.SmoothDamp(oldPos, target, ref velo2D, 0.5f, _flightSpeed);
        }

        this.transform.position = finalPos;
        _evadionCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isAbleToEvade || _evadionCooldown > 0.0f) { return; }

        _isEvading = true;
        Vector2 bubblePos = other.transform.position;
        Vector2 diff = (Vector2)transform.position - bubblePos;
        _evadingPos = (Vector2)transform.position + diff;
        _flightDirection = 1;
        _evadionCooldown = _evadionTotalCooldown;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_isEvading)
        {
            Gizmos.DrawSphere(_evadingPos, 0.1f);
        }
    }
}
