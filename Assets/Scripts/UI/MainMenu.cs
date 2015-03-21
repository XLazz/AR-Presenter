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

public class MainMenu : MonoBehaviour {

	public Texture2D Epson;
	private Rect _epsonLogoRect;
	public Texture2D Logo;
	private Rect _logoRect;

	private Rect _captionRect;
	private Rect _hintRect;

	private Rect[] _menuRect;

	private SettingsMenu _settings;
	private AboutMenu _about;
	private HelpMenu _help;
	private Fader _fader;


	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Use this for initialization
	void Start () {

		var k = ResizeHelper.k;

		_settings = this.GetComponent<SettingsMenu>();
		_settings.enabled = false;

		_about = this.GetComponent<AboutMenu>();
		_about.enabled = false;

		_help = this.GetComponent<HelpMenu>();
		_help.enabled = false;

		_fader = this.GetComponent<Fader>();

		var lw = 120f*k;
		_epsonLogoRect = new Rect(0,0, lw, lw*Epson.height/Epson.width);
		_logoRect = new Rect(Screen.width - lw,0, lw, lw*Logo.height/Logo.width);

		_captionRect = new Rect(0, 20f*k, Screen.width, 80f*k);

		_menuRect = new Rect[5];
		for (int i = 0; i < _menuRect.Length; i++) {
			_menuRect[i] = new Rect(0, 140f*k + 55f*k*i, Screen.width, 55f*k);
		}		

		_hintRect = new Rect(0, Screen.height - 30f*k, Screen.width, 30f*k);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void OnGUI()
	{
		GUI.DrawTexture(_epsonLogoRect, Epson);
		GUI.DrawTexture(_logoRect, Logo);

		GUI.Label(_captionRect, "AR-APPLICATION", ResizeHelper.MenuCaptionStyle);

		if (GUI.Button(_menuRect[0], Lang.MainMenu_Start, ResizeHelper.MenuItemStyle)) {
			_fader.enabled = true;
			Application.LoadLevelAsync("EpsonARScene");
		}

		if (GUI.Button(_menuRect[1], Lang.MainMenu_Settings, ResizeHelper.MenuItemStyle)) {
			this.enabled = false;
			_settings.enabled = true;
		}

		if (GUI.Button(_menuRect[2], Lang.MainMenu_Help, ResizeHelper.MenuItemStyle)) {
			this.enabled = false;
			_help.enabled = true;
		}

		if (GUI.Button(_menuRect[3], Lang.MainMenu_Credits, ResizeHelper.MenuItemStyle)) {
			this.enabled = false;
			_about.enabled = true;
		}

		if (GUI.Button(_menuRect[4], Lang.MainMenu_Exit, ResizeHelper.MenuItemStyle)) {
			Application.Quit();
		}

		GUI.Label(_hintRect, Lang.MainMenu_Hint, ResizeHelper.MenuHintStyle);

	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	
	}
}
