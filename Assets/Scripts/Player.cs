using UnityEngine;

public class Player : Car, IDieable
{
    void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.Space))
            ApplyBreaking(1);
        else
            ApplyBreaking(0);
        Move(vertical);
        Rotate(horizontal);
    }

    public void Die()
    {
        Died?.Invoke();
        Debug.Log("Die");
        //Destroy(gameObject);
    }
}
