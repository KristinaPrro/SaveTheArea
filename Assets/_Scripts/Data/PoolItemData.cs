using System;
using UnityEngine;

[Serializable]
public struct PoolItemData
{
	[SerializeField, HideInInspector]
	private string _name;
	[field: SerializeField]
	public PoolItemType PoolItemType { get; private set; }
	[field: SerializeField]
	public int Count { get; private set; }
	[field: SerializeField]
	public GameObject ItemGameObject { get; private set; }

	public void SetName()
	{
		_name = PoolItemType.ToString();
	}
}