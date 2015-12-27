using UnityEngine;
using System.Collections;

namespace ApplicationLogic
{
	[RequireComponent( typeof(ApplicationBase) )]
	public abstract class ApplicationStateLoop : MonoBehaviour {

		public ApplicationState state;

		ApplicationBase _application;

		// methods executed from base
		void OnSetupState( ApplicationState state )
		{
			if( enabled && this.state == state )
				Setup();			
		}

		void OnTeardownState( ApplicationState state )
		{
			if( enabled && this.state == state )
				Teardown();
		}

		void OnPauseState( ApplicationState state )
		{
			if( enabled && this.state == state )
				Pause();
		}

		void OnResumeState( ApplicationState state )
		{
			if( enabled && this.state == state )
				Resume();
		}

		// Set up
		void Setup() {
			if( application.debugStateChanges )
				Debug.Log( "Setup " + this ); 

			OnSetup();
		}

		void Update()
		{
			if( isCurrentState )
				OnUpdate();
		}

		// Teardown
		void Teardown()
		{
			if( application.debugStateChanges )
				Debug.Log( "Teardown " + this ); 

			OnTeardown();
		}

		void Pause()
		{

			if( application.debugStateChanges )
				Debug.Log( "Pause " + this ); 
			OnPause();
		}

		void Resume()
		{
			if( application.debugStateChanges )
				Debug.Log( "Resume " + this );
			OnResume(); 	
		}

		public abstract void OnSetup();		
		public abstract void OnTeardown();


		public virtual void OnPause(){}
		public virtual void OnResume(){}
		public virtual void OnUpdate(){}

		protected ApplicationBase application
		{
			get
			{
				if( _application == null )
					_application = GetComponent<ApplicationBase>();

				return _application;
			}

		}

		protected bool isPaused
		{
			get
			{
				return application.isPaused;
			}
		}

		protected bool showGUI
		{
			get
			{
				if( (isPaused) || (!isCurrentState) )
					return false;

				return application.showGUI;
			}
			set
			{
				application.showGUI = value;
			}
		}

		protected bool isCurrentState
		{
			get
			{
				return application.IsCurrentState( this.state );
			}
		}
	}
}