using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Schedule {


	public Schedule(string name_){
		name = name_;
	}

	public string name;

	public string Status {
		get {
			return name + " (" + (taskIndex + 1) +" of " + tasks.Count + ")";
		}
	}

	public delegate bool Task(BaseZombie actor);
	public float[] taskTimeouts;

	public List<Task> tasks = new List<Task>();
	private Task currentTask;

	public EnemyConditions interruptors;

	public float lastTaskStarTime = -1;

	int taskIndex = 0;
	bool isCompleted;

	public bool IsCompleted {
		get {
			return isCompleted;
		}
	}

	public bool IsInterrupted (EnemyConditions condition)
	{
		return (condition & interruptors) != 0;
	}

	bool TimedOut ()
	{
		if (taskTimeouts == null || taskTimeouts.Length == 0)
		{		
			return false;
		}
		else {
			return Time.time > lastTaskStarTime + taskTimeouts [taskIndex];
		}
	}

	// Update is called once per frame
	public void ManualUpdate (BaseZombie actor) {
		if (isCompleted)
			return;

		if (currentTask == null) {
			currentTask = tasks[taskIndex];
			lastTaskStarTime = Time.time;
		}

		if (actor.debug && TimedOut ())
						Debug.Log ("Time out: " + Status + " : " + (Time.time - lastTaskStarTime));

		if (currentTask(actor) || TimedOut()){
			currentTask = null;
			taskIndex++;
			if (taskIndex >= tasks.Count){
				isCompleted = true;
				if (actor.debug) Debug.Log("Complete:" + Status);
			}
		}

	}


	public void Reset(){
		taskIndex = 0;
		isCompleted = false;
		lastTaskStarTime = -1;
		currentTask = null;
	}



	void Complete ()
	{
		throw new NotImplementedException ();
	}
}
