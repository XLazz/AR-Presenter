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

public class SplashScreenDrawer : MonoBehaviour {
	
	public Texture2D Spinner;
	protected Texture2D PortraitTexture;
	
    public float SecondsVisible = 2.0f;
	
	// Use this for initialization
	IEnumerator Start () {
	  /*if ((Application.platform == RuntimePlatform.Android) && (int.Parse(Application.unityVersion.Substring(0, 1)) >= 4))
            SecondsVisible += 3.5f;*/
	
		/*var f1 = (Screen.height*800f/Screen.width)/1280f;
		var f2 = (Screen.height*1080f/Screen.width)/1920f;
		PortraitTexture = f1 > f2 
			? (Texture2D) Resources.Load("SplashScreen/splashscreen800x1280")  
				: (Texture2D) Resources.Load("SplashScreen/splashscreen1080x1920");
		*/
		Invoke("LoadMainScene", SecondsVisible);

		/*string _error = "";
		try {
			using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				_error = "UnityPlayer getted " + (unityPlayer == null) + "\n";
				using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					_error += "currentActivity getted " + (activity == null) + "\n";
					using (AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow")) {
						_error += "window getted " + (window == null) + "\n";
						using (AndroidJavaObject view = window.Call<AndroidJavaObject>("getDecorView")) {
							_error += "view getted " + (window == null) + "\n";
							uint t = 0x00000002 | 0x00000004;
							view.Call("setSystemUiVisibility", new object[] { t });
						}
					}
				}
			}
			
			Debug.Log("Full screen mode:" + _error);
		}
		catch (System.Exception ex) {
			Debug.LogError("Full screen mode: (ERROR)" + ex.Message);
			
		}*/
		yield return new WaitForEndOfFrame();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
    {
		//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), PortraitTexture);
        
        Matrix4x4 oldMatrix = GUI.matrix;
        float thisAngle = Time.frameCount*4;

        Rect thisRect = new Rect(Screen.width/2.0f - Spinner.width/2f, Screen.height/2.0f - Spinner.height/2f,
                                 Spinner.width, Spinner.height);

        GUIUtility.RotateAroundPivot(thisAngle, thisRect.center);
        GUI.DrawTexture(thisRect, Spinner);
        GUI.matrix = oldMatrix;
    }

    /// <summary>
    /// Loads the ImageTargets scene.
    /// </summary>
    private void LoadMainScene()
    {
		Configuration.ConfigMaganer.Init( MySettings.ConfigFile, true );

		if (MySettings.IsStartWithMenu) {
			Application.LoadLevelAsync("MenuScene");
		} else {
			Application.LoadLevelAsync("EpsonARScene");
		}
    }


}
