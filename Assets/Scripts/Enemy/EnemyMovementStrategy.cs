using UnityEngine;

public abstract class EnemyMovementStrategy : MonoBehaviour
{
    [SerializeField] protected float horizontalSpeed = 2f;
    [SerializeField] protected float verticalSpeed = 0.5f;

    protected virtual void Start()
    {
        // Configuraciones iniciales opcionales
        Initialize();
    }

    protected virtual void Update()
    {
        // Método genérico de movimiento
        Move();
    }

    // Método virtual para inicialización
    protected virtual void Initialize()
    {
        // Implementación base o dejar para sobrescribir
    }

    // Método abstracto de movimiento para forzar implementación en clases derivadas
    protected abstract void Move();

    // Método útil para verificar si el objeto está en pantalla
    protected bool IsVisibleOnScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x > -0.1f && screenPoint.x < 1.1f && 
               screenPoint.y > -0.1f && screenPoint.y < 1.1f;
    }

    // Método para destruir si sale de la pantalla
    protected void DestroyIfOffScreen()
    {
        if (!IsVisibleOnScreen())
        {
            Destroy(gameObject);
        }
    }
}

// Ejemplo de implementación concreta
public class LinearEnemyMovement : EnemyMovementStrategy
{
    protected override void Move()
    {
        // Movimiento lineal básico
        transform.Translate(Vector3.left * horizontalSpeed * Time.deltaTime);
        
        // Verificar si está fuera de pantalla
        DestroyIfOffScreen();
    }
}

public class SineWaveEnemyMovement : EnemyMovementStrategy
{
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 2f;

    private float time;

    protected override void Initialize()
    {
        time = 0f;
    }

    protected override void Move()
    {
        time += Time.deltaTime;

        // Movimiento ondulatorio
        float x = -horizontalSpeed * Time.deltaTime;
        float y = Mathf.Sin(time * frequency) * amplitude;

        transform.Translate(new Vector3(x, y, 0));
        
        // Verificar si está fuera de pantalla
        DestroyIfOffScreen();
    }
}

public class CircularEnemyMovement : EnemyMovementStrategy
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private float rotationSpeed = 2f;

    private Vector3 centerPoint;
    private float time;

    protected override void Initialize()
    {
        centerPoint = transform.position;
        time = 0f;
    }

    protected override void Move()
    {
        time += Time.deltaTime;

        // Calcular posición circular
        float x = centerPoint.x + Mathf.Cos(time * rotationSpeed) * radius;
        float y = centerPoint.y + Mathf.Sin(time * rotationSpeed) * radius;

        // Mover horizontalmente
        x -= Time.deltaTime * horizontalSpeed;

        transform.position = new Vector3(x, y, transform.position.z);
        
        // Verificar si está fuera de pantalla
        DestroyIfOffScreen();
    }
}