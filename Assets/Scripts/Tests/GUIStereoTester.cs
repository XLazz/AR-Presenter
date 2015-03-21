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

public class GUIStereoTester : MonoBehaviour {

	public GameObject Cube;
	private RenderTexture _texture;
	protected Rect _rect;

	// Use this for initialization
	void Start () {
		_rect = new Rect(0,0,200,200);

	}

	void OnGUI()
	{
		/*RenderTexture.active = _texture;
		GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
		if (GUI.Button(_rect, "Hello so cruel world")) {
		}
		RenderTexture.active = null;*/

		var n = GUI.matrix;
		var t = n.m00;
		Debug.Log(t);

		n.m00 = 0.5f;
		GUI.matrix = n;
		
		GUI.Button(new Rect(Screen.width/2, 200, 200, 200), "!!!!");

		n.m00 = t;
		GUI.matrix = n;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
