using System;
using UnityEngine;

namespace GumFly.Behaviours
{
    [RequireComponent(typeof(Renderer))]
    public class AnimateUvs : MonoBehaviour
    {
        private MaterialPropertyBlock _mpb;

        private Vector4 _baseValue = new Vector4();

        [SerializeField]
        private Vector2 _speed;

        [SerializeField]
        private string _propertyName = "_MainTex_ST";

        private Renderer _renderer;

        private int _propertyID;

        private void Awake()
        {
            _mpb = new MaterialPropertyBlock();
            
            _propertyID = Shader.PropertyToID(_propertyName);
            _renderer = GetComponent<Renderer>();

            _baseValue = _renderer.sharedMaterial.GetVector(_propertyID);
        }

        private void Update()
        {
            _baseValue.z += _speed.x * Time.deltaTime;
            _baseValue.w += _speed.y * Time.deltaTime;
            
            _mpb.SetVector(_propertyID, _baseValue);
            
            _renderer.SetPropertyBlock(_mpb);
        }
    }
}