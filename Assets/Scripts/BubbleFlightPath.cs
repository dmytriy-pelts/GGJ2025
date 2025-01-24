using UnityEngine;

public class BubbleFlightPath : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _curvature;

    [SerializeField]
    private float _angle;

    [SerializeField]
    private Vector2 _v0;

    [SerializeField]
    private float _gravity = 9.81f;

    [SerializeField]
    private float _gravityReversal = 0f;

    [SerializeField]
    private float _initForceX = 20f;
    [SerializeField]
    private float _reversalForceY = 0f;

    private Vector2 getPostition(float t)
    {
        // TODO(dmytriy):   - Increase total time based on _gravityReversal scale
        //                  - Calculate _initForceX and _reversalForceY based on _gravityReversal

        Vector2 _v0Final = _v0;
        _v0Final.x -= _gravityReversal * _initForceX;
        float x = (_v0Final.x  * Mathf.Cos(_angle * Mathf.Deg2Rad)) * t;
        float y = (_v0Final.y * Mathf.Sin(_angle * Mathf.Deg2Rad)) * t - ((_gravity - _gravityReversal * t) * t * t * _reversalForceY) / 2f;
        return new Vector2(x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        float tStep = 0.2f;
        float tTotal = 6.0f;
        int steps = (int)(tTotal / tStep);
        for (int i = 0; i < steps; i++)
        {
            Vector2 pos = getPostition(i * tStep);
            Gizmos.DrawSphere(pos, 0.1f);
        }
    }
}
