using Cysharp.Threading.Tasks;
using GumFly.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GumFly.UI.ChewChew
{
    public enum KeyType
    {
        L, R
    }

    public class RhythmEvent
    {
        public KeyType Key;
        public double Time;
    }

    public enum RhythmJudgement
    {
        Perfect,
        Good,
        Bad,
        Wrong
    }

    public class RhythmProcessor : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector _director;

        private List<RhythmEvent> _events = new List<RhythmEvent>();
        private List<KeyBehaviour> _behaviours = new List<KeyBehaviour>();

        [SerializeField]
        private KeyBehaviour _leftPrefab;

        [SerializeField]
        private KeyBehaviour _rightPrefab;

        [SerializeField]
        private float _perfectThreshold = 0.1f;

        [SerializeField]
        private float _goodThreshold = 0.2f;

        [SerializeField]
        private float _badThreshold = 0.5f;


        [SerializeField]
        private RectTransform _keyContainer;

        [SerializeField]
        private float _timeScale = 100.0f;

        [SerializeField]
        private bool _autostart = false;

        public List<RhythmEvent> Events => _events;
        
        public int EventCount { get; private set; }

        public UnityEvent<RhythmJudgement> JudgementMade { get; } = new UnityEvent<RhythmJudgement>();

        private void Start()
        {
            if (_autostart)
            {
                Initialize(_director.playableAsset as TimelineAsset);
                PlayAsync();
            }
        }

        private void Clear()
        {
            _keyContainer.DestroyChildren();
            _events.Clear();
            _behaviours.Clear();
        }

        public void Initialize(TimelineAsset asset)
        {
            _director.playableAsset = asset;
            _director.RebuildGraph();
            
            foreach (var marker in asset.markerTrack.GetMarkers().OfType<SignalEmitter>())
            {
                if (!marker.asset) continue;

                var keyType = marker.asset.name == "L" ? KeyType.L : KeyType.R;
                var ev = new RhythmEvent { Key = keyType, Time = marker.time, };
                _events.Add(ev);

                var prefab = keyType == KeyType.L ? _leftPrefab : _rightPrefab;
                var behaviour = Instantiate(prefab, _keyContainer);
                behaviour.Initialize(ev);
                behaviour.transform.localPosition = Vector3.right * (float)marker.time * _timeScale;
                _behaviours.Add(behaviour);
            }

            EventCount = _events.Count;
            _events.Sort((a, b) => a.Time.CompareTo(b.Time));
        }

        public async UniTask PlayAsync()
        {
            _director.Play();

            while (_director.state == PlayState.Playing)
            {
                await UniTask.Yield();
            }

            Clear();
        }

        private void Update()
        {
            if (_director.state != PlayState.Playing)
            {
                return;
            }

            _keyContainer.anchoredPosition =
                _keyContainer.anchoredPosition.WithX((float)(_director.time * -_timeScale));
            
            bool lPressed = Input.GetMouseButtonDown(0);
            bool rPressed = Input.GetMouseButtonDown(1);

            if (lPressed && rPressed)
            {
                return;
            }

            if (lPressed || rPressed)
            {
                var type = lPressed ? KeyType.L : KeyType.R;

                if (TryRemoveClosestEvent(_director.time, _badThreshold, out var e))
                {
                    var behaviour = _behaviours.Find(it => it.Event == e);
                    var result = Judge(_director.time, e, type);
                    behaviour.Consume(result);
                    
                    //_behaviours.Remove(behaviour);

                    JudgementMade.Invoke(result);
                }
                else
                {
                    Debug.Log($"No match at {_director.time}");
                }
            }
        }

        private RhythmJudgement Judge(double t, RhythmEvent rhythmEvent, KeyType pressed)
        {
            if (pressed != rhythmEvent.Key)
            {
                return RhythmJudgement.Wrong;
            }

            double diff = Math.Abs(rhythmEvent.Time - t);

            if (diff < _perfectThreshold)
            {
                return RhythmJudgement.Perfect;
            }

            if (diff < _goodThreshold)
            {
                return RhythmJudgement.Good;
            }

            return RhythmJudgement.Bad;
        }


        private bool TryRemoveClosestEvent(double t, double threshold, out RhythmEvent rhythmEvent)
        {
            rhythmEvent = null;

            double bestMatch = threshold;
            int bestMatchIndex = -1;

            double minTime = t - threshold;
            double maxTime = t + threshold;

            for (int i = 0; i < _events.Count; i++)
            {
                var e = _events[i];

                if (e.Time < minTime)
                {
                    continue;
                }

                if (e.Time > maxTime)
                {
                    // We won't find any matches
                    break;
                }

                double diff = Math.Abs(t - e.Time);

                // Within threshold
                if (diff < bestMatch)
                {
                    rhythmEvent = e;
                    bestMatch = diff;
                    bestMatchIndex = i;
                }
            }

            if (bestMatchIndex >= 0)
            {
                _events.RemoveAt(bestMatchIndex);
                return true;
            }

            return false;
        }
    }
}