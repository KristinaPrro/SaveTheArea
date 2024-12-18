using ModestTree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiViewGameStatusScreen: UiView
{
	[field: SerializeField]
	public TextMeshProUGUI TextPlayerHealth { get; private set; }
	[field: SerializeField]
	public TextMeshProUGUI TextEnemyCount { get; private set; }
	[field: SerializeField]
	public Button ButtonExit { get; private set; }

	public void Awake()
	{
		Assert.IsNotNull(TextPlayerHealth);
		Assert.IsNotNull(TextEnemyCount);
		Assert.IsNotNull(ButtonExit);
	}
}