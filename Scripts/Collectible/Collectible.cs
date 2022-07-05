using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private float collectibleVelocity = 3f;

    [SerializeField]
    private bool hasShadow = false;

    private Transform desiredTransform;
    private Animator myAnimator;
    private ParticleSystem idleVFX;

    private float accuracy = 0.5f;

    // Set to disable during puzzle and cutscenes.
    private static bool canPickCollectible = true;
    
    private enum State { Idle, Going}
    private State state;

    private void Awake()
    {
        desiredTransform = GameObject.FindGameObjectWithTag("Collectibles Desired").transform;
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        state = State.Idle;
        idleVFX = GetComponentInChildren<ParticleSystem>();

        StartCoroutine(FixColliderBug());
    }

    private IEnumerator FixColliderBug()
    {
        Vector2 colliderOffset = new Vector2(0.5f, 0.5f);
        GetComponent<BoxCollider2D>().size -= colliderOffset;
        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider2D>().size += colliderOffset;
    }

    private void Update()
    {
        if (state == State.Going)
        {
            if (Vector2.Distance(transform.position, desiredTransform.position) > accuracy)
            {
                Vector3 direction = desiredTransform.position - transform.position;
                transform.position += direction * 0.005f * collectibleVelocity;

                myAnimator.SetTrigger("fadeOut");
            }
        }
    }

    private void OnMouseDown()
    {
        if (canPickCollectible)
        {
            SoundEffectsManager.Instance.pikcupAudio.Play();
            idleVFX.Stop();
            state = State.Going;

            if (hasShadow)
            {
                transform.Find("Collectible Shadow").gameObject.SetActive(false);
            }
        }
    }

    private void OnMouseEnter()
    {
        if (canPickCollectible)
        {
            Cursor.SetCursor(CustomMouseManager.Instance.GetHoverCursor(),
                Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(CustomMouseManager.Instance.GetNormalCursor(),
            Vector2.zero, CursorMode.ForceSoftware);
    }

    // Called by animation event.
    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }

    // Called by animation event.
    public void EnableCollectiblesUI()
    {
        UIManager.Instance.EnableCollectiblesUI(true);
    }

    // Called by animation event.
    public void UpdateCollectiblesUI()
    {
        CollectibleManager.Instance.AddNewCollectible();
        UIManager.Instance.UpdateCollectiblesUI();
    }

    public void EnableTextCollectiblesUI()
    {
        UIManager.Instance.EnableCollectiblesUI(false);
    }

    public static void SetCanPickCollectible(bool value)
    {
        canPickCollectible = value;
    }
}
