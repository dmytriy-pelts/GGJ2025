using GumFly.Extensions;
using System;
using UnityEngine;

namespace GumFly.Behaviours
{
    [RequireComponent(typeof(RectTransform))]
    public class FollowCursor : MonoBehaviour
    {
        private Rect _confinement;
        private void Awake()
        {
            _confinement = transform.parent.GetComponent<RectTransform>().rect;
        }

        private void Update()
        {
            var target = Input.mousePosition;
            var diff = target.XY() - transform.parent.position.XY();

            if (diff.magnitude > 20)
            {
                diff.Normalize();
                transform.localPosition = new Vector2(
                    diff.x * _confinement.width,
                    diff.y * _confinement.height
                ) * 0.5f;
            }
        }
    }
}