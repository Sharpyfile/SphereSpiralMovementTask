using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereMove : MonoBehaviour
{    
    [Header("Speed")]
    public float maxSpeed = 1.0f;
    public float acceleration = 0.0025f;
    public float currentSpeed = 0.0f;

    [Header("Size of spiral and distance")]
    public float spiralSize = 10.0f;
    public float distance = 0.0f;

    [Header("Material and cooldown")]
    public Material material;
    public float seconds = 5.0f;

    [Header("UI elements")]
    public GameObject panel;
    public Text distanceText;
    public string message;

    [Header("Shrinking")]
    float timeRemaining = 2.5f;
    public float maxTime = 2.5f;
    float timeScale = 0.0f;
    [Header("Particles")]
    public ParticleSystem perticles;
    Vector3 newPosition = Vector3.zero;
    Vector3 newColor = Vector3.zero;    
    bool stopMove = false;
    float timer = 0.0f;
    

    public void StartMovement()
    {
        StartCoroutine("Move");
    }

    public void StopMovement()
    {
        if (!stopMove)
            stopMove = true;
    }

    IEnumerator Move()
    {
        // Keep moving until its close to center of the spiral
        while(spiralSize > 0.1f)
        {
            // Check if speed reached max speed
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration;
                Mathf.Clamp(currentSpeed, 0, maxSpeed);
            }
                
            // Calculate position of the sphere
            newPosition = Vector3.zero;
            newPosition.x = Mathf.Cos(timer) * spiralSize;
            newPosition.z = Mathf.Sin(timer) * spiralSize;
                
            // Calculate distance between previous and new position
            // This method is not precise as it depends on distance between points on arc
            // while method calculates distance in straight line
            // difference is insignificant but its visible (around 0.01f, depending on timing between pauses)
            distance += Vector3.Distance(transform.position, newPosition);

            // Set new position
            transform.position = newPosition;

            // Calculate new color values and set them
            newColor = newPosition;
            newColor.Normalize();

            material.SetColor("_Color", new Color(newColor.x, newColor.y, newColor.z, 1.0f));

            // Keep timer and size of spiral at the same ratio
            spiralSize -= Time.deltaTime * currentSpeed;
            timer += Time.deltaTime * currentSpeed; 
                
            // If pause, set UI elements active, update them and wait for X seconds, then reset speed
            if (stopMove)
            {
                distanceText.text = message + " " + distance.ToString();
                panel.SetActive(true);                
                yield return new WaitForSeconds(seconds);
                stopMove = false;
                panel.SetActive(false);
                currentSpeed = 0.0f;
  
            }
            else
                yield return null;
                         
        }
        timeRemaining = maxTime;
        while(timeRemaining > 0)
        {
            timeScale = timeRemaining / maxTime;
            timeRemaining -= Time.deltaTime;
            transform.localScale = new Vector3(timeScale, timeScale, timeScale);
            yield return null;
        }

        // Play particles and end coroutine
        perticles.Play();
        distanceText.text = message + " " + distance.ToString();
        panel.SetActive(true);
        yield break;
    }
}
