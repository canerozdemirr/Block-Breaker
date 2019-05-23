using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    [SerializeField] private float screenWidthInUnits = 16;
    [SerializeField] private float minX = 1f;
    [SerializeField] private float maxX = 15f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    float mousePosInUnit = Input.mousePosition.x / Screen.width * screenWidthInUnits;
	    Vector2 _paddlePos = new Vector2(mousePosInUnit, transform.position.y);
	    _paddlePos.x = Mathf.Clamp(mousePosInUnit, minX, maxX);
	    transform.position = _paddlePos;
	}
}
