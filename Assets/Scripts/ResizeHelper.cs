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

public class ResizeHelper {

	public static float k;

	public static GUIStyle MenuItemStyle;
	public static GUIStyle MenuCaptionStyle;
	public static GUIStyle MenuHintStyle;
	public static GUIStyle ButtonStyle;

	public static GUIStyle MarkerDescriptionStyle;

	public static GUIStyle AboutTextStyle;

	public static GUIStyle DetailsMenuItem;
	public static GUIStyle DescriptionText;
	

	private static GUISkin _skin;

	static ResizeHelper()
	{
		k = Screen.height / 480f;

		_skin = (GUISkin) Object.Instantiate( Resources.Load("DefaultSkin") );

		MenuItemStyle = new GUIStyle(_skin.button);
		MenuItemStyle.fontSize = (int)(40f*k);
		MenuItemStyle.fontStyle = FontStyle.Bold;
		SetColor(MenuItemStyle, new Color32(17, 229, 212, 255));
		MenuItemStyle.hover.textColor = MenuItemStyle.onHover.textColor = new Color32(131, 252, 242, 255);
		MenuItemStyle.active.textColor = MenuItemStyle.onActive.textColor = new Color32(200, 255, 176, 255);
		MenuItemStyle.alignment = TextAnchor.MiddleCenter;
		SetBackgroud(MenuItemStyle, Make(new Color(0,0,0,0)));

		MenuCaptionStyle = new GUIStyle(_skin.label);
		MenuCaptionStyle.fontSize = (int)(44f*k);
		MenuCaptionStyle.fontStyle = FontStyle.Bold;
		SetColor(MenuCaptionStyle, new Color(1,1,1));
		MenuCaptionStyle.alignment = TextAnchor.MiddleCenter;

		MenuHintStyle = new GUIStyle(_skin.label);
		MenuHintStyle.fontSize = (int)(24f*k);
		SetColor(MenuHintStyle, new Color(1,1,1));
		MenuHintStyle.alignment = TextAnchor.MiddleCenter;

		ButtonStyle = new GUIStyle(_skin.button);
		ButtonStyle.fontSize = MenuItemStyle.fontSize;
		ButtonStyle.alignment = TextAnchor.MiddleRight;
		SetColor(ButtonStyle, new Color32(212, 210, 22, 255));
		ButtonStyle.hover.textColor = ButtonStyle.onHover.textColor = new Color32(245, 245, 98, 255);
		ButtonStyle.active.textColor = ButtonStyle.onActive.textColor = new Color32(241, 215, 110, 255);
		SetBackgroud(ButtonStyle, Make(new Color(0,0,0,0)));

		MarkerDescriptionStyle  = new GUIStyle(_skin.label);
		MarkerDescriptionStyle.fontSize = (int)(20f*k);
		MarkerDescriptionStyle.padding = new RectOffset((int)(8*k), (int)(8*k), (int)(8*k), (int)(8*k));
		MarkerDescriptionStyle.alignment = TextAnchor.MiddleLeft;
		SetColor(MarkerDescriptionStyle, new Color32(255, 255, 255, 255));
		MarkerDescriptionStyle.hover.textColor = MarkerDescriptionStyle.onHover.textColor = new Color32(255, 245, 217, 255);
		MarkerDescriptionStyle.active.textColor = MarkerDescriptionStyle.onActive.textColor = new Color32(255, 245, 217, 255);

		AboutTextStyle = new GUIStyle(_skin.label);
		AboutTextStyle.fontSize = (int)(26f*k);
		AboutTextStyle.alignment = TextAnchor.UpperCenter;
		SetColor(AboutTextStyle, new Color32(255, 255, 255, 255));

		DetailsMenuItem  = new GUIStyle(_skin.label);
		DetailsMenuItem.fontSize = (int)(24f*k);
		DetailsMenuItem.padding = new RectOffset((int)(8*k), (int)(8*k), (int)(8*k), (int)(8*k));
		DetailsMenuItem.wordWrap = false;
		DetailsMenuItem.alignment = TextAnchor.MiddleCenter;
		SetColor(DetailsMenuItem, new Color32(255, 255, 255, 255));

		DescriptionText  = new GUIStyle(_skin.label);
		DescriptionText.fontSize = (int)(24f*k);
		DescriptionText.wordWrap = true;
		DescriptionText.alignment = TextAnchor.UpperLeft;
		SetColor(DescriptionText, new Color32(0, 0, 0, 255));
	}


	protected static void Scale(GUIStyle s, float k)
	{
		s.fontSize = (int)(s.fontSize*k);
		var p = s.padding;
		s.padding = new RectOffset((int)(p.left*k), (int)(p.right*k), (int)(p.top*k), (int)(p.bottom*k));
		
		var m = s.margin;
		s.margin = new RectOffset((int)(m.left*k), (int)(m.right*k), (int)(m.top*k), (int)(m.bottom*k));
	}
	
	protected static void SetBackgroud(GUIStyle s, Texture2D t)
	{
		s.active.background = s.focused.background = s.hover.background = s.normal.background 
			= s.onActive.background = s.onFocused.background = s.onHover.background = s.onNormal.background
				= t;
	}
	
	public static void SetColor(GUIStyle s, Color c)
	{
		s.active.textColor = s.focused.textColor = s.hover.textColor = s.normal.textColor 
			= s.onActive.textColor = s.onFocused.textColor = s.onHover.textColor = s.onNormal.textColor
				= c;
	}

	public static Texture2D Make(Color c)
	{
		var t = new Texture2D(1,1, TextureFormat.ARGB32, false);
		t.SetPixel(0,0, c);
		t.Apply();
		return t;
	}

}
