using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [Header("HealthSystem Configuration")]
    [SerializeField] int health;
    [SerializeField] int maxHealth;

    [Header("Feedback Configuration")]
    [SerializeField] Material damagedMat;
    [SerializeField] MeshRenderer enemyRend;
    [SerializeField] GameObject deathVFX;
    Material baseMat;   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        health = maxHealth;
        baseMat = enemyRend.material;

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            health = 0;
            deathVFX.SetActive(true);
            deathVFX.transform.position = transform.position;
            gameObject.SetActive(false); // enemy off= c muere
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage; 
        enemyRend.material = damagedMat; // se cambia temporalmente el color
        Invoke(nameof(ResetEnemyMat), 0.1f);
    }

    void ResetEnemyMat()
    {
        enemyRend.material = baseMat;
    }
}
