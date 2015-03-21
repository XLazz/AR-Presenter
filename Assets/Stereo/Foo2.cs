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
using System.Runtime.InteropServices;

public class Foo2 : MonoBehaviour {

	Rect _rect;
	bool _is3D;

	Texture2D _red;
	Rect _redRect;
	Texture2D _blue;
	Rect _blueRect;

	string _error;
	Rect _errorRect;
	GUIStyle _style;
	
	// Use this for initialization
	void Start () {
		_rect = new Rect(0,0,200,100);

		_red = new Texture2D(1,1, TextureFormat.ARGB32, false);
		_red.SetPixel(0,0, new Color(1f,0,0));
		_red.Apply();
		_redRect = new Rect(0,0, Screen.width/2, Screen.height);

		_blue = new Texture2D(1,1, TextureFormat.ARGB32, false);
		_blue.SetPixel(0,0, new Color(0,0,1f));
		_blue.Apply();
		_blueRect = new Rect(Screen.width/2,0, Screen.width/2, Screen.height);

		_errorRect = new Rect(0, Screen.height/2, Screen.width/2, Screen.height/2);
		_error = "1\n2\n3\n4\n5";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUI.DrawTexture(_redRect, _red);
		GUI.DrawTexture(_blueRect, _blue);

		if (_is3D) {
			if (GUI.Button(_rect, "Return to 2D")) {
				_is3D = false;
				SwitchMode();
				Debug.Log("State" + _is3D);
			}
		} else {
			if (GUI.Button(_rect, "Switch to 3D")) {
				_is3D = true;
				SwitchMode();
				Debug.Log("State" + _is3D);
			}
		}

		if (_style == null) {
			_style = new GUIStyle(GUI.skin.label);
			_style.fontSize = 30;
		}

		if (!string.IsNullOrEmpty(_error)) {
			GUI.Label(_errorRect, _error, _style);
		}
	}

	void SwitchMode()
	{
		try {
			using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				_error = "UnityPlayer getted " + (unityPlayer == null) + "\n";
				using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					_error += "currentActivity getted " + (activity == null) + "\n";
				 	using (AndroidJavaObject app = activity.Call<AndroidJavaObject>("getApplicationContext")) 
					{
						_error += "getApplicationContext called " + (app == null) + "\n";
						using (AndroidJavaObject displayControl = new AndroidJavaObject("jp.epson.moverio.bt200.DisplayControl", new object[] { app }))
						{
							_error += "DisplayControl created " + (displayControl == null) + "\n";					
							int mode = _is3D ? 1 : 0;
							bool toast = _is3D;
							displayControl.Call<int>("setMode", new object[] { mode, toast });

							_error += "Done";
						}					
					}
				}
			} 
		}
		catch (System.Exception ex) {
			_error = ex.Message;
		}
		
	}
}
