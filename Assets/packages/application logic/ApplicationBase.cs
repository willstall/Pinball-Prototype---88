using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ApplicationLogic
{
	public class ApplicationBase : MonoBehaviour
	{
		public ApplicationState initialState = ApplicationState.MAIN_MENU;
		public KeyCode showGUIKey = KeyCode.BackQuote;
		public bool debugStateChanges = false;
		public bool showGUI = false;

		bool _isPaused = false;
		

		public ApplicationState currentState{ get; private set; }
		public ApplicationState nextState{ get; private set; }
		

		public void Start()
		{
			ChangeState( initialState );
		}

		public void Update()
		{
			if(Input.GetKeyDown( showGUIKey ))
				showGUI = (showGUI == true)?(false):(true);
		}

		public void PauseCurrentState()
		{
			if(isPaused)
				return;

			_isPaused = true;
			SendMessage( "OnPauseState", currentState );	
		}

		public void ResumeCurrentState()
		{
			if(!isPaused)
				return;

			_isPaused = false;
			SendMessage( "OnResumeState", currentState );	
		}

		public void ChangeState( ApplicationState newState )
		{
			if( IsCurrentState(newState) )
				return;

			nextState = newState;

			SendMessage( "OnTeardownState", currentState );	

			currentState = newState;

			SendMessage( "OnSetupState", currentState );	
		}

		public bool isPaused
		{
			get
			{
				return _isPaused;
			}			
		}

		public bool IsCurrentState( ApplicationState state )
		{
			return currentState == state;
		}

	}
}

/*
		INITIALIZE,
		LEVEL_START,
		LEVEL_PLAY,
		LEVEL_EDIT,
		LEVEL_UP,
		LEVEL_COMPLETE,
		LEVEL_REPRIEVE,
		PAUSE

    INITIALIZE,
    MENU,
    START,
    LEVELUP,
    PENDINGBURGER,
    EATING,
    NEWINGREDIENT,
    PLAY,
    COMPLAINING,
    OVER,
    RESTART,
    WIN,
    NULL
    */