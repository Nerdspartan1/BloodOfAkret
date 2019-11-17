using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shop : MonoBehaviour
{
	[Header("Prefabs")]
	public GameObject PerkCardPrefab;

	[Header("References")]
	public Transform ArrayPanel;

	public List<Perk> UnlockedPerks;
	public int MaxPerksForSale;

	public void PresentPerks()
	{
		List<Perk> randomPerks = new List<Perk>(UnlockedPerks);
		randomPerks.RandomizeList();

		int nbPerksForSale = Mathf.Min(MaxPerksForSale, randomPerks.Count);

		foreach(Transform card in ArrayPanel)
		{
			Destroy(card.gameObject);
		}

		for (int i = 0; i < nbPerksForSale; ++i)
		{
			PerkCard card = Instantiate(PerkCardPrefab, ArrayPanel).GetComponent<PerkCard>();
			card.SetPerk(randomPerks[i]);
		}
	}
}
