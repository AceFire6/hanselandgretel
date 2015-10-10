using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour {

	public GameObject Boss;

	private Slider healthBar;
	private Health bossHealth;
	private Text bossHealthText;

	private bool firstUpdate;
	private bool dead;

	// Use this for initialization
	void Start () {
		healthBar = GetComponent<Slider>();
		bossHealth = Boss.GetComponent<Health>();
		bossHealthText = GetComponentInChildren<Text>();
		healthBar.maxValue = bossHealth.maxHealth;
		firstUpdate = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsDead()) {
			GameObject.Find ("BossNameplate").SetActive(false);
		} else {
			if (!firstUpdate) {
				UpdateText();
			}
			healthBar.value = bossHealth.totalHealth;
		}
	}

	public void UpdateText() {
		bossHealthText.text = Mathf.Max(bossHealth.totalHealth, 0) + "/" + bossHealth.maxHealth;
	}

	public bool IsDead() {
		return bossHealth.totalHealth <= 0;
	}
}
