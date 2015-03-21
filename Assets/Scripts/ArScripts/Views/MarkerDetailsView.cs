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
	public abstract class ViewBase
	{
		protected Rect _rect;
		protected Rect _innerRect;

		private Texture2D _border;
		private Texture2D _bg;

		public ViewBase(Rect rect)
		{
			var k = ResizeHelper.k;
			
			_rect = rect;
			_innerRect = new Rect(rect.x + 8f*k, rect.y + 8f*k,  rect.width - 16f*k, rect.height - 16f*k);
			
			_border = ResizeHelper.Make( new Color32(20,93,134, 200));
			_bg = ResizeHelper.Make( new Color(1,1,1, 0.8f));
		}

		public abstract string Caption { get; }

		public virtual void Free() {}

		public virtual void Leave() {}
		public virtual void Enter() {}

		public virtual void Draw()
		{
			GUI.DrawTexture(_rect, _border);
			GUI.DrawTexture(_innerRect, _bg);
		}

		public virtual void MouseDownHandler(Vector2 p) {}
		public virtual void MouseUpHandler(Vector2 p) {}
		public virtual void MouseMoveHandler(Vector2 p) {}
	}
}

