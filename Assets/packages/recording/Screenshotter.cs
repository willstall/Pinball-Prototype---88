using UnityEngine;
using System.Collections;
using System.IO;

namespace Recording
{


	public class Screenshotter : MonoBehaviour {

		public bool useMinimumResolution;
		public Vector2 minimumResolution; 

		public void Update()
		{
			if( Input.GetKeyDown( KeyCode.F12 ) && ( Input.GetKey( KeyCode.LeftCommand ) || Input.GetKey(KeyCode.RightCommand) ))
			{
				Screenshot();
			}
		}
		
		
		public void Screenshot()
		{
			string filename = ""; 
			if(!Application.isEditor)
			{
				filename = Application.persistentDataPath + "/" + filename;
			} else {
				Directory.CreateDirectory( Application.dataPath + "/../Screenshots/" ); 
				filename = "Screenshots/" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".png";
			}
			
			int ratio = 4;

			if( useMinimumResolution )
			{
				float horizRatio = minimumResolution.x / Screen.width;
				float vertRatio = minimumResolution.y / Screen.height;

				ratio = Mathf.CeilToInt( Mathf.Max( horizRatio, vertRatio ) );
			}


			Application.CaptureScreenshot(filename, ratio );
		}
	}


}