using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoveScript : MonoBehaviour 
{
	[Range(1f, 20f)]
	[SerializeField]
	private float scrollSpeed = 1f;

	[SerializeField]
	private float scrollOffset;

	private Vector2 startPosition;
	private float newPosition;

	private void Start() 
	{
		startPosition = transform.position;
	}
	
	private void Update() 
	{
		newPosition = Mathf.Repeat (Time.time * - scrollSpeed, scrollOffset);
		transform.position = startPosition + Vector2.right * newPosition;
	}
}
