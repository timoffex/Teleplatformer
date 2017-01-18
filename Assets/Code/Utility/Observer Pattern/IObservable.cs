using System;

namespace ObserverPattern {
	public interface IObservable<T> {
		IDisposable Subscribe (IObserver<T> observer);
	}
}

