using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour {

    public Card card;
    public string type;
    public int power;

    public SpriteRenderer spriteRenderer;

	void Start () {
        type = card.type;
        power = card.power;
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = card.artWork;
	}
}
