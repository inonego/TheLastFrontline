using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    public new Transform camera;

    private Vector3 currentPosition;
    private Vector3 currentRotation;

    private Vector3 targetPosition;
    private Vector3 targetRotation;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

    [SerializeField] private Vector3 recoil;

    [SerializeField] private float kickBackZ;

    public float snappiness, returnAmount;

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation.eulerAngles;

        currentPosition = targetPosition = transform.localPosition;
        currentRotation = targetRotation = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, initialRotation, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.deltaTime * snappiness);

        transform.localRotation = Quaternion.Euler(currentRotation);

        camera.localRotation = Quaternion.Euler(currentRotation);

        KickBack();
    }

    public void ApplyRecoil()
    {
        targetPosition -= new Vector3(0f, 0f, kickBackZ);
        targetRotation += new Vector3(recoil.x, Random.Range(-recoil.y, +recoil.y), Random.Range(-recoil.z, +recoil.z));
    }

    private void KickBack()
    {
        targetPosition = Vector3.Lerp(targetPosition, initialPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Slerp(currentPosition, targetPosition, Time.deltaTime * snappiness);

        transform.localPosition = currentPosition;
    }
}
