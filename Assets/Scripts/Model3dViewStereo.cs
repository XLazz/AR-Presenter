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

public class Model3dViewStereo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var ar = Camera.main.GetComponent<MarkerDetailsView>();
		var self = this.GetComponent<Camera>();
		ar.CameraLeft = self;
		
		if (MySettings.IsStereo) {

			var cameraName = this.gameObject.name;
			this.gameObject.name = cameraName + "(left)";

			var cameraObject = new GameObject(cameraName + "(right)");
			var camera = cameraObject.AddComponent<Camera>();
			camera.nearClipPlane = self.nearClipPlane;
			camera.farClipPlane = self.farClipPlane;
			
			cameraObject.transform.position = Vector3.zero;
			cameraObject.transform.rotation = Quaternion.Euler(0,0,0);
			cameraObject.transform.localScale = Vector3.one;

			self.rect = new Rect(0,0, 0.5f, 1);
			camera.rect = new Rect(0.5f,0, 0.5f, 1);
			camera.depth = self.depth;
			camera.cullingMask = self.cullingMask;
			camera.clearFlags = CameraClearFlags.Depth;

			cameraObject.transform.parent = this.transform;
			cameraObject.transform.localPosition = new Vector3(1, 0, 0);
			this.transform.position = new Vector3(-1, 0, 0);

			// disable to manual render
			camera.enabled = false;

			ar.CameraRight = camera;

		} else {
			ar.CameraRight = null;

			this.enabled = false;
		} 

		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
