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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.IO;
using ArScripts.Views;
using Configuration;

public class MarkerDetailsView : MonoBehaviour {

	public class MenuItem
	{
		public Rect rect;
		public ViewBase Drawer;	
	}

	public Material StandardMaterial;
	public Material TransparentMaterial;

	[HideInInspector]
	public Camera CameraLeft;
	[HideInInspector]
	public Camera CameraRight;
	
	private ItemNode _marker;
	private MarkersLister _markerLister;

	private Texture2D _darkBlue;
	private Texture2D _mediumBlue;
	private Texture2D _lightBlue;

	private MenuItem _selectedMenu = null;

	private Rect _panelCaptionRect;
	private Rect _panelRect;

	private Rect _itemsRect;
	private Rect _itemsViewRect;
	private Vector2 _panelPosition;

	private List<MenuItem> _panelItems = new List<MenuItem>();

	private Rect _closeRect;
	public Texture2D CloseIcon;

	public Texture2D PlusIcon;
	public Texture2D MinusIcon;

	private Rect _mainRect;

	//private TouchEmulator _touch;
	private NewMouseController _touch;
	private bool _inited;

	///// variables ////////////////////////////////////////////////////////////////////////////////////////////////

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
	void Start () {
		SafeInit();
		
		//ConfigMaganer.Init(Path.Combine(Application.persistentDataPath, MySettings.ConfigFile), false);
		//SetMarker( ConfigMaganer.Instance.Items[0] );
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
	void SafeInit()
	{
		if (_inited) return;
		_inited = true;

		float k = ResizeHelper.k;

		_touch = Camera.main.GetComponent<NewMouseController>();
		_touch.Click += HandleTouchClick;
		_touch.MouseDown += HandleMouseDown;
		_touch.MouseUp += HandleMouseUp;
		_touch.MouseMove += HandleMouseMove;

		_markerLister = this.GetComponent<MarkersLister>();

		_darkBlue = ResizeHelper.Make( new Color32(15,70,102,255));
		_mediumBlue = ResizeHelper.Make( new Color32(20,93,134,255));
		_lightBlue = ResizeHelper.Make( new Color32(29,133,192,255));

		_closeRect = new Rect (Screen.width - 30f*k, 0, 30f*k, 30f*k);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
	void HandleMouseMove (Vector2 pos)
	{
		if (_selectedMenu != null) {
			_selectedMenu.Drawer.MouseMoveHandler(pos);
		}
	}

	void HandleMouseUp (Vector2 pos)
	{
		if (_selectedMenu != null) {
			_selectedMenu.Drawer.MouseUpHandler(pos);
		}
	}

	void HandleMouseDown (Vector2 pos)
	{
		if (_selectedMenu != null) {
			_selectedMenu.Drawer.MouseDownHandler(pos);
		}
	}
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
	void HandleTouchClick (Vector2 p)
	{
		if (!this.enabled) return;
		for (int i = 0; i < _panelItems.Count; i++) {

			var testRect = new Rect( _itemsRect.x, _itemsRect.y + _itemsViewRect.y - _panelPosition.y + _panelItems[i].rect.y,
			                        _panelItems[i].rect.width, _panelItems[i].rect.height);

			if (testRect.Contains(p)) {
				if (_selectedMenu != null) {
					_selectedMenu.Drawer.Leave();
				}
				_selectedMenu = _panelItems[i];
				_selectedMenu.Drawer.Enter();
				return;
			}

			if ((_selectedMenu != null) && (_selectedMenu.Drawer is ImagesView))
			{
				var img = (ImagesView) _selectedMenu.Drawer;
				if (img.LeftSlider.Contains(p)) {
					img.TakePrevImage();
					return;
				} else if (img.RightSlider.Contains(p)) {
					img.TakeNextImage();
					return;
				}

			}
			if ((_selectedMenu != null) && (_selectedMenu.Drawer is Model3DView))
			{
				var model = (Model3DView) _selectedMenu.Drawer;
				if (model.ZoomInRect.Contains(p)) {
					model.ZoomIn();
					return;
				} else if (model.ZoomOutRect.Contains(p)) {
					model.ZoomOut();
					return;
				}
				
			}
		}

		if (_closeRect.Contains(p)) {
			Close ();
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
	public void Close()
	{
		foreach (var menu in _panelItems) {
			menu.Drawer.Free();
		}
		_panelItems.Clear();

		this.enabled = false;
		_markerLister.enabled = true;
	}
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.Escape)) {
			Close();
		}*/
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
	void OnGUI()
	{
		if (_marker == null) return;
		if (MySettings.IsStereo) 
		{
			var m = GUI.matrix;
			var n = GUI.matrix;
			n.m00 = 0.5f;
			GUI.matrix = n;
			Draw();
			
			n.m03 = Screen.width/2;
			GUI.matrix = n;
			Draw();
			
			GUI.matrix = m;
		} else {
			Draw();
		}
	}
	
	void Draw()
	{
		GUI.DrawTexture(_panelRect, _mediumBlue);
		GUI.Label(_panelCaptionRect, _marker.Title, ResizeHelper.MarkerDescriptionStyle);

		_panelPosition = GUI.BeginScrollView( _itemsRect, _panelPosition, _itemsViewRect );

		for (int i = 0; i < _panelItems.Count; i++) {
			GUI.DrawTexture(_panelItems[i].rect, _selectedMenu == _panelItems[i] ? _lightBlue : _darkBlue);
			/*if (GUI.Button(_panelItems[i].rect, _panelItems[i].Drawer.Caption, ResizeHelper.DetailsMenuItem))
			{
				_selectedMenu = _panelItems[i];				
			}*/
			GUI.Label(_panelItems[i].rect, _panelItems[i].Drawer.Caption, ResizeHelper.DetailsMenuItem);
		}

		GUI.EndScrollView();

		if (_selectedMenu.Drawer != null) {
			_selectedMenu.Drawer.Draw();
		}

		GUI.DrawTexture(_closeRect, CloseIcon);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
	public void SetMarker(ItemNode marker)
	{
		SafeInit();

		_marker = marker;

		var k = ResizeHelper.k;

		var maxItemWidth = 250*k;

		_panelRect = new Rect(0, 0, maxItemWidth + (8f + 8f)*k*2f, Screen.height);
		
		_mainRect = new Rect ( _panelRect.xMax + 20f*k, 30f*k, Screen.width - 60f*k - _panelRect.width, Screen.height - 40f*k);

		var captionHeight = ResizeHelper.MarkerDescriptionStyle.CalcHeight(new GUIContent( _marker.Title ), _panelRect.width);
		_panelCaptionRect = new Rect(10f*k,0, _panelRect.width, captionHeight + 10f*k );
		_itemsRect = new Rect( _panelRect.x + 10*k, _panelCaptionRect.yMax + 10f*k, _panelRect.width - 10f*2*k, Screen.height - _panelCaptionRect.yMax - 10f*2*k);

		_panelItems.Clear();
		if (_marker.Pages != null) {
			foreach (var page in _marker.Pages) {
				_panelItems.Add( new MenuItem { Drawer = new DescriptionView( _mainRect, page  ) } );
			}
		}

		if (_marker.Models != null) {
			foreach (var model in _marker.Models) {
				_panelItems.Add( new MenuItem { Drawer = new Model3DView( _mainRect, model, PlusIcon, MinusIcon ) {
						StandardMaterial = StandardMaterial, 
						TransparentMaterial = TransparentMaterial, 
						CameraLeft = CameraLeft,
						CameraRight = CameraRight} } );
			}
		}

		if (_marker.Images != null) {
			foreach (var image in _marker.Images) {
				_panelItems.Add( new MenuItem { Drawer = new ImagesView( _mainRect, image ) } );
			}
		}

		if (_marker.Videos != null) {
			foreach (var video in _marker.Videos) {
				_panelItems.Add( new MenuItem {  Drawer = new VideoView( _mainRect, video ) } );
			}
		}

		for (int i = 0; i < _panelItems.Count; i++) {
			var size = ResizeHelper.DetailsMenuItem.CalcHeight(new GUIContent( _panelItems[i].Drawer.Caption) , _itemsRect.width);
			_panelItems[i].rect = new Rect(0, (size + 10f*k + 6f*k)*i, _itemsRect.width, size + 6f*k);
		}
		_itemsViewRect = new Rect(0,0, _itemsViewRect.width - 10f*k, _panelItems[_panelItems.Count - 1].rect.yMax + 10f*k);

		_selectedMenu = _panelItems[0];
	}
}
