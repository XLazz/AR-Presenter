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
	public class VideoView : ViewBase
	{
		private VideoNode _video;
		
		private Rect _iconRect;
		private Texture2D _icon;
		
		public VideoView (Rect rect, VideoNode video) : base (rect)
		{
			var k = ResizeHelper.k;
			_video = video;

			var iconSize = 200f*k;
			var c = new Vector2( rect.x + rect.width/2, rect.y + rect.height/2);

			_iconRect = new Rect(c.x - iconSize/2, c.y - iconSize/2, iconSize, iconSize );

			_icon = (Texture2D) Resources.Load("icon_play");
		}

		public override string Caption {
			get { return _video.Title; }
		}

		public override void Draw()
		{
			if (_video == null) return;
			base.Draw();
			
			GUI.DrawTexture(_iconRect, _icon);
			if (GUI.Button(_iconRect, "", GUIStyle.none)) {
				var path = _video.Path.StartsWith("/") ? _video.Path : System.IO.Path.Combine( Application.persistentDataPath, _video.Path);
				Handheld.PlayFullScreenMovie("file://" + path, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
			}
		}
	}
}

