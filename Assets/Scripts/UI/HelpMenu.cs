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

public class HelpMenu : MonoBehaviour {

	private Rect _captionRect;
	private Rect _backRect;
	private Rect _textRect;
	
	private MainMenu _menu;
	private Vector2 _scroll;
	private GUIStyle _style;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Use this for initialization
	void Start () {
		var k = ResizeHelper.k;
		_captionRect = new Rect(0, 20f*k, Screen.width, 80f*k);
		
		_backRect = new Rect(20f*k, Screen.height - 50f*k, Screen.width - 20*2*k, 50f*k);
		
		_textRect = new Rect(20f*k, 110f*k, Screen.width - 20*2*k, Screen.height - 110f*k - 50f*k);
		
		_menu = this.GetComponent<MainMenu>();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void OnGUI() 
	{		
		if (_style == null) {
			_style = new GUIStyle(ResizeHelper.AboutTextStyle);
			_style.alignment = TextAnchor.UpperLeft;
		}

		GUI.Label(_captionRect, Lang.Help_Caption , ResizeHelper.MenuCaptionStyle);

		GUILayout.BeginArea(_textRect);
		_scroll = GUILayout.BeginScrollView(_scroll);
		GUILayout.Label(Lang.Help_Text, _style);
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		
		if (GUI.Button(_backRect, Lang.Settings_Back, ResizeHelper.ButtonStyle)) {
			this.enabled = false;
			_menu.enabled = true;
		}
		
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			this.enabled = false;
			_menu.enabled = true;
		}
	}
}
