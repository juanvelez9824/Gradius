using UnityEngine;

public enum EnemyType { Basic, Tough, Boss }

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Configuration")]
    [SerializeField] private EnemyType enemyType = EnemyType.Basic;
    [SerializeField] private int health = 3;
    [SerializeField] private int scoreValue = 100;
    [SerializeField] private float damageFlashDuration = 0.1f;

    [Header("Shooting Configuration")]
    [SerializeField] private GameObject enemyProjectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private bool canShoot = true;

    [Header("Visual Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color damageColor = Color.red;

    // Componentes
    private EnemyMovementStrategy movementStrategy;
    private AudioSource explosionSound;

    // Propiedades
    public EnemyType Type => enemyType;
    public int ScoreValue => scoreValue;

    private void Awake()
    {
        // Obtener componente de movimiento
        movementStrategy = GetComponent<EnemyMovementStrategy>();
        
        // Obtener componente de audio si existe
        explosionSound = GetComponent<AudioSource>();

        // Configurar estrategia de movimiento si no está añadida
        if (movementStrategy == null)
        {
            movementStrategy = gameObject.AddComponent<LinearEnemyMovement>();
        }

        // Iniciar disparos si es necesario
        if (canShoot)
        {
            InvokeRepeating(nameof(Shoot), shootInterval, shootInterval);
        }
    }

    private void Shoot()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing && enemyProjectilePrefab != null && firePoint != null)
        {
            Instantiate(enemyProjectilePrefab, firePoint.position, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        // Reducir vida
        health -= damage;

        // Feedback visual de daño
        StartCoroutine(DamageFlashRoutine());

        // Reproducir sonido de daño (opcional)
        if (explosionSound != null)
        {
            explosionSound.Play();
        }

        // Verificar si el enemigo debe morir
        if (health <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator DamageFlashRoutine()
    {
        // Cambiar color a color de daño
        spriteRenderer.color = damageColor;
        
        // Esperar un breve momento
        yield return new WaitForSeconds(damageFlashDuration);
        
        // Restaurar color original
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        // Aumentar puntuación
        GameManager.Instance.UpdateScore(scoreValue);

        // Efectos de muerte (opcional)
        // Instantiate explosion effect
        
        // Reproducir sonido de explosión
        if (explosionSound != null)
        {
            explosionSound.Play();
        }

        // Destruir enemigo
        Destroy(gameObject);
    }

    // Método para configurar tipo de enemigo dinámicamente
    public void SetEnemyType(EnemyType type)
    {
        enemyType = type;
        
        // Actualizar estrategia de movimiento según tipo
        if (movementStrategy != null)
        {
            Destroy(movementStrategy);
        }

        movementStrategy = gameObject.AddComponent(GetMovementStrategyForType(type)) as EnemyMovementStrategy;
    }

    // Seleccionar estrategia de movimiento según tipo de enemigo
    private System.Type GetMovementStrategyForType(EnemyType type)
    {
        return type switch
        {
            EnemyType.Basic => typeof(LinearEnemyMovement),
            EnemyType.Tough => typeof(SineWaveEnemyMovement),
            EnemyType.Boss => typeof(CircularEnemyMovement),
            _ => typeof(LinearEnemyMovement)
        };
    }

    // Método para configurar parámetros de enemigo
    public void ConfigureEnemy(int newHealth, int newScoreValue, float newShootInterval)
    {
        health = newHealth;
        scoreValue = newScoreValue;
        shootInterval = newShootInterval;

        // Reiniciar intervalo de disparos
        if (canShoot)
        {
            CancelInvoke(nameof(Shoot));
            InvokeRepeating(nameof(Shoot), shootInterval, shootInterval);
        }
    }
}