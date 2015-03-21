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
using System.IO;
using UnityEngine;
using Configuration;

namespace ArScripts.Views
{
	public class DescriptionView : ViewBase
	{
		private PageNode _page;
		private string _description;

		private Texture2D _preview;
		private Vector2 _previewSize;

		private Rect _textRect;
		private Rect _viewRect;

		public DescriptionView (Rect rect, PageNode page) : base (rect)
		{
			var k = ResizeHelper.k;
			_textRect = new Rect(rect.x + 20f*k, rect.y + 20f*k,  rect.width - 40f*k, rect.height - 40f*k);

			_page = page;
			var path = page.Path.StartsWith("/") ? page.Path : Path.Combine(Application.persistentDataPath, page.Path);
			if (File.Exists(path)) {
				_description = File.ReadAllText(path, System.Text.Encoding.UTF8);
			} else {
				_description = "<file not found>";
			}

			_description = ReplaceImgTag(_description);
			_description = _description.Replace("<p>", "").Replace("<P>", "").Replace("</p>", "\r\n\r\n").Replace("</P>", "\r\n\r\n");
			_description = _description.Replace("<br>", "\n").Replace("<br />", "\n").Replace("<br/>", "\n");
			_description = _description.Replace("<BR>", "\n").Replace("<BR />", "\n").Replace("<BR/>", "\n");

			_viewRect = new Rect(0,0, _textRect.width - 36f*k, 
			                     ResizeHelper.DescriptionText.CalcHeight(new GUIContent(_description), _textRect.width - 36f*k)
			                     + (_preview == null ? 0 : _previewSize.y));
		}

		protected string ReplaceImgTag(string text)
		{
			var index = text.IndexOf("<img");
			if (index != -1)
			{
				int len = "</img>".Length;
				var endIndex = text.IndexOf("</img>", index);
				if (endIndex == -1) {
					endIndex = text.IndexOf("/>", index);					
					len = "/>".Length;					
				}

				if (endIndex != -1)
				{
					// we found our tag
					var imgtext = text.Substring(index, endIndex - index + len).ToLower();
					text = text.Remove(index, imgtext.Length);

					var doc = new System.Xml.XmlDocument();
					try {
						doc.LoadXml(imgtext);

						var sw = doc.FirstChild.Attributes["width"];
						var sh = doc.FirstChild.Attributes["height"];
						var ss = doc.FirstChild.Attributes["src"];
						if (sw == null || sh == null || ss == null) {
							Debug.LogError("img tag must have 'width' 'heght' and 'src' attributes: " + imgtext);
						} else {
							var path = ss.Value.StartsWith("/") ? ss.Value : Path.Combine(Application.persistentDataPath, ss.Value);
							if (!File.Exists(path)) {
								Debug.LogError("img tag source not found: " + path);								
							} else {
								int width, height;
								if (!int.TryParse(sw.Value, out width)  || !(int.TryParse(sh.Value, out height))) {
									Debug.LogError("img tag can't [arce size: " + sw.Value + "," + sh.Value);																	
								} else {
									_previewSize = new Vector2(width, height);
									_preview = new Texture2D(1,1, TextureFormat.RGB565, false);
									_preview.LoadImage(System.IO.File.ReadAllBytes(path));
								}
							}
						}

					} catch (Exception ex) {
						Debug.LogError("Couldn't parse img tag: " + ex.Message);						
						
					}


					/*var wi = imgtext.IndexOf("width=");
					if (wi == -1) {
						Debug.LogError("Couldn't find width attribute: " + imgtext);						
					} else {
						wi += "width=".Length + 1;
						if ((imgtext[wi] == '\"') || (imgtext[wi] == '\'')) wi++;

						var wi2 = imgtext.IndexOfAny(new [] {'\'', '\"', ' '}, wi);
						if (wi2 == -1) {
							Debug.LogError("Couldn't find width attribute ending: " + imgtext);						
						} else {
							int width;
							if (int.TryParse(imgtext.t
						}
					}*/

				} else {
					Debug.LogError("Couldn't find the end of img tag: " + text);
				}
			}
			return text;
		}

		public override string Caption {
			get { return _page.Title; }
		}

		public Vector2 _scrollPostion;
		public override void Draw()
		{
			if (_page == null) return;
			base.Draw();


			_scrollPostion = GUI.BeginScrollView(_textRect, _scrollPostion,_viewRect);
			GUILayout.BeginArea(_viewRect);

			if (_preview != null) {
				GUILayout.Label(_preview, GUILayout.Width(_previewSize.x), GUILayout.Height(_previewSize.y));
			}
			GUILayout.Label(_description, ResizeHelper.DescriptionText);

			GUILayout.EndArea();
			GUI.EndScrollView();
		}

		private Vector2 _mouseScrollStartPos;
		private bool _mouseScroll;
		public override void MouseDownHandler(Vector2 p) 
		{
			if (_textRect.Contains(p)){
				_mouseScroll = true;
				_mouseScrollStartPos = p;
			}
		}
		public override void MouseUpHandler(Vector2 p)
		{
			if (_mouseScroll) {
				_mouseScroll = false;
			}
		}
		public override void MouseMoveHandler(Vector2 p)
		{
			if (_mouseScroll) {
				_scrollPostion = new Vector2(_scrollPostion.x, _scrollPostion.y + (_mouseScrollStartPos.y - p.y));
				_mouseScrollStartPos = p;
			}
		}
	}
}

