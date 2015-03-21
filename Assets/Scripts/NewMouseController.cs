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

public class NewMouseController : MonoBehaviour {

	public delegate void EventHandler(Vector2 pos);
	public event EventHandler Click;
	public event EventHandler MouseDown;
	public event EventHandler MouseUp;
	public event EventHandler MouseMove;

	protected float _downTime;
	protected Vector3 _downPosition;
	protected bool _isDown;

	protected Vector3 _lastPosition;

	public Texture2D CursorIcon;
	protected bool _showCursor;
	protected float _time;

	// Use this for initialization
	void Start () {
		_time = Time.time;
		_showCursor = true;

		Screen.showCursor = false;
	}

	void OnDestroy()
	{
		Screen.showCursor = true;	
	}
	
	// Update is called once per frame
	void Update () {

		_showCursor = (Time.time - _time < 3);

		if (Input.GetMouseButtonDown(0))
		{
			_isDown = true;
			_downPosition = Input.mousePosition;
			_downTime = Time.time;
			_time = Time.time;
			if (MouseDown != null) {
				MouseDown(new Vector2( Input.mousePosition.x, Screen.height - Input.mousePosition.y) );
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			if ((Time.time - _downTime < 1f)
				&& ( (Input.mousePosition - _downPosition).magnitude < 5f*ResizeHelper.k)) {
				// click
				if (Click != null) {
					Click( new Vector2( Input.mousePosition.x, Screen.height - Input.mousePosition.y) );
				}
			}
			_isDown = false;
			_time = Time.time;

			if (MouseUp != null) {
				MouseUp(new Vector2( Input.mousePosition.x, Screen.height - Input.mousePosition.y) );
			}
		}
		if (_isDown)
		{
			if (MouseMove != null) {
				MouseMove( new Vector2( Input.mousePosition.x, Screen.height - Input.mousePosition.y) );
			}
		}

		if (_lastPosition != Input.mousePosition) {
			_lastPosition = Input.mousePosition;
			_time = Time.time;
		}

	}

	void OnGUI()
	{
		GUI.depth = -1;
		if (_showCursor) {
			var p = new Vector2( Input.mousePosition.x, Screen.height - Input.mousePosition.y);
			
			if (MySettings.IsStereo) {
				GUI.DrawTexture(new Rect(p.x/2f, p.y, 20f*ResizeHelper.k, 20f*ResizeHelper.k), CursorIcon);
				GUI.DrawTexture(new Rect(p.x/2f + Screen.width/2f, p.y, 20f*ResizeHelper.k, 20f*ResizeHelper.k), CursorIcon);
				
			} else {
				GUI.DrawTexture(new Rect(p.x, p.y, 20f*ResizeHelper.k, 20f*ResizeHelper.k), CursorIcon);
			}
		}
	}
}
