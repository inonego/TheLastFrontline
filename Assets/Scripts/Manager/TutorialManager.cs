using System;
using System.Collections;
using System.Collections.Generic;
using inonego;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [Header("UI")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI TutorialText;
    private Animator UIPanelAnimator;
    public GameObject imagePanel;
    private Animator imagePanelAnimator;
    public Image imagePanelImage;
    public List<Sprite> images;

    [Header("Monster")] 
    public GameObject monsterPrefab;
    public Transform spawnPoint;
    
    [Header("Interactable")]
    public HandAnimator handAnimator;
    public InputActionReference NextAction;
    public GameObject grenade;
    private XRGrabInteractable grenadeGrab;
    public GameObject radio;
    private XRGrabInteractable radioGrab;
    
    private int index = 0;
    private bool isEnd = false;
    private bool waiting = false;
    protected override void Awake()
    {
        base.Awake();
        UIPanelAnimator = tutorialPanel.GetComponent<Animator>();
        imagePanelAnimator = imagePanel.GetComponent<Animator>();
        grenadeGrab = grenade.GetComponent<XRGrabInteractable>();
        radioGrab = radio.GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        UIPanelAnimator.SetBool("IsVisible", true);
        imagePanelAnimator.SetBool("IsVisible", true);
        StartCoroutine(Tutorial());
    }

    public void Update()
    {
        if (NextAction.action.WasPressedThisFrame())
        {
            ClickNextButton();
        }
    }

    public void ClickNextButton()
    {
        waiting = false;
    }

    private IEnumerator Tutorial()
    {
        while (!isEnd)
        {
            switch (index)
            {
                case 0:
                    yield return ShowText("Press button <A> on right controller to go next.");
                    imagePanelAnimator.SetBool("IsVisible", false);
                    yield return ShowText("Your mission is to protect the core behind you for 3 minutes.");
                    yield return ShowText("You can check the progress of the operation by lifting your left arm.");
                    yield return ShowText("Did you check the progress UI?");
                    yield return ShowText("Good. Let me tell you how to kill monsters.");
                    index++;
                    break;

                case 1:
                    changeImage(1);
                    yield return ShowText("To shoot, press the corresponding button on the left controller.");
                    imagePanelAnimator.SetBool("IsVisible", false);
                    yield return ShowText("I'll summon the monster, kill it.");
                    
                    GameObject monster1 = SpawnMonster();
                    yield return WaitForMonsterDeath(monster1);

                    yield return ShowText("Good. Let me tell you how to use grenade.");
                    index++;
                    break;

                case 2:
                    changeImage(2);
                    yield return ShowText("Put your right hand on the grenade and press <GRAB> button on the right controller.");
                    yield return ShowText("With the hand grenade in hand, press <REMOVE PIN> button.");
                    yield return ShowText("The pin falls out and three seconds later, the grenade explodes.");
                    yield return ShowText("Take your hand off <GRAB> button and swing the grenade before it bursts and it will be thrown.");
                    imagePanelAnimator.SetBool("IsVisible", false);
                    yield return ShowText("Now grab a grenade.");
                    yield return WaitForGrenadeGrab();
                    yield return ShowText("Good! Let's kill a monster with grenade.");

                    GameObject monster2 = SpawnMonster();
                    yield return WaitForMonsterDeath(monster2);

                    yield return ShowText("Good. Let me tell you how to use the radio.");
                    index++;
                    break;

                case 3:
                    changeImage(3);
                    yield return ShowText("Put your hand on it just like a grenade, hold it with the same button, and use it.");
                    imagePanelAnimator.SetBool("IsVisible", false);
                    yield return ShowText("Now grab a radio.");
                    yield return WaitForRadioGrab();
                    yield return ShowText("Good! Let's kill a monster with radio.");

                    GameObject monster3 = SpawnMonster();
                    yield return WaitForMonsterDeath(monster3);

                    yield return ShowText("Good. Let's go protect the world.");
                    isEnd = true;
                    break;
            }
        }
        
        // 메인 씬으로 전환 필요
        SceneManager.LoadScene("MainScene");
    }

    private GameObject SpawnMonster()
    {
        GameObject monster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);

        monster.GetComponent<Enemy>().Halt();
        monster.GetComponent<Rigidbody>().isKinematic = true;

        return monster;
    }

    private IEnumerator ShowText(string message)
    {
        waiting = true;
        TutorialText.text = message;
        yield return new WaitUntil(() => !waiting); 
    }

    private IEnumerator WaitForMonsterDeath(GameObject monster)
    {
        Health monsterHealth = monster.GetComponent<Health>();
        if (monsterHealth != null)
        {
            yield return new WaitUntil(() => monsterHealth.IsDead);
        }
    }

    private IEnumerator WaitForGrenadeGrab()
    {
        yield return new WaitUntil(() => grenadeGrab.isHovered || grenade == null);
    }
    
    private IEnumerator WaitForRadioGrab()
    {
        yield return new WaitUntil(() => radioGrab.isHovered);
    }

    private void changeImage(int index)
    {
        imagePanelImage.sprite = images[index];
        imagePanelAnimator.SetBool("IsVisible", true);
    }
}
