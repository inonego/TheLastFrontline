using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HUDTracker : MonoBehaviour
{
    [Header("General")]
    public Camera Camera;

    [Header("Detection")]
    public int MaxDetectionCount = 3;
    public float DetectionRadiusInScreenSpace = 10f; 

    [Header("UI")]
    public Vector3 UIScale = Vector3.one;
    public GameObject BoundingBoxPrefab; 
    public RectTransform BoundingBoxParent;
    public float ForceDirectedMinDistance = 100f;
    public int ForceDirectedIterations = 10;
    public float LerpSpeed;
    
    private List<HUDTrackerUI> UIObjectList = new List<HUDTrackerUI>();

    private Dictionary<Enemy, HUDTrackerUI> enemyHUDTrackerUIList = new Dictionary<Enemy, HUDTrackerUI>();
    
    public struct ScreenSpaceInfo
    {
        public Vector2 ScreenPosition;
        public float ScreenSpaceDistance;
        public float ScreenSpaceDepth;
    }

    /// <summary>
    /// 컴포넌트 초기화 시 호출되는 메서드
    /// </summary>
    private void Start()
    {
        StartCoroutine(ProcessTrack());
    }
    
    /// <summary>
    /// 적 추적 정보를 저장하기 위한 구조체
    /// </summary>
    public struct TrackInfo
    {
        public Enemy Enemy;
        public Vector2 ScreenPosition;
        public float ScreenSpaceDistance;
        public float WorldSpaceDistance;
    }

    /// <summary>
    /// 적 오브젝트를 추적하고 바운딩 박스를 업데이트하는 코루틴
    /// </summary>
    private IEnumerator ProcessTrack()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();

        List<KeyValuePair<Enemy, float>> detectedEnemies = new List<KeyValuePair<Enemy, float>>();

        while (true)
        {
            void DetectEnemies(float detectionRadiusInScreenSpace)
            {
                // 감지된 적들 목록 초기화
                detectedEnemies.Clear();

                foreach (var enemy in EnemyManager.Instance.Enemies)
                {
                    // 화면에 보이면서 거리가 충분히 가까운 적들만 처리
                    if (enemy != null)
                    {
                        if (enemy.isVisible)
                        {
                            var screenSpaceInfo = GetScreenSpaceInfo(enemy);

                            if (screenSpaceInfo.ScreenSpaceDistance <= detectionRadiusInScreenSpace && screenSpaceInfo.ScreenSpaceDepth > 0f)
                            {
                                detectedEnemies.Add(new KeyValuePair<Enemy, float>(enemy, screenSpaceInfo.ScreenSpaceDistance));
                            }
                        }
                    }
                }

                // 거리순으로 정렬
                detectedEnemies.Sort((a, b) => a.Value.CompareTo(b.Value));

                // 최대 감지 수 제한
                int maxDetectionCount = Mathf.Min(MaxDetectionCount, detectedEnemies.Count);

                // 최대 감지 수 이상의 적들은 제거
                detectedEnemies.RemoveRange(maxDetectionCount, detectedEnemies.Count - maxDetectionCount);
            }
            
            void RemoveEnemiesNotInDetectedList()
            {
                // 제거할 적들 목록 초기화
                enemiesToRemove.Clear();
                
                foreach (var (enemy, UI) in enemyHUDTrackerUIList)
                {
                    // 적이 없거나 감지된 적들에 포함되지 않으면 제거
                    if (enemy == null || !detectedEnemies.Any(detectedEnemy => detectedEnemy.Key == enemy))
                    {
                        // 해당 UI를 비활성화
                        UI.gameObject.SetActive(false);

                        enemiesToRemove.Add(enemy);
                    }
                }

                // 제거할 적들을 Dictionary에서 제거
                foreach (var enemy in enemiesToRemove)
                {
                    enemyHUDTrackerUIList.Remove(enemy);
                }
            }

            void AddEnemiesInDetectedList()
            {
                foreach (var detectedEnemy in detectedEnemies)
                {
                    // 현재 추적 정보 목록에 해당 적이 없으면
                    if (!enemyHUDTrackerUIList.ContainsKey(detectedEnemy.Key))
                    {
                        // 사용하지 않은 UI 오브젝트를 찾거나 새로 생성
                        HUDTrackerUI UI = UIObjectList.FirstOrDefault(ui => !enemyHUDTrackerUIList.ContainsValue(ui));
                        
                        // 사용할 수 있는 UI 오브젝트가 없으면 새로 생성
                        if (UI == null)
                        {
                            UI = CreateUIObject();
                        }
                        
                        // 찾거나 생성한 UI 오브젝트를 활성화
                        UI.gameObject.SetActive(true);

                        // 적과 UI 오브젝트를 추적 정보 목록에 추가
                        enemyHUDTrackerUIList.Add(detectedEnemy.Key, UI);
                    }
                }
            }

            void UpdateTrackInfo()
            {
                foreach (var (enemy, UI) in enemyHUDTrackerUIList)
                {
                    // 적의 스크린 좌표와 스크린 공간 거리 계산
                    var screenSpaceInfo = GetScreenSpaceInfo(enemy);

                    // 적의 월드 공간 거리 계산
                    float worldSpaceDistance = Vector3.Distance(Camera.transform.position, enemy.transform.position);

                    TrackInfo info = new TrackInfo
                    {
                        Enemy = enemy,
                        ScreenPosition = screenSpaceInfo.ScreenPosition,
                        ScreenSpaceDistance = screenSpaceInfo.ScreenSpaceDistance,
                        WorldSpaceDistance = worldSpaceDistance
                    };

                    UI.SetTrackInfo(info);
                }
            }

            // 화면 상에서 일정 범위 내의 적을 감지
            DetectEnemies(DetectionRadiusInScreenSpace);

            // 현재 적 추적 목록에서 제거할 적들 찾아 제거
            RemoveEnemiesNotInDetectedList();

            // 현재 적 추적 목록에 추가할 적들 찾아 추가
            AddEnemiesInDetectedList();

            // 적 추적 정보를 업데이트
            UpdateTrackInfo();

            // HUDTrackerUI를 업데이트
            UpdateHUDTrackerUI();

            yield return null;
        }
    }

    /// <summary>
    /// HUDTrackerUI 오브젝트를 생성하는 메서드
    /// </summary>
    /// <returns></returns>
    private HUDTrackerUI CreateUIObject()
    {
        GameObject GO = Instantiate(BoundingBoxPrefab, BoundingBoxParent);

        GO.SetActive(false);
        
        HUDTrackerUI UI = GO.GetComponent<HUDTrackerUI>();

        UIObjectList.Add(UI);

        return UI;
    }

    /// <summary>
    /// 적 오브젝트의 스크린 좌표와 스크린 공간 거리를 계산하는 메서드
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    private ScreenSpaceInfo GetScreenSpaceInfo(Enemy enemy)
    {
        Vector3 screenCenter = Camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
        
        Vector3 enemyScreenPos = Camera.WorldToScreenPoint(enemy.transform.position);

        float screenSpaceDistance = Vector3.Distance(Vector3.Scale(enemyScreenPos, new Vector3(1f, 1f, 0f)), screenCenter);
        float screenSpaceDepth = enemyScreenPos.z;

        return new ScreenSpaceInfo
        {
            ScreenPosition = enemyScreenPos,
            ScreenSpaceDistance = screenSpaceDistance,
            ScreenSpaceDepth = screenSpaceDepth
        };
    }
    
    private List<Vector2> UIEndPoints = new List<Vector2>();

    /// <summary>
    /// 감지된 적 오브젝트들의 위치에 맞춰 바운딩 박스 UI를 업데이트하는 메서드
    /// </summary>
    private void UpdateHUDTrackerUI()
    {
        UIEndPoints.Clear();

        if (enemyHUDTrackerUIList.Count > 1)
        {   
            // 계산할 위치를 목록에 추가합니다.
            for (int i = 0; i < enemyHUDTrackerUIList.Count; i++)
            {
                HUDTrackerUI UI = enemyHUDTrackerUIList.ElementAt(i).Value;

                UIEndPoints.Add(UI.TrackInfo.Value.ScreenPosition + Vector2.up * ForceDirectedMinDistance);
            }

            // 적들의 바운딩 박스가 겹치지 않도록 위치를 계산하고 UI 업데이트
            ApplyForceDirected(UIEndPoints, ForceDirectedMinDistance, ForceDirectedIterations);

            for (int i = 0; i < enemyHUDTrackerUIList.Count; i++)
            {
                HUDTrackerUI UI = enemyHUDTrackerUIList.ElementAt(i).Value;

                UI.UpdateLine(UIEndPoints[i]);
            }
        }
        else if (enemyHUDTrackerUIList.Count == 1)
        {
            HUDTrackerUI UI = enemyHUDTrackerUIList.First().Value;

            UI.UpdateLine(UI.TrackInfo.Value.ScreenPosition + Vector2.up * ForceDirectedMinDistance);
        }

        for (int i = 0; i < enemyHUDTrackerUIList.Count; i++)
        {
            HUDTrackerUI UI = enemyHUDTrackerUIList.ElementAt(i).Value;

            UI.LerpSpeed = LerpSpeed;
            UI.transform.localScale = UIScale;
        }
    }

    private void ApplyForceDirected(List<Vector2> positions, float minDistance = 100f, int iterations = 10)
    {
        for (int iter = 0; iter < iterations; iter++)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                Vector2 force = Vector2.zero;
                
                for (int j = 0; j < positions.Count; j++)
                {
                    if (i == j) continue;
                    
                    Vector2 diff = positions[i] - positions[j];
                    float distance = diff.magnitude;
                    
                    if (distance < minDistance)
                    {
                        force += diff.normalized * (minDistance - distance) * 0.5f;
                    }
                }
                
                positions[i] += force;
            }
        }
    }
}
