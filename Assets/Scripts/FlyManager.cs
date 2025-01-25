using GumFly.ScriptableObjects;
using UnityEngine;

public class FlyManager : MonoBehaviour
{ 
    [SerializeField]
    private Level levelConfig;
    [SerializeField]
    private RectTransform _spawnArea;
    [SerializeField]
    private float _inwardOffsetX = 300.0f;
    [SerializeField]
    private float _inwardOffsetY = 200.0f;

    private int flyTotal = 0;

    private void Start()
    {
        for(int flyTypeIndex = 0; flyTypeIndex < levelConfig.flyTypesInLevel.Length; flyTypeIndex++)
        {
            FlyAmount flyType = levelConfig.flyTypesInLevel[flyTypeIndex];
            flyTotal += flyType.amount;

            for(int flyIndex = 0; flyIndex < flyType.amount; flyIndex++)
            {
                SpawnFlyRandomly(flyType.reference);
            }
        }
    }

    private void SpawnFlyRandomly(GameObject prefab)
    {
        Vector2 randomPos = getRandomPositionInBounds(_spawnArea.rect);
        GameObject obj = Instantiate(prefab, randomPos, Quaternion.identity);
        obj.transform.SetParent(this.transform);
    }

    private Vector2 getRandomPositionInBounds(Rect bound)
    {
        Vector2 result = Vector2.zero;
        result.x = Random.Range(bound.xMin + _inwardOffsetX, bound.xMax - _inwardOffsetX);
        result.y = Random.Range(bound.yMin + _inwardOffsetY, bound.yMax - _inwardOffsetY);
        result += bound.position;
        return result;
    }
}
