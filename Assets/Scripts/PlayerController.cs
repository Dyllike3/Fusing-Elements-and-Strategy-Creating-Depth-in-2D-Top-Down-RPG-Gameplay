using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public float moveSpeed = 1f;
    private Rigidbody2D rb; // Reference to Rigidbody2D

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    // Update is called once per frame
    void Update()
    {
        // Get player input
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Normalize the input to avoid faster diagonal movement
        moveInput = moveInput.normalized * moveSpeed;

        // Move the player using Rigidbody2D
        rb.velocity = moveInput;
    }
}
