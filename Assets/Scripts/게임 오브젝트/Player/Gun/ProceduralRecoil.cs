using UnityEngine;
    
public class ProceduralRecoil : MonoBehaviour
{
    public bool lockPosition = false; // 위치 잠금 여부
    public bool lockRotation = false; // 회전 잠금 여부

    public Vector3 Position { get; private set; } = Vector3.zero; // 현재 위치
    public Quaternion Rotation { get; private set; } = Quaternion.identity; // 현재 회전
    
    private Vector3 targetPosition = Vector3.zero; // 목표 위치
    private Quaternion targetRotation = Quaternion.identity; // 목표 회전

    private readonly Vector3 initialPosition = Vector3.zero; // 초기 위치
    private readonly Quaternion initialRotation = Quaternion.identity; // 초기 회전

    [SerializeField] private Vector3 recoil; // 반동 값
    [SerializeField] private float kickBackZ; // Z축 반동 강도

    public float snappiness, returnAmount; // 반동의 반응 속도 및 복귀 강도

    private void Update()
    {
        // 위치 잠금이 해제된 경우
        if (!lockPosition)
        {
            // 목표 위치를 초기 위치로 부드럽게 복귀
            targetPosition = Vector3.Lerp(targetPosition, initialPosition, Time.deltaTime * returnAmount);
            Position = Vector3.Slerp(Position, targetPosition, Time.deltaTime * snappiness);
        }
        else
        {
            // 위치가 잠금 상태일 경우 목표 위치를 초기 위치로 설정
            Position = targetPosition = Vector3.zero;
        }

        // 회전 잠금이 해제된 경우
        if (!lockRotation)
        {
            // 목표 회전을 초기 회전으로 부드럽게 복귀
            targetRotation = Quaternion.Slerp(targetRotation, initialRotation, Time.deltaTime * returnAmount);
            Rotation = Quaternion.Slerp(Rotation, targetRotation, Time.deltaTime * snappiness);
        }
        else
        {
            // 회전이 잠금 상태일 경우 목표 회전을 초기 회전으로 설정
            Rotation = targetRotation = Quaternion.identity;
        }
    }

    public void ApplyRecoil()
    {
        targetPosition -= new Vector3(0f, 0f, kickBackZ);
        targetRotation *= Quaternion.Euler(recoil.x, Random.Range(-recoil.y, +recoil.y), Random.Range(-recoil.z, +recoil.z));
    }
}
