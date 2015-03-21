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

public class ModelRotator : MonoBehaviour {

	private Vector2 _startPoint;
	private Vector2 _prev;
	private Vector3 _startAngle;
	private float k;
	
	private NewMouseController _touch;
	private bool _isRotating = false;

	private Rect _bounds;
	
	////////////////////////////////////////////////////////////////////////////////////////////////
	public void Start()
	{	
		k = ResizeHelper.k;
		#if UNITY_EDITOR
		/*_touch = Camera.main.GetComponent<NewMouseController>();
		_touch.TouchBegan += HandleTouchBegan;
		_touch.TouchMoved += HandleTouchMoved;
		_touch.TouchEnded += HandleTouchEnded;*/
		#endif
		var maxItemWidth = 250*k + 20f*k;

		_bounds = new Rect(maxItemWidth, 0, Screen.width - maxItemWidth, Screen.height);
	}
	

	////////////////////////////////////////////////////////////////////////////////////////////////
	void HandleTouchEnded(Vector2 p) {
	}
	////////////////////////////////////////////////////////////////////////////////////////////////
	void HandleTouchMoved(Vector2 p, Vector2 prev)
	{
		if (_isRotating && _bounds.Contains(p)) {
			RotateMask(p - prev);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////
	void HandleTouchBegan (Vector2 p)
	{
		p = new Vector2(p.x, Screen.height - p.y);
		if (_bounds.Contains(p)) {
			_startPoint = p;
			_isRotating = true;
		}
	}
		
	////////////////////////////////////////////////////////////////////////////////////////////////
	void OnDestroy()
	{
		#if UNITY_EDITOR
		if (_touch != null) {
			//_touch.TouchBegan -= HandleTouchBegan;
			//_touch.TouchMoved -= HandleTouchMoved;
			//_touch.TouchEnded -= HandleTouchEnded;
		}
		#endif
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////
	void ScaleMask(float factor)
	{
		var res = this.transform.localScale * factor;
		this.transform.localScale = res;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////
	void RotateMask(Vector2 p)
	{
		/*var d = p - _startPoint;
		var xAngle = d.x*k + _startAngle.y;
		if (xAngle < 0) xAngle += 360;
		else if (xAngle >= 360) xAngle -= 360;
		
		if (xAngle > 90 && xAngle < 180) xAngle = 90;
		else if (xAngle >= 180 && xAngle < 270) xAngle = 270;
		
		
		var yAngle = d.y*k + _startAngle.x;
		if (yAngle < 0) yAngle += 360;
		else if (yAngle >= 360) yAngle -= 360;
		
		if (yAngle > 30 && yAngle < 180) yAngle = 30;
		else if (yAngle >= 180 && yAngle < 330) yAngle = 330;*/

		//this.transform.localRotation = Quaternion.Euler(yAngle , xAngle, _startAngle.z);

		//this.transform.Rotate(-p.y*k/2f, -p.x*k/2f ,0);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////
	private enum TouchStates
	{
		none,
		move,
		zoom
	}
	private TouchStates _state = TouchStates.none;
	private float _distanceBetweenFingers; 
	private bool _miDistanceInited = false;
	
	public void Update()
	{
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.A)) {
			ScaleMask(0.9f);
		} else if (Input.GetKeyDown(KeyCode.Z)) {
			ScaleMask(1.1f);
		}
		#endif

		/*if (Input.touchCount == 2)
		{
			var t1 = Input.GetTouch(0);
			var p1 = new Vector2(t1.position.x, Screen.height - t1.position.y);					
			var t2 = Input.GetTouch(1);
			var p2 = new Vector2(t2.position.x, Screen.height - t2.position.y);					
			if (_bounds.Contains(p1) && _bounds.Contains(p2))					    
			{
				if (!_miDistanceInited)
				{
					_distanceBetweenFingers = Vector2.Distance(p1, p2);
					_miDistanceInited = true;
				} else 
				{
					var newDistanse = Vector2.Distance(p1, p2);
					ScaleMask(newDistanse/_distanceBetweenFingers);
					_distanceBetweenFingers = newDistanse;
				}
			}
		} else {
			_miDistanceInited = false;
		}*/

		if (_state == TouchStates.none) {
			if (Input.touchCount == 1) {
				var t = Input.GetTouch(0);
				var p = new Vector2(t.position.x, Screen.height - t.position.y);					
				if (t.phase == TouchPhase.Began && _bounds.Contains(p)) {
					_state = TouchStates.move;
					_startAngle = this.transform.localRotation.eulerAngles;
					_startPoint = p;
					_prev = p;
					_isRotating = true;
				} 
			} else if (Input.touchCount == 2) {
				var t1 = Input.GetTouch(0);
				var p1 = new Vector2(t1.position.x, Screen.height - t1.position.y);					
				var t2 = Input.GetTouch(1);
				var p2 = new Vector2(t2.position.x, Screen.height - t2.position.y);					
				if ((t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began))
				{
					_state = TouchStates.zoom;
					_distanceBetweenFingers = Vector2.Distance(p1,p2);
				}					
			}
		} 
		else if (_state == TouchStates.move)
		{
			if (Input.touchCount == 1) {
				var t = Input.GetTouch(0);
				var p = new Vector2(t.position.x, Screen.height - t.position.y);					
				if (t.phase == TouchPhase.Ended) {
					_state = TouchStates.none;
				} else if (t.phase == TouchPhase.Moved && _bounds.Contains(p)) {
					RotateMask(p - _prev);
					_prev = p;
				}
			} else if (Input.touchCount == 2) {
				var t1 = Input.GetTouch(0);
				var p1 = new Vector2(t1.position.x, Screen.height - t1.position.y);					
				var t2 = Input.GetTouch(1);
				var p2 = new Vector2(t2.position.x, Screen.height - t2.position.y);					
				if ((t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
				    && _bounds.Contains(p1)
				    && _bounds.Contains(p2)
				    ) {
					_state = TouchStates.zoom;
					_distanceBetweenFingers = Vector2.Distance(p1, p2);
				} 
			} else {
				_state = TouchStates.none;
			}
		} else if (_state == TouchStates.zoom) {
			if (Input.touchCount == 2) {
				var t1 = Input.GetTouch(0);
				var p1 = new Vector2(t1.position.x, Screen.height - t1.position.y);					
				var t2 = Input.GetTouch(1);
				var p2 = new Vector2(t2.position.x, Screen.height - t2.position.y);					
				if (_bounds.Contains(p1) && _bounds.Contains(p2))					    
				{
					var newDistanse = Vector2.Distance(p1, p2);
					ScaleMask(newDistanse/_distanceBetweenFingers);
					_distanceBetweenFingers = newDistanse;
				} 
			} else {
				_state = TouchStates.none;
			}
		}
	}
}
