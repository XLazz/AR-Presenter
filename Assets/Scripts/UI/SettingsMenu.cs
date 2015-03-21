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

public class SettingsMenu : MonoBehaviour {

	private Rect _captionRect;

	private Rect[] _menuRect;
	private Rect[] _itemsRect;

	private Rect _backRect;
		
	private bool _isRussia;
	private bool _isStereo;
	private bool _isDebug;
	private bool _startFromMenu;
	private string _configName;

	private GUIStyle _styleLabel;
	private GUIStyle _styleValue;
	private MainMenu _menu;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Use this for initialization
	void Start () {
		var k = ResizeHelper.k;
		_captionRect = new Rect(0, 20f*k, Screen.width, 80f*k);

		_menuRect = new Rect[5];
		_itemsRect = new Rect[5];
		for (int i = 0; i < _menuRect.Length; i++) {
			_menuRect[i] = new Rect(0, 140f*k + 55f*k*i, Screen.width/2, 55f*k);
			_itemsRect[i] = new Rect(Screen.width/2f, 140f*k + 55f*k*i, Screen.width/2, 55f*k);
		}

		_backRect = new Rect(20f*k, Screen.height - 50f*k, Screen.width - 20*2*k, 50f*k);

		_isRussia = Lang.LangType.Russia == Lang.Type;
		_configName = MySettings.ConfigFile;
		_isDebug = Configuration.ConfigMaganer.Instance.Application.Debug;
		_isStereo = MySettings.IsStereo;
		_startFromMenu = MySettings.IsStartWithMenu;

		_menu = this.GetComponent<MainMenu>();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void InitStyles()
	{
		_styleValue = new GUIStyle(ResizeHelper.MenuItemStyle);
		_styleValue.alignment = TextAnchor.MiddleLeft;

		_styleLabel = new GUIStyle(_styleValue);
		_styleLabel.onHover = _styleLabel.onActive = _styleLabel.onNormal;

	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void Close()
	{
		var chages = (MySettings.ConfigFile != _configName) || (MySettings.IsStartWithMenu != _startFromMenu) || (MySettings.IsStereo != _isStereo) || (Configuration.ConfigMaganer.Instance.Application.Debug != _isDebug);
		if (chages)
		{
			// TODO: show message "Save changes?"

			if (_configName != MySettings.ConfigFile) 
			{

				// load new one
				if (!Configuration.ConfigMaganer.Init ( _configName, false )) {
					Debug.LogError("Can't load from new config file " + _configName);
					// TODO: show notification message
				}
			}

			MySettings.IsStartWithMenu = _startFromMenu;
			MySettings.IsStereo = _isStereo;
			MySettings.ConfigFile = _configName;
			MySettings.Save();

			if (Configuration.ConfigMaganer.Instance.Application.Debug != _isDebug) 
			{
				Configuration.ConfigMaganer.Instance.Application.Debug = _isDebug;
				Configuration.ConfigMaganer.SaveInstance( MySettings.ConfigFile );
			}

			Debug.Log("Configuration was saved");

		}

		this.enabled = false;
		_menu.enabled = true;
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void OnGUI() 
	{
		if (_styleValue == null) {
			InitStyles();
		}

		GUI.Label(_captionRect, Lang.MainMenu_Settings , ResizeHelper.MenuCaptionStyle);

		GUI.Label(_menuRect[0], Lang.Settings_Language, _styleLabel);
		var t = GUI.Toggle(_itemsRect[0], _isRussia, Lang.Settings_LanguageName, _styleValue );
		if (t != _isRussia) {
			_isRussia = !_isRussia;
			Lang.SwitchTo( _isRussia ? Lang.LangType.Russia : Lang.LangType.English);
		}

		GUI.Label(_menuRect[1], Lang.Settings_Stereo, _styleLabel);
		_isStereo = GUI.Toggle(_itemsRect[1], _isStereo, _isStereo ? Lang.Settings_On : Lang.Settings_Off, _styleValue );

		GUI.Label(_menuRect[2], Lang.Settings_StartWith, _styleLabel);
		_startFromMenu = GUI.Toggle(_itemsRect[2], _startFromMenu, _startFromMenu ? Lang.Settings_MainMenu : Lang.Settings_AR, _styleValue );

		GUI.Label(_menuRect[3], Lang.Settings_ConfigurationFile, _styleLabel);
		_configName = GUI.TextField(_itemsRect[3], _configName, _styleValue );
	
		GUI.Label(_menuRect[4], Lang.Settings_Debug, _styleLabel);
		_isDebug = GUI.Toggle(_itemsRect[4], _isDebug, _isDebug ? Lang.Settings_On : Lang.Settings_Off, _styleValue );

		if (GUI.Button(_backRect, Lang.Settings_Back, ResizeHelper.ButtonStyle)) {
			Close();
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
