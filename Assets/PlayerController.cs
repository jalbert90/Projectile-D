using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // The speed of Player moves
    public GameObject projectilePrefab; // The Projectile Object prefab
    public float projectileSpeed = 15.0f; // The initial speed of the Projectile Object
                                          // rest of the variables for dashing..
    public float dashSpeed = 10f; // The speed at which the player dashes
    public KeyCode dashKey = KeyCode.Space; // The key that the player can press to dash
    public float dashCooldown = 1f; // The amount of time the player has to wait before dashing again

    private bool isDashing = false; // Whether the player is currently dashing
    private Vector3 dashDirection; // The direction in which the player is dashing
    private float dashTimer = 0f; // The timer that counts down to the next dash

    void Update()
    {
        // all previous code here 
        // Get input from the user
        float horizontalInput = Input.GetKey("a") ? -1 : Input.GetKey("d") ? 1 : 0;
        float verticalInput = Input.GetKey("w") ? 1 : Input.GetKey("s") ? -1 : 0;

        // Calculate the player's movement
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0).normalized;
        if (isDashing)
        {
            // If the player is dashing, move in the dash direction and disable regular movement
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            moveDirection = Vector3.zero;

            // Stop dashing after a certain amount of time
            isDashing = false;
        }
        else
        {
            // If the player is not dashing, move in the regular direction
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // Check if the player pressed the dash key and the dash timer is not active
            if (Input.GetKeyDown(dashKey) && dashTimer <= 0f)
            {
                // If the player pressed the dash key, start dashing in the direction of the mouse
                isDashing = true;
                dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                dashDirection.z = 0;
                dashDirection.Normalize();

                // Start the dash timer
                dashTimer = dashCooldown;
            }
        }

        // Decrement the dash timer
        dashTimer -= Time.deltaTime;

        // Shooting Mechanism
        if (Input.GetMouseButtonDown(0) && !isDashing)
        {
            // Instantiate a copy of the projectilePrefab object 
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Get the direction between the player's position and the mouse cursor
            Vector2 shootingDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            shootingDirection.Normalize();
            // Adding force to the rigidbody 
            projectile.GetComponent<Rigidbody2D>().velocity = shootingDirection * projectileSpeed;

        }
    }
}