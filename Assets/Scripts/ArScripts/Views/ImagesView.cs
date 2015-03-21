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
	public class ImagesView : ViewBase
	{
		private ImageNode _image;
		private Texture2D[] _textures;
		private Rect[] _rects;

		private int _selectedImage;

		protected static Texture2D _slider;

		public Rect LeftSlider;
		public Rect _leftSliderUV;
		public Rect RightSlider;
		public Rect _rightSliderUV;

		public ImagesView (Rect rect, ImageNode image) : base (rect)
		{
			if (_slider == null) {
				_slider = (Texture2D) Resources.Load("slide_arrow");
			}

			var k = ResizeHelper.k;
			_image = image;

			_selectedImage = 0;

			_textures = new Texture2D[_image.Images.Length];
			_rects = new Rect[_image.Images.Length];

			for (int i = 0; i < _image.Images.Length; i++)
			{
				var path = _image.Images[i].Path.StartsWith("/") ? _image.Images[i].Path : System.IO.Path.Combine(Application.persistentDataPath, _image.Images[i].Path);

				_textures[i] = new Texture2D(1,1, TextureFormat.RGB565, false);
				_textures[i].LoadImage(System.IO.File.ReadAllBytes(path));
				_rects[i] = GetImageRect(new Vector2(_image.Images[i].Width, _image.Images[i].Height));
			}

			_leftSliderUV = new Rect(1,0, -1,1);
			_rightSliderUV = new Rect(0,0, 1, 1);
			LeftSlider = new Rect(_innerRect.x, _innerRect.y, 40f*k, _innerRect.height);
			RightSlider = new Rect(_innerRect.xMax - 40f*k, _innerRect.y, 40f*k, _innerRect.height);
		}

		public override string Caption {
			get { return _image.Title; }
		}
				
		public override void Draw()
		{
			if (_image == null) return;
			base.Draw();
			
			GUI.DrawTexture(_rects[_selectedImage], _textures[_selectedImage]);

			if (_selectedImage > 0) {
			GUI.DrawTextureWithTexCoords(LeftSlider, _slider, _leftSliderUV); 
			}
			if (_selectedImage + 1 < _textures.Length) {
				GUI.DrawTextureWithTexCoords(RightSlider, _slider, _rightSliderUV); 
			}
		}

		public void TakePrevImage()
		{
			if (_selectedImage > 0) {
				_selectedImage--;
			}
		}

		public void TakeNextImage()
		{
			if (_selectedImage + 1 < _textures.Length) {
				_selectedImage++;
			}
		}


		protected Rect GetImageRect(Vector2 size) {
			var w = _innerRect.height * size.x / size.y;
			if (w < _innerRect.width)
			{
				var res = new Rect( (_innerRect.width - w)/2 + _innerRect.x, _innerRect.y, w, _innerRect.height);
				return res;			       
			} else {
				var h = _innerRect.width * size.y / size.x;
				var res = new Rect( _innerRect.x, (_innerRect.height - h)/2 + _innerRect.y, _innerRect.width, h);
				return res;			       
			}
		}
	}
}

