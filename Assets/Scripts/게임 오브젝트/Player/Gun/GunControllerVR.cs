using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class GunControllerVR : MonoBehaviour
{
    private Gun gun;
    private ProceduralRecoil proceduralRecoil;
    
    private InputAction fireInputAction     => InputManager_InGame.Instance.inputActions[InputManager_InGame.InputType.Fire].action;
    private InputAction reloadInputAction   => InputManager_InGame.Instance.inputActions[InputManager_InGame.InputType.Reload].action;

    private void Awake()
    {
        // 총, 조준, 절차적 반동 컴포넌트를 가져옴
        gun = GetComponent<Gun>();
        proceduralRecoil = GetComponent<ProceduralRecoil>();

        // 총 발사 이벤트에 핸들러 추가
        gun.OnFired += OnGunFired;
    }

    private void Update()
    {
        // 발사 버튼이 눌렸는지 확인
        gun.SetTriggerPressed(fireInputAction.IsPressed());

        // 재장전 버튼이 눌렸거나 총알이 없으나 재장전 중이 아닐 경우 재장전
        if (reloadInputAction.WasPressedThisFrame() || (gun.BulletCount == 0 && !gun.isReloading))
        {
            gun.Reload();
        }

        // 총의 위치 및 회전 설정
        transform.localPosition = proceduralRecoil.Position;
        transform.localRotation = proceduralRecoil.Rotation;
    }

    private void OnGunFired()
    {
        // 총 발사 시 반동 적용
        proceduralRecoil.ApplyRecoil();
    }
}