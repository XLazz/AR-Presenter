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

using System;
using UnityEngine;
using Configuration;

namespace ArScripts.Views
{
	public class Model3DView : ViewBase
	{
		private ModelNode _model;

		public Material StandardMaterial { get; set; }
		public Material TransparentMaterial { get; set; }
		public Camera CameraLeft { get; set; }
		public Camera CameraRight { get; set; }

		private bool _inited;
		private GameObject[] _obj;

		private Texture2D _plusIcon;
		private Texture2D _minusIcon;

		public Rect ZoomInRect;
		public Rect ZoomOutRect;
		
		public Model3DView (Rect rect, ModelNode model, Texture2D plusIcon, Texture2D minusIcon) : base (rect)
		{
			_model = model;
			_plusIcon = plusIcon;
			_minusIcon = minusIcon;

			var p = 12f * ResizeHelper.k;
			var s = 64f * ResizeHelper.k;
			ZoomOutRect = new Rect(rect.xMax - 2*s - p, rect.yMax - s - p, s,s);
			ZoomInRect = new Rect(rect.xMax - s - p, rect.yMax - s - p, s,s);
		}

		void ReadModel()
		{		
			var path = _model.Path.StartsWith("/") ? _model.Path : System.IO.Path.Combine(Application.persistentDataPath, _model.Path);
			_obj = ObjReader.use.ConvertFile (path, true, StandardMaterial, TransparentMaterial);
			foreach (var obj in _obj)
			{
				obj.layer = 20;
				obj.transform.parent = CameraLeft.transform;
				obj.transform.localPosition = MySettings.IsStereo ? new Vector3(2f, -3f, 15f) : new Vector3(4f, -3f, 15f);
				obj.transform.localScale = new Vector3(5f, 5, 5f);
				//obj.AddComponent<ModelRotator>();
			}
		}

		public override void Free() {
			if ((_obj != null) && (_obj.Length > 0))
			{
				foreach (var obj in _obj) {
					GameObject.Destroy(obj);
				}
				_obj = null;
			}

			_inited = false;
			CameraLeft.gameObject.SetActive(false);
			Resources.UnloadUnusedAssets();
		}

		public override void Leave() {
			CameraLeft.gameObject.SetActive(false);
			if (_obj != null) {
				foreach (var o in _obj) {
					o.SetActive(false);
				}
			}
		}

		public override void Enter() {
			if (!_inited) {
				ReadModel();
				_inited = true;
			}

			CameraLeft.gameObject.SetActive(true);

			if (_obj != null) {
				foreach (var o in _obj) {
					o.SetActive(true);
				}
			}
		}

		public void ZoomIn()
		{
			foreach (var obj in _obj) {
				obj.transform.localScale = obj.transform.localScale * 1.05f;
			}
		}
		public void ZoomOut()
		{
			foreach (var obj in _obj) {
				obj.transform.localScale = obj.transform.localScale * 0.95f;
			}
		}

		public override string Caption {
			get { return _model.Title; }
		}

		public override void Draw()
		{
			if (_model == null) return;
			base.Draw();

			if (Event.current.type == EventType.Repaint)
			{
				foreach (var obj in _obj)
				{
					obj.transform.Rotate(0, 0.5f, 0);
				}

				if (CameraLeft.gameObject.activeSelf) {
					CameraLeft.Render();
					if (CameraRight != null) {
						CameraRight.Render();
					}
				}

				GUI.DrawTexture(ZoomInRect, _plusIcon);
				GUI.DrawTexture(ZoomOutRect, _minusIcon);
			}
			
		}
	}
}

