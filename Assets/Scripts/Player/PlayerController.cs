using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}

public class PlayerController : MonoBehaviour
{

    public Facing facing { get; set; }

    public bool isProjectileOnCooldown { get; set; } = false;
    public bool isIdle { get; set; } = true;

    public bool isDashing { get; set; } = false;
	public bool isDashOnCooldown { get; set; } = false;

    public bool isInMelee { get; set; } = false;
    public bool isMeleeOnCooldown { get; set; } = false;

    private static PlayerController instance;

    public static PlayerController Instance
    {
        get { return instance; }
    }

    private void Awake() 
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            instance = this;
        }
    }

    private void Start()
    {
        facing = Facing.Down;
    }
}
