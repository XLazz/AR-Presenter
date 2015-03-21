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

public class FullScreener : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Dim();	
	}

	float _time;
	// Update is called once per frame
	void Update () {
		/*if (Time.time - _time > 1){
			_time = Time.time;
			DimRunable();
		}*/
	}

	void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			Dim();
		}
	}
	
	private void Dim()
	{
		///var buildVersion = new AndroidJavaClass("android.os.Build.VERSION");
		//if (buildVersion.GetStatic<int>("SDK_INT") >= 11)
		{
			using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					activity.Call("runOnUiThread", new AndroidJavaRunnable(DimRunable2));
				}
			}
		}
		//DimRunable();
	}

	public static void DimRunable()
	{
		try
		{
			using (var helper = new AndroidJavaClass("com.vieinco.epsonarhelper.ScreenerHelper"))
			{
				Debug.Log("Inco: helper - " + (helper != null));
				helper.CallStatic("Init", new object[] { true } );
				Debug.Log("Inco: helper.method has been called");				
			}
		}	
		catch (System.Exception ex)
		{
			Debug.LogError("Inco: ============== ");
			Debug.LogError(ex.Message);
			
		}
	}
	
	public static void DimRunable2()
	{
		try
		{
			using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
						//Debug.Log ( " requestWindowFeature returned " + activity.Call<bool>("requestWindowFeature", new object[] {0x00000001} ));

					using (var window = activity.Call<AndroidJavaObject>("getWindow"))
					{
						using (var view = window.Call<AndroidJavaObject>("getDecorView"))
						{
								view.Call("setSystemUiVisibility", 0x00000002 | 0x00000004);
						}
					}
				}
			}
		} catch (System.Exception ex)
		{
			Debug.LogError(" ============== ");
			Debug.LogError(ex.Message);
			
		}
	}
}
