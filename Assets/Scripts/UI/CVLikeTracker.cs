using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CVLikeTracker : MonoBehaviour
{
    [Header("General")]
    public float UpdateInterval = 0.0f;

    [Header("Detection")]
    public int MaxDetectionCount = 3;
    public float DetectionRadiusInScreenSpace = 10f;  

    [Header("UI")]
    public Vector3 UIScale = Vector3.one;
    public int InitialUIObjectCount = 3;
    public GameObject BoundingBoxPrefab; 
    public RectTransform BoundingBoxParent;

    public Camera MainCamera { get; private set; } = null;
    
    private Dictionary<Enemy, CVLikeTrackerUI> enemyTrackerList = new Dictionary<Enemy, CVLikeTrackerUI>();
    
    private List<CVLikeTrackerUI> UIObjectList = new List<CVLikeTrackerUI>();

    /// <summary>
    /// 컴포넌트 초기화 시 호출되는 메서드
    /// </summary>
    private void Start()
    {
        MainCamera = Camera.main;

        InitializeBoundingBoxes();

        StartCoroutine(ProcessTrack());
    }
    
    /// <summary>
    /// 적 추적 정보를 저장하기 위한 구조체
    /// </summary>
    public struct TrackInfo
    {
        public Enemy Enemy;
        public Vector3 ScreenPosition;
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
            #region 적 감지

            // 감지된 적들 목록 초기화
            detectedEnemies.Clear();

            foreach (var enemy in EnemyManager.Instance.Enemies)
            {
                // 화면에 보이면서 거리가 충분히 가까운 적들만 처리
                if (enemy != null && enemy.isVisible)
                {
                    var (enemyScreenPos, screenSpaceDistance) = GetScreenSpaceInfo(enemy);

                    if (screenSpaceDistance <= DetectionRadiusInScreenSpace)
                    {
                        detectedEnemies.Add(new KeyValuePair<Enemy, float>(enemy, screenSpaceDistance));
                    }
                }
            }

            // 거리순으로 정렬
            detectedEnemies.Sort((a, b) => a.Value.CompareTo(b.Value));

            // 최대 감지 수 제한
            int maxDetectionCount = Mathf.Min(MaxDetectionCount, detectedEnemies.Count);

            // 최대 감지 수 이상의 적들은 제거
            detectedEnemies.RemoveRange(maxDetectionCount, detectedEnemies.Count - maxDetectionCount);

            #endregion

            #region 적 추적 정보 현재 목록 업데이트
            
            // 제거할 적들 목록 초기화
            enemiesToRemove.Clear();
            
            foreach (var (enemy, UI) in enemyTrackerList)
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
                enemyTrackerList.Remove(enemy);
            }

            #endregion

            #region 적 추적 정보 초기화 및 추가

            foreach (var detectedEnemy in detectedEnemies)
            {
                if (!enemyTrackerList.ContainsKey(detectedEnemy.Key))
                {
                    CVLikeTrackerUI UI = UIObjectList.FirstOrDefault(ui => !enemyTrackerList.ContainsValue(ui));
                    
                    if (UI == null)
                    {
                        UI = CreateUIObject();
                    }

                    UI.Reset();
                    
                    UI.gameObject.SetActive(true);

                    enemyTrackerList.Add(detectedEnemy.Key, UI);
                }
            }

            #endregion

            UpdateBoundingBoxes();

            if (UpdateInterval <= 0f)
            {
                yield return null;
            }
            else
            {
                // 설정된 간격만큼 대기 후 다음 프레임에서 다시 처리
                yield return new WaitForSeconds(UpdateInterval);
            }
        }
    }

    private CVLikeTrackerUI CreateUIObject()
    {
        GameObject GO = Instantiate(BoundingBoxPrefab, BoundingBoxParent);

        GO.SetActive(false);
        
        CVLikeTrackerUI UI = GO.GetComponent<CVLikeTrackerUI>();

        UIObjectList.Add(UI);

        return UI;
    }

    /// <summary>
    /// 바운딩 박스 UI 오브젝트들을 초기화하는 메서드
    /// </summary>
    private void InitializeBoundingBoxes()
    {
        // 최대 감지 수만큼 바운딩 박스 UI 오브젝트를 미리 생성
        for (int i = 0; i < InitialUIObjectCount; i++)
        {
            CreateUIObject();
        }
    }

    private (Vector3 screenPos, float screenSpaceDistance) GetScreenSpaceInfo(Enemy enemy)
    {
        Vector3 screenCenter = MainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));

        Vector3 enemyScreenPos = Vector3.Scale(MainCamera.WorldToScreenPoint(enemy.transform.position), new Vector3(1f, 1f, 0f));

        float screenSpaceDistance = Vector3.Distance(enemyScreenPos, screenCenter);

        return (enemyScreenPos, screenSpaceDistance);
    }

    // 각 UI의 현재 각도와 할당될 각도를 저장할 Dictionary
    private Dictionary<CVLikeTrackerUI, float> currentAngles = new Dictionary<CVLikeTrackerUI, float>();
            
    private List<float> targetAngles = new List<float>();
            
    /// <summary>
    /// 감지된 적 오브젝트들의 위치에 맞춰 바운딩 박스 UI를 업데이트하는 메서드
    /// </summary>
    private void UpdateBoundingBoxes()
    {
        foreach (var (enemy, UI) in enemyTrackerList)
        {
            // 적의 스크린 좌표와 스크린 공간 거리 계산
            var (enemyScreenPos, screenSpaceDistance) = GetScreenSpaceInfo(enemy);

            // 적의 월드 공간 거리 계산
            float worldSpaceDistance = Vector3.Distance(MainCamera.transform.position, enemy.transform.position);

            TrackInfo info = new TrackInfo
            {
                Enemy = enemy,
                ScreenPosition = enemyScreenPos,
                ScreenSpaceDistance = screenSpaceDistance,
                WorldSpaceDistance = worldSpaceDistance
            };

            UI.transform.localScale = UIScale;

            UI.SetTrackInfo(info);
        }

        // 각 화면상 위치의 평균 계산
        Vector3 averageScreenPos = Vector3.zero;

        foreach (var (enemy, UI) in enemyTrackerList)
        {
            averageScreenPos += UI.trackInfo.Value.ScreenPosition;
        }

        if (enemyTrackerList.Count > 1)
        {
            averageScreenPos /= enemyTrackerList.Count;

            // 각도 간격 계산 (3개일 경우 120도)
            float angleStep = 360f / enemyTrackerList.Count;
            
            // 가능한 목표 각도들을 생성 (0, 120, 240도)
            for (int i = 0; i < enemyTrackerList.Count; i++)
            {
                targetAngles.Add(i * angleStep);
            }

            // 각 UI의 현재 각도 계산 (평균 위치 기준)
            foreach (var (enemy, UI) in enemyTrackerList)
            {
                Vector2 direction = (UI.trackInfo.Value.ScreenPosition - averageScreenPos).normalized;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                if (angle < 0) angle += 360f;

                currentAngles[UI] = angle;
            }
            
            // 각 UI에 가장 가까운 목표 각도 할당
            while (currentAngles.Count > 0)
            {
                float minDiff = float.MaxValue;

                CVLikeTrackerUI selectedUI = null;

                float selectedTargetAngle = 0f;
                
                foreach (var ui in currentAngles.Keys)
                {
                    foreach (float targetAngle in targetAngles)
                    {
                        float diff = Mathf.Abs(Mathf.DeltaAngle(currentAngles[ui], targetAngle));

                        if (diff < minDiff)
                        {
                            minDiff = diff;
                            selectedUI = ui;
                            selectedTargetAngle = targetAngle;
                        }
                    }
                }
                
                // 선택된 UI에 각도 할당하고 평균 위치 기준으로 방향 설정
                float radian = selectedTargetAngle * Mathf.Deg2Rad;
                
                Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

                selectedUI.UpdateLine(direction);
                
                currentAngles.Remove(selectedUI);
                targetAngles.Remove(selectedTargetAngle);
            }
        }
        else 
        //
        if (enemyTrackerList.Count == 1)
        {
            enemyTrackerList.First().Value.UpdateLine(Vector2.up);
        }
    }
}
