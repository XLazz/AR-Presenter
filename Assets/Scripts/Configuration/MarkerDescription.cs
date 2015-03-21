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

namespace Configuration 
{
	[XmlType("config")]
	public class Config
	{
		[XmlAttribute("version")]
		public string Version { get; set; }

		[XmlElement("application")]
		public ApplicationNode Application { get; set; }

		[XmlArray("items")]
		public ItemNode[] Items { get; set; }


		public ItemNode Get(string name)
		{
			foreach (var m in Items) {
				if (m.Marker == name) {
					return m;
				}
			}
			return null;
		}
	}

	[XmlType("application")]
	public class ApplicationNode
	{
		[XmlElement("debug")]
		public bool Debug { get; set; }

		[XmlElement("dataPath")]
		public string DataPath { get; set; }

		[XmlElement("xmlPath")]
		public string XmlPath { get; set; }
	}

	[XmlType("item")]
	public class ItemNode
	{
		[XmlElement("marker")]
		public string Marker { get; set; }

		[XmlIgnore]
		public bool Detected { get; set; }

		[XmlElement("title")]
		public string Title { get; set; }

		[XmlElement("icon")]
		public string Icon { get; set; }

		[XmlArray("html_pages")]
		public PageNode[] Pages { get; set; }

		[XmlArray("models")]
		public ModelNode[] Models { get; set; }

		[XmlArray("videos")]
		public VideoNode[] Videos { get; set; }

		[XmlArray("images")]
		public ImageNode[] Images { get; set; }
	}

	[XmlType("html_page")]
	public class PageNode
	{
		[XmlElement("path")]
		public string Path { get; set; }
		
		[XmlElement("title")]
		public string Title { get; set; }
	}

	[XmlType("image")]
	public class ImageNode
	{
		[XmlElement("title")]
		public string Title { get; set; }

		[XmlArray("refs")]
		public ImageInfo[] Images { get; set; }
	}

	[XmlType("ref")]
	public class ImageInfo
	{
		[XmlText]
		public string Path {get; set; }

		[XmlAttribute("width")]
		public int Width { get; set; }

		[XmlAttribute("height")]
		public int Height { get;set; }
	}

	[XmlType("model")]
	public class ModelNode
	{
		[XmlElement("path")]
		public string Path { get; set; }
		
		[XmlElement("title")]
		public string Title { get; set; }

		[XmlElement("description")]
		public string Description { get; set; }
	}

	[XmlType("video")]
	public class VideoNode
	{
		[XmlElement("path")]
		public string Path { get; set; }
		
		[XmlElement("title")]
		public string Title { get; set; }
		
		[XmlElement("description")]
		public string Description { get; set; }
	}
}

