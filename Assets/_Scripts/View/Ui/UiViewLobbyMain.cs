using ModestTree;
using UnityEngine;
using UnityEngine.UI;

public class UiViewLobbyMain : UiView
{
	[field: SerializeField]
	public Button ButtonStart { get; private set; }
	[field: SerializeField]
	public Button ButtonSettings { get; private set; }
	[field: SerializeField]
	public Button ButtonCustomize { get; private set; }
	[field: SerializeField]
	public Button ButtonThanks { get; private set; }
	[field: SerializeField]
	public Button ButtonExit { get; private set; }
	
	public void Awake()
	{
		Assert.IsNotNull(ButtonStart);
		Assert.IsNotNull(ButtonSettings);
		Assert.IsNotNull(ButtonCustomize);
		Assert.IsNotNull(ButtonThanks);
		Assert.IsNotNull(ButtonExit);
	}
}