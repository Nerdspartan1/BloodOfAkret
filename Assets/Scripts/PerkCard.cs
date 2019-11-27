using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PerkCard : MonoBehaviour
{
	[Header("References")]
	public Text Name;
	public Text Rarity;
	public Image CardBackground;
	public Image Icon;
	public Text Description;

	public Color[] RarityColors;

	public Perk Perk;
	private Button _button;


	public void Start()
	{
		if (Perk) SetPerk(Perk);
	}

	public void SetPerk(Perk perk)
	{
		Perk = perk;
		Name.text = Perk.Name;
		switch (Perk.Rarity)
		{
			case PerkRarity.Common:
				Rarity.text = "Common";
				CardBackground.color = RarityColors[0];
				break;
			case PerkRarity.Uncommon:
				Rarity.text = "Uncommon";
				CardBackground.color = RarityColors[1];
				break;
			case PerkRarity.Rare:
				Rarity.text = "Rare";
				CardBackground.color = RarityColors[2];
				break;
			case PerkRarity.Legendary:
				Rarity.text = "Legendary";
				CardBackground.color = RarityColors[3];
				break;
		}
		
		Icon.sprite = Perk.Icon;
		Description.text = Perk.Description;
		_button = GetComponent<Button>();

		_button.onClick.RemoveAllListeners();
		_button.onClick.AddListener(PurchasePerk);

	}

	public void PurchasePerk()
	{
		GameManager.Instance.Player.GetComponent<Player>().PerkUp(Perk);

		WaveManager.Instance.CloseShop();
	}
}
