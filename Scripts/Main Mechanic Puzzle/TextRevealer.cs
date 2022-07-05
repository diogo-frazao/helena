using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextRevealer : MonoBehaviour
{
	[SerializeField]
	private float timeBetweenLetters = 0.3f;

	[SerializeField]
	private DistractionPuzzle distraction;

	// Cached References.
	private Text textToShow;

	private void Awake()
    {
		textToShow = GetComponent<Text>();
    }

	private void Start()
    {
        // We only want them to show at the end of the word.
        ShowPuzzlePieces(false);

        StartCoroutine(RevealText());

        ShowChildPieces();
    }

    private void ShowChildPieces()
    {
        if (transform.childCount > 0)
        {
            StartCoroutine(ShowPiecesOneByOne());
        }
    }

    private IEnumerator ShowPiecesOneByOne()
    {
		foreach (Transform child in transform)
        {
			yield return new WaitForSeconds(0.5f);
			child.gameObject.SetActive(true);
        }
    }

    private IEnumerator RevealText()
	{
		string originalString = textToShow.text;
		textToShow.text = "";

		int numCharsRevealed = 0;

		while (numCharsRevealed < originalString.Length)
		{
			// Skips spaces.
			if (originalString[numCharsRevealed] == ' ')
				++numCharsRevealed;

			++numCharsRevealed;
			textToShow.text = originalString.Substring(0, numCharsRevealed);

			if (numCharsRevealed == originalString.Length)
            {
				ShowPuzzlePieces(true);
			}

			yield return new WaitForSeconds(timeBetweenLetters);
		}

		if (gameObject.CompareTag(GameManager.Instance.GetNormalWordTag()))
        {
			distraction.GoToNext();
		}
		// Else is called by Puzzle Piece script when it reaches desired position.
	}

	private void ShowPuzzlePieces(bool value)
	{
		if (GameManager.Instance.IsPuzzlePiece(gameObject.transform))
		{
			foreach (Transform child in transform)
				child.gameObject.SetActive(value);
		}
	}

	// Called by Puzzle Piece when it reaches the desired position of the piece.
	public DistractionPuzzle GetDistraction()
    {
		return distraction;
    }

	// Called by puzzle piece in order to fade away every piece.
	public PuzzlePiece[] GetPiecesInChildren()
    {
		PuzzlePiece[] piecesChildren = GetComponentsInChildren<PuzzlePiece>();
		return piecesChildren;
    }
}