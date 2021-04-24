using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float walkSpeed=5;
    public float runSpeed=10;
    public float speed = 5;
    Vector2 velocity;


    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        velocity.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        velocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(velocity.x, 0, velocity.y);
    }
}
