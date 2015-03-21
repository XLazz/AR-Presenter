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

public class Foo : MonoBehaviour {

	protected MarkerDetailsView _detailsView;

	// Use this for initialization
	void Start () {

		var k = ResizeHelper.k;

		_detailsView = this.GetComponent<MarkerDetailsView>();

		StartCoroutine ( Starter() );
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if ((_detailsView != null) && (_detailsView.enabled)) {
				_detailsView.Close();
			} else {
				Application.LoadLevel("MenuScene");
			}
		}
	}

	IEnumerator Starter()
	{
		if (GameObject.Find("ObjLoaderHolder") == null) {
			var g = GameObject.Instantiate( Resources.Load("ObjLoaderHolder") );
			g.name = "ObjLoaderHolder";
		}

		////////////////////////////////////////////////////////////////////////////////
		// Load data set
		////////////////////////////////////////////////////////////////////////////////
		int count = 0;
		while (!QCARRuntimeUtilities.IsQCAREnabled())
		{
			yield return new WaitForSeconds(0.5f);
			count++;
			if (count > 10)
			{
				Debug.LogError("Timeout exception of waying QCARRuntimeUtilities.IsQCAREnabled");
				yield break;
			}
		}
		Debug.Log("DataSetLoadBehaviour: IsQCAREnabled == true");
		var path = Configuration.ConfigMaganer.Instance.Application.XmlPath;
		if (!path.StartsWith("/")) {
			path = System.IO.Path.Combine(Application.persistentDataPath, path);
		}

		if (QCARRuntimeUtilities.IsPlayMode())
		{
			// initialize QCAR 
			QCARUnity.CheckInitializationError();
		}
		
		if (TrackerManager.Instance.GetTracker<ImageTracker>() == null)
		{
			TrackerManager.Instance.InitTracker<ImageTracker>();
		}
		
		if (!DataSet.Exists(path, DataSet.StorageType.STORAGE_ABSOLUTE))
		{
			Debug.LogError("Data set " + path + " does not exist.");
			Application.Quit();
		}
		
		ImageTracker imageTracker = (ImageTracker) TrackerManager.Instance.GetTracker<ImageTracker>();
		DataSet dataSet = imageTracker.CreateDataSet();
		
		if (!dataSet.Load(path, DataSet.StorageType.STORAGE_ABSOLUTE))
		{
			Debug.LogError("Failed to load data set " + path + ".");
			Application.Quit();
		}
		imageTracker.ActivateDataSet(dataSet);
		Debug.Log("Dataset activated: " + path);
		////////////////////////////////////////////////////////////////////////////////
		


		while (!QCARManager.Instance.Initialized) {
			yield return new WaitForEndOfFrame();
		}
		////////////////////////////////////////////////////////////////////////////////
		QCARRenderer.Instance.DrawVideoBackground = Configuration.ConfigMaganer.Instance.Application.Debug;
		////////////////////////////////////////////////////////////////////////////////
		
		if (CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_INFINITY))	{
			Debug.Log("Camera focus mode setted to FOCUS_MODE_INFINITY");
		} else {
			Debug.Log("Camera can't focus to FOCUS_MODE_INFINITY");
		}

		var markersLister = Camera.main.GetComponent<MarkersLister>();

		DataSetTrackableBehaviour[] trackableBehaviours = (DataSetTrackableBehaviour[])	GameObject.FindObjectsOfType(typeof(DataSetTrackableBehaviour));
		var selectionObjectOriginal = (GameObject) Resources.Load("SelectionObject");
		foreach (DataSetTrackableBehaviour trackableBehaviour in trackableBehaviours)
		{
			IEditorDataSetTrackableBehaviour editorTrackableBehaviour = trackableBehaviour;		
			
			if (editorTrackableBehaviour.gameObject.transform.childCount == 0)
			{
				editorTrackableBehaviour.gameObject.name = "Marker[" + editorTrackableBehaviour.TrackableName + "]";
				editorTrackableBehaviour.gameObject.AddComponent<TurnOffBehaviour>();
				
				var markerName = editorTrackableBehaviour.TrackableName;				
				var trackableEvent = editorTrackableBehaviour.gameObject.AddComponent<TrackableEventHandler>();

				if (Configuration.ConfigMaganer.Instance.Application.Debug)
				{
					var obj = (GameObject) GameObject.Instantiate( selectionObjectOriginal );
					obj.transform.parent = editorTrackableBehaviour.gameObject.transform;
					obj.transform.localPosition = Vector3.zero;
					obj.transform.localScale = Vector3.one;
				}
				
				markersLister.AddMarker(markerName, trackableEvent);
					
			}
		}	
	}

	void DebugRoot()
	{
		Debug.Log("----- Debug root -----");
		foreach (GameObject go in Object.FindObjectsOfType(typeof(GameObject)))
		{
			if (go.transform.parent == null)
			{
				DebugDeeper("\\", go.transform);
			}
		}
	}
	
	void DebugDeeper(string prefix, Transform trans)
	{
		int children = trans.GetChildCount();
		Debug.Log(prefix + trans.name);

		var componets = trans.gameObject.GetComponents(typeof(MonoBehaviour));
		foreach (var c in componets)
		{
			Debug.Log("\t" + c.GetType().FullName);
		}
		
		for (int child=0;child<children;child++)
		{
			DebugDeeper(prefix + trans.name + "\\",  trans.GetChild(child) );
		}
	}
}
