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

public class MouseSimpleController : MonoBehaviour {

	public delegate void MouseEventHandler(Vector2 p);
	public delegate void MouseEventHandler2(Vector2 cur, Vector2 prev);

	public event MouseEventHandler TouchBegan;
	public event MouseEventHandler2 TouchMoved;
	public event MouseEventHandler TouchEnded;

	private Vector2 _position;
	private float _time;
	private bool _showCursor;
	public Texture2D CursorIcon;
	
	void Start () {
		Screen.showCursor = false;	
		_showCursor = true;
	}

	void OnDestroy()
	{
		Screen.showCursor = true;	
	}

	void OnGUI()
	{
		GUI.depth = -1;
		if (_showCursor) {
			if (MySettings.IsStereo) {
				GUI.DrawTexture(new Rect(_position.x/2f, _position.y, 16f*ResizeHelper.k, 16f*ResizeHelper.k), CursorIcon);
				GUI.DrawTexture(new Rect(_position.x/2f + Screen.width/2f, _position.y, 16f*ResizeHelper.k, 16f*ResizeHelper.k), CursorIcon);
				
			} else {
				GUI.DrawTexture(new Rect(_position.x, _position.y, 16f*ResizeHelper.k, 16f*ResizeHelper.k), CursorIcon);
			}
		}
	}
	
	void Update () {

		_showCursor = (Time.time - _time < 3);

#if UNITY_EDITOR		
		MouseProc();
#else
		MouseProc();
		//TouchProc();
#endif	
	}
	
	void TouchProc()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			var p = new Vector2(touch.position.x, Screen.height - touch.position.y);
			switch (touch.phase)
			{
				case TouchPhase.Began:	_time = Time.time; if (TouchBegan != null) TouchBegan(p); break;
				case TouchPhase.Moved: _time = Time.time; if (TouchMoved != null) TouchMoved(p, _position); break;				
				case TouchPhase.Ended: _time = Time.time; if (TouchEnded != null) TouchEnded(p); break;
			}
			_position = p;
		}
	}
	protected bool IsMousePressed = false;
	void MouseProc()
	{
		var p = new Vector2( Input.mousePosition.x, Screen.height - Input.mousePosition.y);
	
		if (Input.GetMouseButtonDown(0)){
			IsMousePressed = true;
			_time = Time.time;
			if (TouchBegan != null) {
				TouchBegan(p);
			}
			_position = p;
		} else if (Input.GetMouseButtonUp(0)){
			IsMousePressed = false;
			_time = Time.time;
			if (TouchEnded != null) {
				TouchEnded(p);
			}
			_position = p;
		} else 
		{
			if (p != _position) {
				_time = Time.time;

				if (IsMousePressed) {
					if (TouchMoved != null) {
						TouchMoved(p, _position);
					}
				}
				_position = p;
			}

		}
	}
}
