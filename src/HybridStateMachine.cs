﻿using UnityEngine;
using System;

namespace FSM
{
	/// <summary>
	/// A State-like StateMachine that allows you to run additional functions (companion code)
	/// with the sub-states.
	/// It is especially handy for hierarchical state machines, as it allows you to factor out
	/// common code from the sub states into the HybridStateMachines, essentially removing
	/// duplicate code.
	/// The HybridStateMachine can also be seen as a StateWrapper around a normal StateMachine.
	/// </summary>
	public class HybridStateMachine<TEvent> : StateMachine<TEvent>
	{
		private Action<HybridStateMachine<TEvent>> onEnter;
		private Action<HybridStateMachine<TEvent>> onLogic;
		private Action<HybridStateMachine<TEvent>> onExit;

		public Timer timer;

		/// <summary>
		/// Initialises a new instance of the HybridStateMachine class
		/// </summary>
		/// <param name="mono">The MonoBehaviour of the script that created the state machine</param>
		/// <param name="onEnter">A function that is called after running the sub-state's OnEnter method
		/// when this state machine is entered</param>
		/// <param name="onLogic">A function that is called after running the sub-state's OnLogic method
		/// if this state machine is the active state</param>
		/// <param name="onExit">A function that is called after running the sub-state's OnExit method
		/// when this state machine is left</param>
		/// <param name="needsExitTime">(Only for hierarchical states):
		/// 	Determins whether the state machine as a state of a parent state machine is allowed to instantly
		/// 	exit on a transition (false), or if it should wait until the active state is ready for a
		/// 	state change (true).</param>
		public HybridStateMachine(
				MonoBehaviour mono,
				Action<HybridStateMachine<TEvent>> onEnter = null,
				Action<HybridStateMachine<TEvent>> onLogic = null,
				Action<HybridStateMachine<TEvent>> onExit = null,
				bool needsExitTime = false) : base(mono, needsExitTime)
		{
			this.onEnter = onEnter;
			this.onLogic = onLogic;
			this.onExit = onExit;

			this.timer = new Timer();
		}

		public override void OnEnter()
		{
			base.OnEnter();

			timer.Reset();
			onEnter?.Invoke(this);
		}

		public override void OnLogic()
		{
			base.OnLogic();

			onLogic?.Invoke(this);
		}

		public override void OnExit()
		{
			base.OnExit();

			onExit?.Invoke(this);
		}
	}

	public class HybridStateMachine : HybridStateMachine<string>
	{
		public HybridStateMachine(
			MonoBehaviour mono,
			Action<HybridStateMachine<string>> onEnter = null,
			Action<HybridStateMachine<string>> onLogic = null,
			Action<HybridStateMachine<string>> onExit = null,
			bool needsExitTime = false) : base(mono, onEnter, onLogic, onExit, needsExitTime)
		{
		}
	}
}
