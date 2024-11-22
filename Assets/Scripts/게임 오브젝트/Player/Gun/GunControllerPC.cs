using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class GunControllerPC : MonoBehaviour
{
    public new CinemachineVirtualCamera camera;

    private Gun gun;
    private Aim aim;
    private ProceduralRecoil proceduralRecoil;
    
    private InputAction zoomInputAction     => InputManager_InGame.Instance.inputActions[InputManager_InGame.InputType.Zoom].action;
    private InputAction fireInputAction     => InputManager_InGame.Instance.inputActions[InputManager_InGame.InputType.Fire].action;
    private InputAction reloadInputAction   => InputManager_InGame.Instance.inputActions[InputManager_InGame.InputType.Reload].action;

    private void Awake()
    {
        // 총, 조준, 절차적 반동 컴포넌트를 가져옴
        gun = GetComponent<Gun>();
        aim = GetComponent<Aim>();
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

        // 줌 가능 여부 확인
        bool CanZoom() => !gun.isReloading;

        // 줌이 가능한 상태에서 줌을 눌렀으면
        if (CanZoom() && zoomInputAction.IsPressed())
        {
            aim.SetView(aim.Views[1]); // 줌 뷰로 전환
            proceduralRecoil.lockPosition = true; // 반동 위치 잠금
        }
        else
        {
            aim.SetView(aim.Views[0]); // 기본 뷰로 전환
            proceduralRecoil.lockPosition = false; // 반동 위치 잠금 해제
        }

        if (aim.CurrentView != null)
        {
            // 줌에 따른 카메라의 시야각 설정
            camera.m_Lens.FieldOfView = aim.GetFieldOfView(camera);
        }

        // 총의 위치 및 회전 설정
        transform.localPosition = aim.Position + proceduralRecoil.Position;
        transform.localRotation = aim.Rotation * proceduralRecoil.Rotation;
    }

    private void OnGunFired()
    {
        // 총 발사 시 반동 적용
        proceduralRecoil.ApplyRecoil();
    }
}