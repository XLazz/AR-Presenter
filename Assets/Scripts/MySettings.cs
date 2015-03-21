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

public class MySettings
{
	private static bool _isStartWithMenu;
	public static bool IsStartWithMenu { get { return _isStartWithMenu; } set { _isStartWithMenu = value; }}

	private static bool _isStereo;
	public static bool IsStereo { get { return _isStereo; } set { _isStereo = value; }}

	private static string _configFile;
	public static string ConfigFile { get { return _configFile; } set { _configFile = value; }}


	static MySettings ()
	{
		if (PlayerPrefs.HasKey("IsStartWithMenu")) 
		{
			_isStartWithMenu = PlayerPrefs.GetInt("IsStartWithMenu") == 1;
			_isStereo = PlayerPrefs.GetInt("IsStereo") == 1;

			_configFile = PlayerPrefs.GetString("ConfigFile");
		} else 
		{
			// create default 
			_isStartWithMenu = true;
			_isStereo = false;
			_configFile = "default.xml";

			Save();
		}
	}

	public static void Save()
	{
		PlayerPrefs.SetInt("IsStartWithMenu", IsStartWithMenu ? 1 : 0);
		PlayerPrefs.SetInt("IsStereo", IsStereo ? 1 : 0);

		PlayerPrefs.SetString("ConfigFile", ConfigFile);

		PlayerPrefs.Save();
	}
}

