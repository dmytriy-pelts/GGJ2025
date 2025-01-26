using GumFly.ScriptableObjects;
using GumFly.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FlyManager : MonoSingleton<FlyManager>
{ 
    [SerializeField]
    public UnityAction AllFliesDead;
    [SerializeField]
    private Level levelConfig;
    [SerializeField]
    private RectTransform _spawnArea;
    [SerializeField]
    private float _inwardOffsetX = 300.0f;
    [SerializeField]
    private float _inwardOffsetY = 200.0f;

    private List<FlyBehaviour> _flyList = new List<FlyBehaviour>();

    public Level LevelConfig => levelConfig;

    public int RemainingFlyCount => _flyList.Count(fly => !fly.IsDead);
    public int DeadFlyCount => _flyList.Count(fly => fly.IsDead);

    private void Start()
    {
        for(int flyTypeIndex = 0; flyTypeIndex < levelConfig.flyTypesInLevel.Length; flyTypeIndex++)
        {
            FlyAmount flyType = levelConfig.flyTypesInLevel[flyTypeIndex];
            for(int flyIndex = 0; flyIndex < flyType.amount; flyIndex++)
            {
                _flyList.Add(SpawnFlyRandomly(flyType.reference));
            }
        }
    }

    private void Update()
    {
        bool isAllDead = true;
        foreach(FlyBehaviour flyBehaviour in _flyList)
        {
            if(!flyBehaviour.IsDead)
            {
                isAllDead = false;
                break;
            }
        }

        if(isAllDead)
        {
            AllFliesDead.Invoke();
        }
    }

    private FlyBehaviour SpawnFlyRandomly(GameObject prefab)
    {
        Vector2 randomPos = getRandomPositionInBounds(_spawnArea.rect);
        GameObject obj = Instantiate(prefab, randomPos, Quaternion.identity);
        obj.transform.SetParent(this.transform);

        return obj.GetComponent<FlyBehaviour>();
    }

    private Vector2 getRandomPositionInBounds(Rect bound)
    {
        Vector2 result = Vector2.zero;
        result.x = Random.Range(bound.xMin + _inwardOffsetX, bound.xMax - _inwardOffsetX);
        result.y = Random.Range(bound.yMin + _inwardOffsetY, bound.yMax - _inwardOffsetY);

        return _spawnArea.TransformPoint(result);
    }
}
