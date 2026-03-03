using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{

    #region General Variables
    [Header("General References")]
    [SerializeField] Camera fpsCam;
    [SerializeField] Transform shootPoint;
    [SerializeField] LayerMask impactLayer;
    RaycastHit hit;

    [Header("Weapon Parameters")]
    [SerializeField] int damage = 10;
    [SerializeField] float range = 100f;
    [SerializeField] float spread = 0;
    [SerializeField] float shootngCooldown = 0.2f;
    [SerializeField] float reloadTime = 1.5f;
    [SerializeField] bool allowButtonHold = false; // Click falso, mantener verdadero

    [Header("Bullet Management")]
    [SerializeField] int ammoSize = 30;
    [SerializeField] int bulletsPerTap = 1;
    int bulletsLeft;

    [Header("FeedbackReferences")]
    [SerializeField] GameObject impactEffects;

    [Header("Dev - Gun State Bools")]
    [SerializeField] bool shooting;
    [SerializeField] bool canShoot;
    [SerializeField] bool reloading;

    #endregion

    private void Awake()
    {
        bulletsLeft = ammoSize;
        canShoot = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Shoot()
    {
        Vector3 direction = fpsCam.transform.forward;
        direction.x += Random.Range(-spread, spread);
        direction.y += Random.Range(-spread, spread);


        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range, impactLayer))
        {
            Debug.Log(hit.collider.name);
        }
    }

    #region Input Methods

    public void onShoot(InputAction.CallbackContext context)
    {

    }
    public void Reload (InputAction.CallbackContext context)
    {

    }

    #endregion
}