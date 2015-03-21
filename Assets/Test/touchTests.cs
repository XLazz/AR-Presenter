/**********************************************************************
 * The MIT License (MIT)
 *
 * Copyright (c) 2014, XLazz, Inc.
 * Contacts: http://xlazz.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 **********************************************************************/

using UnityEngine;
using System.Collections;

public class touchTests : MonoBehaviour {

	bool _isBegun; Vector2 _begunPoint;
	bool _isEnded; Vector2 _endPoint;
	bool _isMoved; Vector2 _movedPoint;



	// Use this for initialization
	void Start () {
		var touch = Camera.main.GetComponent<MouseSimpleController>();
		touch.TouchBegan += HandleTouchBegan;
		touch.TouchMoved += HandleTouchMoved;
		touch.TouchEnded += HandleTouchEnded;
	}

	void HandleTouchEnded (Vector2 p)
	{
		_isEnded = true;
		_endPoint = p;
	}

	void HandleTouchMoved (Vector2 cur, Vector2 prev)
	{
		_isMoved = true;
		_movedPoint = cur;
	}

	void HandleTouchBegan (Vector2 p)
	{
		_isBegun = true;
		_begunPoint = p;
	}
	
	// Update is called once per frame
	string status;
	void Update () {

		//status = "";
		if (_isBegun || _isMoved || _isEnded) {
			status = "";
		}

		if (_isBegun) {
			status = "B\t" + _begunPoint + "\t";
			_isBegun = false;
		}
		if (_isEnded) {
			status += "E\t" + _endPoint + "\t";
			_isEnded = false;
		}
		if (_isMoved) {
			status += " M - " + _movedPoint;
			_isMoved = false;
		}
	}

	void OnGUI()
	{
		GUI.Label(new Rect(0,0, Screen.width, Screen.height), status);
	}
}
