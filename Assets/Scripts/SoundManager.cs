using GumFly.Utils;
using UnityEngine;

namespace GumFly
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField]
        private AudioClip[] _swooshSounds;

        [SerializeField]
        private AudioClip[] _chewingSounds;

        [SerializeField]
        private AudioClip _gulpSound;

        [SerializeField]
        private AudioClip _spitSound;
        
        [SerializeField]
        private AudioClip[] _burstSounds;
        
        [SerializeField]
        private AudioClip[] _eekSounds;

        public void PlayGulp()
        {
            AudioSource.PlayClipAtPoint(_gulpSound, Camera.main.transform.position, 0.5f);
        }

        public void PlaySwoosh(int i, float volume = 1.0f)
        {
            var sound = _swooshSounds[i % _swooshSounds.Length];
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position, 0.5f * volume);
        }
        
        public void PlayChew()
        {
            if (_chewingSounds.Length == 0) return;
            
            var sound = _chewingSounds[Random.Range(0, _chewingSounds.Length)];
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position, 1.0f);
        }

        public void PlayEek()
        {
            if (_eekSounds.Length == 0) return;
            
            var sound = _eekSounds[Random.Range(0, _eekSounds.Length)];
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position, .3f);
        }

        public void PlaySplat(Vector3 position)
        {
            if (_burstSounds.Length == 0) return;
            
            var sound = _burstSounds[Random.Range(0, _burstSounds.Length)];
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position, .2f);
        }

        public void PlaySpit()
        {
            AudioSource.PlayClipAtPoint(_spitSound, FaceManager.Instance.transform.position, .2f);
        } 
        
    }
}