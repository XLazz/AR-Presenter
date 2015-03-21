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
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Configuration;

public class MarkersLister : MonoBehaviour {

	private const int CurrentVersion = 1;

	private float k;
	private Dictionary<string, Texture2D> _icons = new Dictionary<string, Texture2D>();
	private Dictionary<TrackableEventHandler, ItemNode> _markers = new Dictionary<TrackableEventHandler, ItemNode>();

	private Rect[] _rectsIcon = new Rect[3];
	private Rect[] _rectsWithIcon = new Rect[3];
	private Rect[] _rectsWithoutIcon = new Rect[3];

	private Texture2D DescriptionBG;
	//private TouchEmulator _touch;
	private NewMouseController _touch;
	private MarkerDetailsView _details;
	
	private Rect _backRect;
	
	// Use this for initialization
	void Start () {
		k = ResizeHelper.k;

		for (int i = 0; i < _rectsIcon.Length; i++) {
			_rectsIcon[i] = new Rect(16f*k,  16f*k + i*70f*k, 64f*k, 64f*k);
			_rectsWithIcon[i] = new Rect(16f*k + 64f*k,  16f*k + i*70f*k, 200f*k, 64f*k);
			_rectsWithoutIcon[i] = new Rect(16f*k,  16f*k + i*70f*k, 200f*k + 64f*k, 64f*k);
		}

		_touch = Camera.main.GetComponent<NewMouseController>();
		_touch.Click += HandleTouchClick;

		_details = this.GetComponent<MarkerDetailsView>();

		DescriptionBG = ResizeHelper.Make( new Color32(20, 93, 134, 255));

		var size = ResizeHelper.ButtonStyle.CalcSize(new GUIContent(Lang.Settings_Back) );
		_backRect = new Rect(Screen.width - 10f*k - size.x, Screen.height - 50f*k, size.x, 50f*k);		
	}

	void HandleTouchClick (Vector2 p)
	{
		if (!this.enabled) return;

		//p = new Vector2(p.x, Screen.height - p.y);
		int index = 0;

		foreach (var marker in _markers.Values)
		{
			if (marker.Detected ) 
			{
				var rect = _icons.ContainsKey(marker.Marker) ? _rectsWithIcon[index] : _rectsWithoutIcon[index];
				if (rect.Contains(p)) {
					// found click on shortInfo

					//Debug.Log("Clicked on [" + index + "] marker.Name = " + marker.Name + ", " + Time.time);
					_details.SetMarker(marker);
					_details.enabled = true;
					this.enabled = false;

					break;
				}
				index++;
			}
		}

		if (_backRect.Contains(p)) {
			Application.LoadLevel("MenuScene");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_markers == null) return;
		//if (Input.GetKeyDown( KeyCode.Escape) ) {
		//	Application.LoadLevel("MenuScene");			
		//}
	}


	void OnGUI()
	{
		if (_markers == null) return;

		if (MySettings.IsStereo) 
		{
			var m = GUI.matrix;
			var n = GUI.matrix;
			n.m00 = 0.5f;
			GUI.matrix = n;
			Draw ();
			
			n.m03 = Screen.width/2;
			GUI.matrix = n;
			Draw ();

			GUI.matrix = m;
		} else {
			Draw ();
		}
	}

	void Draw()
	{
		GUI.Label(_backRect, Lang.Settings_Back, ResizeHelper.ButtonStyle);	

		int index = 0;
		foreach (var marker in _markers.Values)
		{
			if (marker.Detected) 
			{
				DrawMakerCard(marker, index);
				index++;
			}
		}
	}

	void DrawMakerCard(ItemNode marker, int index)
	{
		var name = marker.Marker;
		if (_icons.ContainsKey(name))
		{
			GUI.DrawTexture(_rectsIcon[index], _icons[name]);
			GUI.DrawTexture(_rectsWithIcon[index], DescriptionBG);
			GUI.Label(_rectsWithIcon[index], marker.Title, ResizeHelper.MarkerDescriptionStyle);
		} else {
			GUI.DrawTexture(_rectsWithoutIcon[index], DescriptionBG);
			GUI.Label(_rectsWithoutIcon[index], marker.Title, ResizeHelper.MarkerDescriptionStyle);
		}
	}


	public void AddMarker(string markerName, TrackableEventHandler trackableEvent)
	{
		var m = Configuration.ConfigMaganer.Instance.Get( markerName );
		if (m != null) {
			trackableEvent.marker = m;
			_markers.Add(trackableEvent, m );
			var texture = new Texture2D(128,128, TextureFormat.RGB565, false);

			var path = m.Icon.StartsWith("/") ? m.Icon : Path.Combine( Application.persistentDataPath, m.Icon);
			if (texture.LoadImage( File.ReadAllBytes( path ) )) {
				_icons.Add(markerName, texture);
			} else {
				Debug.LogWarning("Icon not found " + markerName + " by key " + m.Icon);
			}
		} else {
			Debug.LogWarning(markerName + " has been not found in data file");
		}

	}



}
