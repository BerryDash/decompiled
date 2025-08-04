using UnityEngine;

public class Player : MonoBehaviour
{
	public float jumpForce;

	private Rigidbody2D rb;

	public float groundYPosition = -4.3f;

	private bool isGrounded;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		CheckIfGrounded();
		if (isGrounded && Input.GetKey(KeyCode.Space))
		{
			rb.velocity = Vector2.up * jumpForce;
		}
	}

	private void CheckIfGrounded()
	{
		isGrounded = base.transform.position.y <= groundYPosition + 0.05f;
		rb.gravityScale = (isGrounded ? 0f : 1.5f);
		if (base.transform.position.y < groundYPosition)
		{
			base.transform.position = new Vector2(base.transform.position.x, groundYPosition);
			rb.velocity = new Vector2(rb.velocity.x, 0f);
		}
	}
}
