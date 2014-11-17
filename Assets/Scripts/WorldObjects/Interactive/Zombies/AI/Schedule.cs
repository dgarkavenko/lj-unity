using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Schedule {

	public delegate bool Task();

	List<Task> tasks;

	public Dummy.Conditions interruptors;

	int taskIndex = 0;
	bool isCompleted;

	public bool IsCompleted {
		get {
			return isCompleted;
		}
	}

	public bool IsInterrupted (Dummy.Conditions condition)
	{
		return (int)(condition & interruptors) != 0;
	}

	// Update is called once per frame
	public void ManualUpdate () {
		if (isCompleted)
			return;

		var task = tasks [taskIndex];
		if (task != null){
			if (task()){
				taskIndex++;
				if (taskIndex >= tasks.Count){
					Reset();
				}
			}
		}
	}



	void Reset(){
		taskIndex = 0;
		isCompleted = false;
	}

	void Complete ()
	{
		throw new NotImplementedException ();
	}
}
