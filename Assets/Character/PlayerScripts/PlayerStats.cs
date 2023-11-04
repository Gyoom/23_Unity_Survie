using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class PlayerStats : MonoBehaviour
{
    [Header("COMPONENTS REFERENCES")]

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private MoveBehaviour playerMovementScript; 

    [Header("Health")]

    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private float healthDecreaseRateForHungerAndThirst;

    [SerializeField]
    private Image healthBarFill;

    [HideInInspector]
    public float currentHealth;

    [Header("Hunger")]
    [SerializeField]
    private float maxHunger = 100f;

    [SerializeField]
    private float hungerDecreaseRate;

    [SerializeField]
    private Image hungerBarFill;

    [HideInInspector]
    public float currentHunger;

    [Header("Thirst")]
    [SerializeField]
    private float maxThirst = 100f;

    [SerializeField]
    private float thirstDecreaseRate;

    [SerializeField]
    private Image thirstBarFill;

    [HideInInspector]
    public float currentThirst;

    [HideInInspector]
    public bool isDead = false;

    public bool cheatStats = false;

    public float currentArmorPoint;

    void Awake ()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
    }

    void Update() 
    {
        updateHungerAndThirstBarFill();

        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     TakeDamage(50f);
        // }
    }

    public void TakeDamage(float damage, bool overTime = false) {
        if (cheatStats)
            return;

        if (overTime)
        {
            currentHealth -= damage * Time.deltaTime;
        } 
        else
        {
            currentHealth -= damage * (1 - (currentArmorPoint / 100));
        } 

        if (currentHealth <= 0 && !isDead)
        {
           Die();
        }
        updateHealthBarFill();
    }

    private void Die() 
    {
        isDead = true;
        playerMovementScript.canMove = false;


        hungerDecreaseRate = 0;
        thirstDecreaseRate = 0;
        
        playerAnimator.SetTrigger("Death");

        Debug.Log("Vous êtes mort !");
    }

    public void updateHealthBarFill() 
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    void updateHungerAndThirstBarFill()
    {
        if (cheatStats)
            return;
        // Diminue faim /soif au fil du temps
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentThirst -= thirstDecreaseRate * Time.deltaTime;
        
        // Empeche les valeurs de passer dans le négatif
        currentHunger = currentHunger < 0 ? currentHunger = 0 : currentHunger;
        currentThirst = currentThirst < 0 ? currentThirst = 0 : currentThirst;

        // Met à jour les visuels
        hungerBarFill.fillAmount = currentHunger / maxHunger;
        thirstBarFill.fillAmount = currentThirst / maxThirst;

        if ( currentHunger == 0  || currentThirst == 0)
        {   
            TakeDamage(
                currentHunger == 0 && currentThirst == 0 ? 
                healthDecreaseRateForHungerAndThirst * 2 : 
                healthDecreaseRateForHungerAndThirst
                , true);
        }
    }

    public void consumeItem (ConsumableEffect[] consumableEffects) {
        foreach(ConsumableEffect ConsumableEffect in consumableEffects)
        {
            switch (ConsumableEffect.consumableTarget)
            {
                case ConsumableTarget.health:
                    currentHealth += ConsumableEffect.consumableValue;
                    if (currentHealth > maxHealth) currentHealth = maxHealth;
                    updateHealthBarFill();
                    break;
                case ConsumableTarget.hunger:
                    currentHunger += ConsumableEffect.consumableValue;
                    if (currentHunger > maxHealth) currentHunger = maxThirst;
                    break;
                case ConsumableTarget.thirst:
                    currentThirst += ConsumableEffect.consumableValue;
                    if (currentThirst > maxHealth) currentThirst = maxThirst;
                    break;
            }
        }
    }
}
