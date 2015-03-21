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

using System;
using UnityEngine;

public class TouchEmulator : MonoBehaviour
{
	public Texture2D CursorIcon;
	public delegate void MouseEventHandler(Vector2 p);
	public delegate void MouseEventHandler2(Vector2 p, Vector2 prev);

	public event MouseEventHandler Click;
	public event MouseEventHandler StartDrag;
	public event MouseEventHandler2 Drag;
	public event MouseEventHandler EndDrag;

	private Vector2 _lastTouchPosition;
	private Vector2 _position;

	private float _time;
	private float _timeLastTouchUp;

	private float _timeInStationary;
	private bool _isStationary;

	private Vector2 _dragTestPosition;
	private bool _isTestDraging;
	private bool _isDraging;

	public void Start()
	{
		_position = new Vector2( Screen.width/2, Screen.height/2);
		_time = -3f; // to show cursor
		_timeLastTouchUp = -100f; // no click
		_isStationary = false;

		Screen.showCursor = false;	
	}

	void OnDestroy()
	{
		Screen.showCursor = true;	
	}
	
	void Update()
	{
		#if UNITY_EDITOR		
		MouseProc();
		#else
		TouchProc();
		#endif	
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void TouchDown(Vector2 p)
	{
		_time = Time.time;
		_lastTouchPosition = p;
		_isStationary = false;

		if (Time.time - _timeLastTouchUp < 0.4f)
		{
			_dragTestPosition = _position;
			_isTestDraging = true;
		}
	}

	void TouchUp(Vector2 p)
	{
		if (Time.time - _timeLastTouchUp < 0.4f)
		{
			// click
			if (Click != null) {
				Click(_position);
			}
		}

		if (_isDraging)
		{
			Debug.Log(" - EndDrag - " + Time.time);
			if (EndDrag != null) {
				EndDrag(_position);
			}
		}
		_isTestDraging = false;		 
		_isDraging = false;
		_timeLastTouchUp = Time.time;
	}

	void TouchMove(Vector2 p)
	{
		_time = Time.time;
		var l = _position;
		_position += (p - _lastTouchPosition);
		if (_position.x < 0) _position.x = 0;
		else if (_position.x > Screen.width) _position.x = Screen.width;
		if (_position.y < 0) _position.y = 0;
		else if (_position.y > Screen.height) _position.y = Screen.height;

		if (_isTestDraging) {

			if (_isDraging) {
				Debug.Log(" - Drag - " + Time.time);
				if (Drag != null) {
					Drag(_position, l);
					}
			} else {
				if ((_position - _dragTestPosition).magnitude*ResizeHelper.k > 5) {
					_isDraging = true;
					Debug.Log(" - StartDrag - " + Time.time);
					if (StartDrag != null) {
						StartDrag(_position);
					}
				}
			}
		}

		_lastTouchPosition = p;
		_isStationary = false;

	}

	void TouchStationary(Vector2 p)
	{
		if (_isDraging) return; // draging

		if (_isStationary) {
			if (Time.time - _timeInStationary > 1.5f)
			{
				Click(_position);
				_isStationary = false;
			}

		} else {
			_timeInStationary = Time.time;
			_isStationary = true;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void TouchProc()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			var p = new Vector2(touch.position.x, Screen.height - touch.position.y);
			switch (touch.phase)
			{
			case TouchPhase.Began:	TouchDown(p); break;
			case TouchPhase.Moved: TouchMove(p); break;				
			case TouchPhase.Stationary: TouchStationary(p); break;				
			case TouchPhase.Ended: TouchUp(p); break;
			}
			Debug.Log(" touch: " + touch.phase);
		}
	}

	protected bool IsMousePressed = false;
	protected Vector2 _lastMousePosition;
	void MouseProc()
	{
		var p = new Vector2(Input.mousePosition.x,Screen.height - Input.mousePosition.y);
		
		if (Input.GetMouseButtonDown(0)){
			IsMousePressed = true;
			TouchDown(p);
			_lastMousePosition = p;
		} else if (Input.GetMouseButtonUp(0)){
			IsMousePressed = false;
			TouchUp(p);
		} else 
		{
			if (IsMousePressed) {
				if (p != _lastMousePosition) {
					TouchMove(p);
					_lastMousePosition = p;
				} else {
					TouchStationary(p);
				}
			}
			
		}
	}

	void OnGUI()
	{
		GUI.depth = -1;
		if (Time.time - _time < 3) {
			if (MySettings.IsStereo) {
				GUI.DrawTexture(new Rect(_position.x/2f, _position.y, 16f*ResizeHelper.k, 16f*ResizeHelper.k), CursorIcon);
				GUI.DrawTexture(new Rect(_position.x/2f + Screen.width/2f, _position.y, 16f*ResizeHelper.k, 16f*ResizeHelper.k), CursorIcon);
				
			} else {
				GUI.DrawTexture(new Rect(_position.x, _position.y, 16f*ResizeHelper.k, 16f*ResizeHelper.k), CursorIcon);
			}
		}
	}

}
