using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GumFly.Behaviours
{
    public class LoadScenes : MonoBehaviour
    {
        [SerializeField]
        private string[] _scenes;

        private void Start()
        {
            for (int i = 0; i < _scenes.Length; i++)
            {
                string scene = _scenes[i];

                if (i == 0)
                {
                    SceneManager.LoadScene(scene, LoadSceneMode.Single);
                }
                else
                {
                    SceneManager.LoadScene(scene, LoadSceneMode.Additive);
                }
            }
        }
    }
}