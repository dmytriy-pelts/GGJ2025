using Unity.Collections;
using UnityEngine;

public class BubbleFlightPath : MonoBehaviour
{
    [SerializeField]
    private Vector2 _v0;
    [SerializeField]
    private float _gravityReversal = 0f;

    private const float _angle = 84.0f;
    private const float _gravity = 9.81f;
    private const float _totalTime = 10.0f;
    private const float _reversalForceY = 0f;
    private const float _gravityChangeConst = 6.936f;

    private Vector2 getPostition(float t)
    {
        float ajustedGravity = (_gravityReversal > 0.0f) ? _gravity + _gravityReversal * _gravityChangeConst : _gravity; 
        Vector2 _v0Final = _v0;
        float x = (_v0Final.x  * Mathf.Cos(_angle * Mathf.Deg2Rad)) * t;
        float y = (_v0Final.y * Mathf.Sin(_angle * Mathf.Deg2Rad)) * t - ((ajustedGravity - _gravityReversal * t) * t * t * _reversalForceY) / 2f;
        
        return new Vector2(x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        float tStep = 0.2f;
        int steps = (int)(_totalTime / tStep);
        for (int i = 0; i < steps; i++)
        {
            Vector2 pos = getPostition(i * tStep) + (Vector2)this.transform.position;
            Gizmos.DrawSphere(pos, 10f);
        }
    }
}
