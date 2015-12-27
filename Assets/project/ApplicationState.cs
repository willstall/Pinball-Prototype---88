using UnityEngine;
using System.Collections;

/*

	WE HIGHLY RECOMMEND PLACING THIS FILE INSIDE YOUR PROJECT.

*/
	
namespace ApplicationLogic
{
	public enum ApplicationState
	{
		NULL = 0,
		MAIN_MENU = 1,
		PLAY = 2,
		REPRIEVE = 3,
		EDIT = 4,
		QUIT = 5,
		ACT_INTRO = 6,
		ACT_COMPLETE = 7,
		PLAYLIST_COMPLETE = 8
	}
}