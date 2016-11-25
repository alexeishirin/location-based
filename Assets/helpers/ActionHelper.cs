using System.Collections.Generic;
using System.Collections;
using System;

public class ActionHelper<T> {
	public Action<T> createSequence(params Action<T>[] actions) {
		return (t) => {
			foreach (Action<T> action in actions) {
				action(t);
			}
		};
	}
}