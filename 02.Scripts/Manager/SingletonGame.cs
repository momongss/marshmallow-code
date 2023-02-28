using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현재 사용 중인 Singleton 에는 문제가 있음.
// Awake 시 instance 가 있는지 확인하고, 만약 있다면,
// 

public class SingletonGame<T> : MonoBehaviour where T : Component
{
	#region Fields

	/// <summary>
	/// The instance.
	/// </summary>
	private static T instance;

	#endregion

	#region Properties

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static T I
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();
				if (instance == null)
				{
					GameObject obj = new GameObject();
					obj.name = typeof(T).Name;
					instance = obj.AddComponent<T>();
				}
			}
			return instance;
		}
	}

	#endregion

	#region Methods

	/// <summary>
	/// Use this for initialization.
	/// </summary>
	protected virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	#endregion
}
