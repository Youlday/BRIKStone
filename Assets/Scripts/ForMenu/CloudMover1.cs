using UnityEngine;

public class CloudMover1 : MonoBehaviour
{
    public float speed = 0.4f;      // скорость движения (чем меньше, тем медленнее)
    public float resetPosition = -0.4f;   // когда облако ушло влево - телепортируем
    public float startPosition = 0.5f;    // куда телепортируем (вправо)

    void Update()
    {
        // Двигаем облако влево
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Если ушло слишком далеко влево
        if (transform.position.x <= resetPosition)
        {
            Vector3 pos = transform.position;
            pos.x = startPosition;
            transform.position = pos;
        }
    }
}

public class NewEmptyCSharpScript
{
    
}
