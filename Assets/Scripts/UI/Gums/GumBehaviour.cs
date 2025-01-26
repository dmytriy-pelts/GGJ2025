using Cysharp.Threading.Tasks;
using GumFly.Domain;
using GumFly.ScriptableObjects;
using LitMotion;
using LitMotion.Extensions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace GumFly.UI.Gums
{
    public class GumBehaviour : MonoBehaviour, IInitializable<Gum>
    {
        [SerializeField]
        private AudioClip _gulpSound;
        
        public async void Consume()
        {
            SoundManager.Instance.PlayGulp();
            await LMotion.Punch.Create(0.0f, 10.0f, 0.2f)
                .BindToLocalEulerAnglesZ(transform);
            
            Destroy(gameObject);
        }

        public void Initialize(Gum instance)
        {
            GetComponentInChildren<Graphic>().color = instance.Color;
        }
    }
}