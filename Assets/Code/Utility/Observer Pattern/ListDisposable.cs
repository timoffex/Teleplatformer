using System;
using System.Collections.Generic;

public class ListDisposable<T> : IDisposable {
	private List<T> theList;
	private T obj;

	public ListDisposable (List<T> list, T obj) {
		theList = list;
		this.obj = obj;
	}

	public void Dispose () {
		theList.Remove (obj);
	}
}