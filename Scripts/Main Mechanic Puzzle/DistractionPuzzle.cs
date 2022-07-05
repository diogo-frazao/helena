using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistractionPuzzle : MonoBehaviour
{
    [SerializeField]
    private GameObject[] sentencesParents;

    [Tooltip("If set to true, after completing the puzzle the camera will follow the player again")]
    [SerializeField]
    private bool canRestoreCameraFollow = true;

    [Tooltip("If set to true the envelope will update its content on collision enter.")]
    [SerializeField]
    private bool canChangeEnvelopeContent = true;

    [Tooltip("If set to true after all the puzzles, the notebook will disappear")]
    [SerializeField]
    private bool canDisappearNotebookAfter = true;

    [SerializeField]
    private bool canShowEnvelopeIconAfter = true;

    [SerializeField]
    private bool canPlayLetterSound = true;

    [SerializeField]
    private float timeToStartSentence = 7f;
    
    // Sentence Related.
    private GameObject currentSentence;
    private List<Transform> currentSentenceWords = new List<Transform>();

    // Cached References.
    private Animator noteBookImageAnimator;
    private MoveController player;
    private CameraZoom cameraZoom;

    // Instance variables.
    private int sentenceIndex = 0;
    private int wordIndex = 0;
    private bool hasAlareadyMadePuzzle = false;

    private void Start()
    {
        player = FindObjectOfType<MoveController>();
        cameraZoom = FindObjectOfType<CameraZoom>();

        noteBookImageAnimator = GameObject.FindGameObjectWithTag("Notebook").
                                GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasAlareadyMadePuzzle)
        {
            Invoke(nameof(StartFirstSentence), timeToStartSentence);
        }
    }

    private void StartFirstSentence()
    {
        // Shows notebook.
        noteBookImageAnimator.SetTrigger("appear");

        // Shows first word from the first sentence.
        AssignCurrentSentenceAndItsWords(sentenceIndex);
        currentSentenceWords[0].gameObject.SetActive(true);

        // Used for avoiding replaying the puzzle on trigger enter.
        hasAlareadyMadePuzzle = true;

        SoundEffectsManager.Instance.penWritingAudio.Play();
        GameManager.Instance.ActivatePuzzleBoundaries(true);
    }

    private void AssignCurrentSentenceAndItsWords(int index)
    {
        currentSentence = sentencesParents[index];

        for (int i = 0; i < currentSentence.transform.childCount; i++)
        {
            currentSentenceWords.Add(currentSentence.transform.GetChild(i));
        }
    }

    //Called by GoToNextWord script attached to each word.
    public void GoToNext()
    {
        if (wordIndex < currentSentenceWords.Count - 1)
        {
            GoToNextWord();
        }
        else    //Is the last word from the current sentence
        {
            StartCoroutine(GoTonextSentence());
        }
    }

    private void GoToNextWord()
    {
        wordIndex++;
        currentSentenceWords[wordIndex].gameObject.SetActive(true);
    }

    private IEnumerator GoTonextSentence()
    {
        FadeOutSentenceWords();

        yield return new WaitForSeconds(0.5f);

        // Only after fading the sentence out the sentence it continues.

        wordIndex = 0;
        currentSentenceWords.Clear();

        if (sentenceIndex < sentencesParents.Length - 1)
        {
            sentenceIndex++;
            AssignCurrentSentenceAndItsWords(sentenceIndex);
            currentSentenceWords[0].gameObject.SetActive(true);
        }
        else    // Cleared the last sentence.
        {
            if (canDisappearNotebookAfter)
            {
                SoundEffectsManager.Instance.penWritingAudio.Stop();
                noteBookImageAnimator.SetTrigger("disappear");
            }

            if (canRestoreCameraFollow)
            {
                Invoke(nameof(RestoreCameraAndPlayerMovement), 1f);
            }

            if (canChangeEnvelopeContent)
            {
                EnvelopeManager.Instance.GoToNextEnvelope();
            }

            if (canShowEnvelopeIconAfter)
            {
                EnvelopeManager.Instance.ShowEnvelopeIcon(true);
            }

            // Can pickup collectibles again.
            Collectible.SetCanPickCollectible(true);

            // Feedback to show the letter was modified.
            if (canPlayLetterSound)
            {
                SoundEffectsManager.Instance.letterOpenAudio.Play();
            }

            cameraZoom.ZoomOut(cameraZoom.DefaultZoom);
            GameManager.Instance.ActivatePuzzleBoundaries(false);
        }

    }

    private void RestoreCameraAndPlayerMovement()
    {
        GameManager.Instance.CameraFollowBehaviour();
        player.CanMove(true);
    }

    private void FadeOutSentenceWords()
    {
        foreach (var word in currentSentenceWords)
        {
            if (GameManager.Instance.IsPuzzlePiece(word))
            {
                FadeOutPuzzlePieces(word);
            }

            word.GetComponent<Animator>().SetTrigger("fadeOut");
        }
    }

    private void FadeOutPuzzlePieces(Transform word)
    {
        var puzzlePiece = word.GetComponentInChildren<PuzzlePiece>();

        if (puzzlePiece != null)
        {
            puzzlePiece.GetComponent<Animator>().SetBool("canBlink", false);
            puzzlePiece.GetComponent<Animator>().SetTrigger("fadeOut");
        }
    }
}
