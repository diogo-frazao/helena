using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmandoAnimationManager : MonoBehaviour
{
    public static ArmandoAnimationManager Instance { get; private set; }

    private GameObject myChair;
    private GameObject officeChair;
    private Animator myAnimator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        myAnimator = GetComponent<Animator>();

        // Armando only needs to have a child chair on the first act.
        if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.FirstAct)
        {
            myChair = transform.Find("Secretária_0").gameObject;
            officeChair = GameObject.FindGameObjectWithTag("Office Chair");
        }
    }

    public void DisableOfficeChair()
    {
        officeChair.SetActive(false);
    }

    public void EnableArmandoChair()
    {
        myChair.SetActive(true);
    }

    public void PlayAnimationTrigger(string trigger)
    {
        myAnimator.SetTrigger(trigger);
    }

    public void EnableOfficeFinalCutscene()
    {
        FindObjectOfType<OfficeFinalCutscene>().enabled = true;
    }
}
