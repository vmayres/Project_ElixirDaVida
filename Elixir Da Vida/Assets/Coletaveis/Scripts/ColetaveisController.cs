using UnityEngine;
    using System.Collections;

public class ColetaveisController : MonoBehaviour
{
    public float floatAmplitude = 0.25f;
    public float floatSpeed = 2f;
    public float collectRiseSpeed = 2f;
    public float collectRotationSpeed = 180f;

    private Vector3 startPos;
    private bool isCollected = false;

    public CollectibleEffect effect;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // if (!isCollected)
        // {
        //     float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        //     transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        // }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            StartCoroutine(Collect());
        }
    }

    IEnumerator Collect()
    {
        effect.Apply(); // Aplica o efeito individual

        float timer = 0;
        Vector3 start = transform.position;

        while (timer < 1f)
        {
            transform.position += Vector3.up * collectRiseSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * collectRotationSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;  
        }

        Destroy(gameObject);
    }
}
