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

public class Fader : MonoBehaviour {

	public Texture2D Spinner;
	private Rect _rect;
	private Texture2D _bg;

	// Use this for initialization
	void Start () {
	
		_bg = ResizeHelper.Make(new Color(1,1,1, 0.5f));
		_rect= new Rect(0,0, Screen.width, Screen.height);
	}

	void OnGUI()
	{
		GUI.depth = -1;
		GUI.DrawTexture(_rect, _bg);

		Matrix4x4 oldMatrix = GUI.matrix;
		float thisAngle = Time.frameCount*4;
		
		Rect thisRect = new Rect(Screen.width/2.0f - Spinner.width/2f, Screen.height/2.0f - Spinner.height/2f,
		                         Spinner.width, Spinner.height);
		
		GUIUtility.RotateAroundPivot(thisAngle, thisRect.center);
		GUI.DrawTexture(thisRect, Spinner);
		GUI.matrix = oldMatrix;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
