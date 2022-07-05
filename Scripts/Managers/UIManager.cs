using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private Text collectiblesText;

    [SerializeField]
    private Image handbagImage;

    private CollectibleManager collectibleController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        collectibleController = FindObjectOfType<CollectibleManager>();
    }

    public void EnableCollectiblesUI(bool activateImage)
    {
        UpdateCollectiblesUI();

        if (activateImage)
        {
            collectiblesText.gameObject.SetActive(true);
            handbagImage.gameObject.SetActive(true);
        }
        else
        {
            collectiblesText.gameObject.SetActive(true);
        }
    }

    public void DisableCollectiblesUI()
    {
        collectiblesText.gameObject.SetActive(false);
        handbagImage.gameObject.SetActive(false);
    }

    public void EnableAndStayEnabledCollectiblesUI()
    {
        UpdateCollectiblesUI();
        collectiblesText.gameObject.SetActive(true);
        handbagImage.gameObject.SetActive(true);

        collectiblesText.GetComponent<Animator>().SetBool("canFadeOut", false);
        handbagImage.GetComponent<Animator>().SetBool("canFadeOut", false);
    }

    public void UpdateCollectiblesUI()
    {
        collectiblesText.text = collectibleController.GetNumberOfCollectibles().ToString() +
            "/" + collectibleController.GetMaxNumberOfCollectibles().ToString();
    }
}
