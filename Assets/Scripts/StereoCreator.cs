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
using System.Reflection;

public class StereoCreator : MonoBehaviour {

	public Material BgMaterial;
	private Camera _leftCamera;
	private Camera _rightCamera;

	private GameObject _leftBackground;
	private GameObject _rightBackground;

	private Texture2D _backgroundTexture;

	private Rect _rect;

	// Use this for initialization
	IEnumerator Start () {
		while (!QCARManager.Instance.Initialized) {
			yield return new WaitForEndOfFrame();
		}

		if (MySettings.IsStereo) {
			if (Configuration.ConfigMaganer.Instance.Application.Debug) {
				StartCoroutine( InitBackgrounds() );
			} else {
				StartCoroutine( InitOnlyCameras() );
			}
		} else {
			this.enabled = false;
		}

		_rect = new Rect(0,0, 150, 150);
	}

	IEnumerator InitOnlyCameras()
	{
		while (!QCARManager.Instance.Initialized) {
			yield return new WaitForEndOfFrame();
		}

		QCARRenderer.Instance.DrawVideoBackground = false;
		
		var arCamera = this.GetComponent<Camera>();
		
		var leftMask = (uint)(1 << 27);
		var rightMask = (uint)(1 << 28);
		arCamera.cullingMask = (int)((uint)(arCamera.cullingMask) 
		                             & (~leftMask)
		                             & (~rightMask));
		
		_leftCamera = CreateCamera("LeftCamera", arCamera, new Rect(0,0, 0.5f, 1));
		_leftCamera.cullingMask = (int)((uint)(arCamera.cullingMask) | leftMask );
		_leftCamera.gameObject.layer = 27;
		
		
		_rightCamera = CreateCamera("RightCamera", arCamera, new Rect(0.5f,0, 0.5f, 1));
		_rightCamera.cullingMask = (int)((uint)(arCamera.cullingMask) | rightMask );
		
		SetPosition(this.gameObject, _leftCamera.gameObject, new Vector3(-1, 0, 0));
		SetPosition(this.gameObject, _rightCamera.gameObject, new Vector3(1, 0, 0));
		
		arCamera.cullingMask = 0;
		
		var qcar = this.GetComponent<QCARAbstractBehaviour>();
		var field = typeof(QCARAbstractBehaviour).GetField("mCachedCameraClearFlags", BindingFlags.NonPublic | BindingFlags.Instance);
		field.SetValue(qcar, CameraClearFlags.Nothing);
		
		SwitchMode(true);		
	}
	
	IEnumerator InitBackgrounds()
	{
	    while (!QCARManager.Instance.Initialized) {
			yield return new WaitForEndOfFrame();
		}

		//var info = QCARRenderer.Instance.GetVideoTextureInfo();
		//var conf = QCARRenderer.Instance.GetVideoBackgroundConfig();

		//Debug.Log("Inco: imageSize = [" + info.imageSize.x + "," + info.imageSize.y + "], textureSize = [" + info.textureSize.x + "," + info.textureSize.y + "]" );
		//Debug.Log("Inco: conf.position = [" + conf.position.x + "," + conf.position.y + "], conf.size = [" + conf.size.x + "," + conf.size.y + "]" );
		//Debug.Log("Inco: " + CameraDevice.Instance.GetVideoMode().width + "," + CameraDevice.Instance.GetVideoMode().height);

		// calculate uv
		Rect uv;
		var info = CameraDevice.Instance.GetVideoMode();
		var ts = new Vector2( info.width < 512 ? 512 : (info.width < 1024 ? 1024 : 2048),
		                     info.height < 512 ? 512 : (info.height < 1024 ? 1024 : 2048));
		Debug.Log("Inco: Screen[" + Screen.width + ", "  + Screen.height + "], Camera[" + info.width + ", " + info.height + "], Texture=" + ts);

		var fw = info.height*Screen.width/Screen.height;
		if (fw < info.width) 
		{
			uv = new Rect( (info.width - fw) / (2f * ts.x), 0, fw/ts.x, info.height/ts.y);
			Debug.Log("Inco: #1 Rect = " + uv);
		} else {
			var fh = info.width * Screen.height / Screen.width;
			uv = new Rect( 0, (info.height - fh) / (2f*ts.y), info.width/ts.x, fh/ts.y);			
			Debug.Log("Inco: #2 Rect = " + uv);
		}

		QCARRenderer.Instance.DrawVideoBackground = false;
		
		_backgroundTexture = new Texture2D(0, 0, TextureFormat.RGB565, false);		
		_backgroundTexture.filterMode = FilterMode.Bilinear;
		_backgroundTexture.wrapMode = TextureWrapMode.Clamp;
		
		QCARRenderer.Instance.SetVideoBackgroundTexture(_backgroundTexture);
		BgMaterial.mainTexture = _backgroundTexture;

		var arCamera = this.GetComponent<Camera>();

		var leftMask = (uint)(1 << 27);
		var rightMask = (uint)(1 << 28);
		arCamera.cullingMask = (int)((uint)(arCamera.cullingMask) 
		                             & (~leftMask)
		                             & (~rightMask));

		_leftCamera = CreateCamera("LeftCamera", arCamera, new Rect(0,0, 0.5f, 1));
		_leftCamera.cullingMask = (int)((uint)(arCamera.cullingMask) | leftMask );
		_leftCamera.gameObject.layer = 27;
		_leftBackground = CreateCameraBackground("LeftBackground", _leftCamera, BgMaterial, new Rect(0,0, Screen.width/2, Screen.height), uv);
		_leftBackground.layer = 27;

		
		_rightCamera = CreateCamera("RightCamera", arCamera, new Rect(0.5f,0, 0.5f, 1));
		_rightCamera.cullingMask = (int)((uint)(arCamera.cullingMask) | rightMask );
		_rightBackground = CreateCameraBackground("RightBackground", _rightCamera, BgMaterial, new Rect(Screen.width/2, 0, Screen.width/2, Screen.height), uv);
		_rightBackground.layer = 28;

		SetPosition(this.gameObject, _leftCamera.gameObject, new Vector3(-1, 0, 0));
		SetPosition(this.gameObject, _rightCamera.gameObject, new Vector3(1, 0, 0));

		arCamera.cullingMask = 0;

		var qcar = this.GetComponent<QCARAbstractBehaviour>();
		var field = typeof(QCARAbstractBehaviour).GetField("mCachedCameraClearFlags", BindingFlags.NonPublic | BindingFlags.Instance);
		field.SetValue(qcar, CameraClearFlags.Nothing);

		SwitchMode(true);		
	}
 
	GameObject CreateCameraBackground(string name, Camera camera, Material bgMaterial, Rect screenRect, Rect screenUV)
	{
		var viewportRect = new Rect(0,0,1f, 1);
		var distance = 100;//camera.farClipPlane*1.15f;

		var bg = new GameObject(name);
		bg.transform.position = Vector3.zero;
		bg.transform.rotation = Quaternion.Euler(0,0,0);
		bg.transform.localScale = Vector3.one;

		var mesh = bg.AddComponent<MeshFilter>().mesh;
		var renderer = bg.AddComponent<MeshRenderer>();		
		mesh.Clear();
		
		var vertices = new Vector3[4];
		var triangles = new int[2 * 3] { 0, 3, 2, 0, 2, 1};
		
		vertices[0] = camera.ViewportPointToRay(new Vector2(viewportRect.x, viewportRect.y)).GetPoint(distance);
		vertices[1] = camera.ViewportPointToRay(new Vector2(viewportRect.x + viewportRect.width, viewportRect.y)).GetPoint(distance);
		vertices[2] = camera.ViewportPointToRay(new Vector2(viewportRect.x + viewportRect.width, viewportRect.y + viewportRect.height)).GetPoint(distance);
		vertices[3] = camera.ViewportPointToRay(new Vector2(viewportRect.x, viewportRect.y + viewportRect.height)).GetPoint(distance);

		var uv = new Vector2[4];
		uv[2] = new Vector2(screenUV.xMax, screenUV.yMin);
		uv[3] = new Vector2(screenUV.xMin, screenUV.yMin);
		uv[0] = new Vector2(screenUV.xMin, screenUV.yMax);
		uv[1] = new Vector2(screenUV.xMax, screenUV.yMax);

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		renderer.material = bgMaterial;

		// attach
		bg.transform.parent = camera.gameObject.transform;
		bg.transform.localPosition = Vector3.zero;
		bg.transform.localRotation = Quaternion.Euler(0,0,0);
		bg.transform.localScale = Vector3.one;

		return bg;
	}

	void SetPosition(GameObject parent, GameObject child, Vector3 offset)
	{
		child.transform.parent = parent.transform;
		child.transform.localPosition = Vector3.zero + offset;
		child.transform.localRotation = Quaternion.Euler(0,0,0);
		child.transform.localScale = Vector3.one;
	}

	Camera CreateCamera(string name, Camera anchor, Rect viewRect)
	{
		var cameraObject = new GameObject(name);
		var camera = cameraObject.AddComponent<Camera>();
		camera.nearClipPlane = anchor.nearClipPlane;
		camera.farClipPlane = anchor.farClipPlane;

		cameraObject.transform.position = Vector3.zero;
		cameraObject.transform.rotation = Quaternion.Euler(0,0,0);
		cameraObject.transform.localScale = Vector3.one;

		camera.rect = viewRect;
		camera.backgroundColor = new Color(0,0,0);
		camera.depth = anchor.depth - 1;

		return camera;			
	}

	void SwitchMode(bool is3D)
	{
		string _error = "";
		try {
			using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				_error = "UnityPlayer getted " + (unityPlayer == null) + "\n";
				using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					_error += "currentActivity getted " + (activity == null) + "\n";
					using (AndroidJavaObject app = activity.Call<AndroidJavaObject>("getApplicationContext")) 
					{
						_error += "getApplicationContext called " + (app == null) + "\n";
						using (AndroidJavaObject displayControl = new AndroidJavaObject("jp.epson.moverio.bt200.DisplayControl", new object[] { app }))
						{
							_error += "DisplayControl created " + (displayControl == null) + "\n";	
							int mode = is3D ? 1 : 0;
							bool toast = is3D;
							displayControl.Call<int>("setMode", new object[] { mode, toast });
							
							_error += "Done";
						}					
					}

					using (AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow")) {
						using (AndroidJavaObject view = window.Call<AndroidJavaObject>("getDecorView")) {
							uint t = 0x00000002 | 0x00000004;
							view.Call("setSystemUiVisibility", new object[] { t });
						}
					}
				}
			}

			Debug.Log("Switching mode:" + _error);
		}
		catch (System.Exception ex) {
			Debug.LogError("Switching mode: (ERROR)" + ex.Message);
			
		}

		
	}

	void OnDestroy()
	{
		if (MySettings.IsStereo) {
			SwitchMode(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
