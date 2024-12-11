using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFailedEvent : MonoBehaviour
{
    public Transform Origin;

    public float Radius;

    public Transform WorldUI;    
    public GameObject AlertPanel;
    public int AlertPanelCount = 10;

    public float MinAlertDelay = 0.1f;
    public float MaxAlertDelay = 0.5f;

    public void CreateAlertPanel()
    {
        GameObject GO = Instantiate(AlertPanel, Origin.position, Quaternion.identity);

        Vector3 offset = Random.insideUnitSphere * Radius;

        GO.transform.SetParent(WorldUI);
        GO.transform.position = Origin.position + offset;

        VRModeUI ui = GO.GetComponent<VRModeUI>();

        ui.Anchor = Origin;
        ui.Looker = Camera.main.transform;

        ui.WorldOffset = offset;

        ui.Show();
    }

    private IEnumerator CreateAlertPanelCoroutine()
    {
        for (int i = 0; i < AlertPanelCount; i++)
        {
            CreateAlertPanel();

            yield return new WaitForSeconds(Random.Range(MinAlertDelay, MaxAlertDelay));
        }
    }

    public void SetGameFailed()
    {
        StartCoroutine(CreateAlertPanelCoroutine());
    }
}

