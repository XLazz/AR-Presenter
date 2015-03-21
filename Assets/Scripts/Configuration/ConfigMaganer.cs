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
using System.Xml.Serialization;
using UnityEngine;

namespace Configuration 
{
	public class ConfigMaganer
	{
		protected const int CurrentVersion = 1;

		//public static readonly string DefaultConfigPath = Path.Combine(Application.persistentDataPath, "default.xml");
		
		public static Config Instance { get; protected set; }
		
		public static bool Init(string path, bool loadDefaults)
		{
			Debug.LogWarning(" ININIT:" + path);
			if (!path.StartsWith("/")) {
				path = Path.Combine(Application.persistentDataPath, path);
			}

			Debug.Log("path = " + path + " exists - " + (File.Exists(path)));
			
			if (!File.Exists(path)) {
				if (!loadDefaults) {
					Debug.Log("Configuration file not found: " + path);
					return false;
				}
				Debug.Log("Copy defaults");				
				LoadDefaults(path);
			}
			
			Debug.Log("Trying to load configuration from " + path);
			var ser = new XmlSerializer( typeof(Config) );
			
			try {
				using (var stream = new StreamReader(path))
				{
					Instance = (Config) ser.Deserialize(stream);
				}
			} catch (Exception ex) {
				Debug.LogError(ex.Message);
				Debug.Log("Can't load config file - inited by empty object");
				
				//Instance = new Config();
				return false;
			}

			// check version
			if (string.IsNullOrEmpty( Instance.Version ) || (int.Parse(Instance.Version) < CurrentVersion))
			{
				// reload default
				LoadDefaults(path);
			}
			
			return true;
		}

		public static void SaveInstance(string path)
		{
			if (!path.StartsWith("//")) {
				path = Path.Combine(Application.persistentDataPath, path);
			}

			var ser = new XmlSerializer( typeof(Config) );
			using (var writer = new StreamWriter(path))
			{
				ser.Serialize(writer, Instance);
			}	
		}
		
		protected static void LoadDefaults(string pathToPersist)
		{
			var doc = ((TextAsset) Resources.Load("defaultConfig")).text;
			var ser = new XmlSerializer( typeof(Config) );
			
			try
			{
				using (var reader = new StringReader(doc))
				{
					Instance = (Config) ser.Deserialize(reader);
					
					using (var writer = new StreamWriter(pathToPersist))
					{
						ser.Serialize(writer, Instance);
						
					}						
				}
				
				CopyFromResources(Instance.Application.DataPath, false);
				CopyFromResources(Instance.Application.XmlPath, false);

				// copy all resources
				foreach (var item in Instance.Items)
				{
					// icon
					CopyImageFromResources(item.Icon);

					foreach (var video in item.Videos) {
						CopyFromResources(video.Path, false);						
					}
					foreach (var page in item.Pages) {
						CopyFromResources(page.Path, true);						
					}
					foreach (var model in item.Models) {
						CopyFromResources(model.Path, false);		
						CopyFromResources(model.Path.Replace(".obj", ".mtl"), false);		
						CopyImageFromResources(model.Path.Replace(".obj", ".png"));							
					}

					foreach (var image in item.Images) {
						foreach (var p in image.Images) {
							CopyImageFromResources(p.Path);
						}
					}
				}

				Resources.UnloadUnusedAssets();
				GC.Collect();
				
			}
			catch (Exception ex) 
			{
				Debug.LogError("Can't load default configuration " + ex.Message);
			}
		}

		public static void CopyImageFromResources(string path)
		{
			var separatorIndex = path.LastIndexOf("/");
			var relPath = path.Substring(0, separatorIndex);
			var name = path.Substring(0, path.IndexOf("."));

			var data = (Texture2D) Resources.Load( name );
			
			if (data != null) 
			{
				var distDir = Path.Combine(Application.persistentDataPath, relPath);
				if (!Directory.Exists(distDir)) {
					Directory.CreateDirectory(distDir);
				}

				File.WriteAllBytes( Path.Combine(Application.persistentDataPath, path), data.EncodeToPNG() );
			}
			
		}

		public static void CopyFromResources(string path, bool isText)
		{
			var separatorIndex = path.LastIndexOf("/");
			var relPath = path.Substring(0, separatorIndex);
			//var name = path.Substring(separatorIndex + 1);

			var data = (TextAsset) Resources.Load( path.Replace(".", ""));

			if (data != null) 
			{
				var distDir = Path.Combine(Application.persistentDataPath, relPath);
				if (!Directory.Exists(distDir)) {
					Directory.CreateDirectory(distDir);
				}

				if (!isText) {
					File.WriteAllBytes( Path.Combine(Application.persistentDataPath, path), data.bytes );
				} else {
					File.WriteAllText( Path.Combine(Application.persistentDataPath, path), data.text, System.Text.Encoding.UTF8);
				}

			}

		}
	}
}